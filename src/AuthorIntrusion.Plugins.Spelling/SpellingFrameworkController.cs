// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System.Collections.Generic;
using System.Linq;
using AuthorIntrusion.Common;
using AuthorIntrusion.Common.Actions;
using AuthorIntrusion.Common.Blocks;
using AuthorIntrusion.Common.Commands;
using AuthorIntrusion.Common.Plugins;
using AuthorIntrusion.Plugins.Spelling.Common;
using C5;
using MfGames.HierarchicalPaths;

namespace AuthorIntrusion.Plugins.Spelling
{
	/// <summary>
	/// Project-specific controller for handling the spelling framework.
	/// </summary>
	public class SpellingFrameworkController: IPluginFrameworkController,
		IBlockAnalyzerController,
		ITextSpanController
	{
		#region Properties

		private ArrayList<ISpellingController> SpellingControllers { get; set; }
		private SpellingWordSplitter Splitter { get; set; }

		#endregion

		#region Methods

		public void AnalyzeBlock(
			Block block,
			int blockVersion)
		{
			// Grab the information about the block.
			string text;

			using (block.AcquireReadLock())
			{
				// If we are stale, then break out.
				if (block.IsStale(blockVersion))
				{
					return;
				}

				// Grab the information from the block.
				text = block.Text;
			}

			// Split the word and perform spell-checking.
			var misspelledWords = new ArrayList<TextSpan>();
			C5.IList<TextSpan> words = Splitter.SplitAndNormalize(text);

			foreach (TextSpan span in words.Where(span => !IsCorrect(span.GetText(text)))
				)
			{
				// We aren't correct, so add it to the list.
				span.Controller = this;

				misspelledWords.Add(span);
			}

			// Inside a write lock, we need to make modifications to the block's list.
			using (block.AcquireWriteLock())
			{
				// Check one last time to see if the block is stale.
				if (block.IsStale(blockVersion))
				{
					return;
				}

				// Make the changes to the block's contents.
				block.TextSpans.Remove(this);
				block.TextSpans.AddAll(misspelledWords);
			}
		}

		/// <summary>
		/// Gets the editor actions associated with the given TextSpan.
		/// </summary>
		/// <param name="block">The block.</param>
		/// <param name="textSpan">The text span.</param>
		/// <returns>
		/// A list of editor actions associated with this span.
		/// </returns>
		/// <remarks>
		/// This will be called within a read-only lock.
		/// </remarks>
		public C5.IList<IEditorAction> GetEditorActions(
			Block block,
			TextSpan textSpan)
		{
			// We only get to this point if we have a misspelled word.
			string word = textSpan.GetText(block.Text);

			// Get the suggestions for the word.
			C5.IList<SpellingSuggestion> suggestions = GetSuggestions(word);

			// Go through the suggestions and create an editor action for each one.
			// These will already be ordered coming out of the GetSuggestions()
			// method.
			BlockCommandSupervisor commands = block.Project.Commands;
			var actions = new ArrayList<IEditorAction>(suggestions.Count);

			foreach (SpellingSuggestion suggestion in suggestions)
			{
				// Figure out the operation we'll be using to implement the change.
				var command =
					new ReplaceTextCommand(
						new BlockPosition(block.BlockKey, textSpan.StartTextIndex),
						textSpan.Length,
						suggestion.Suggestion);

				// Create the suggestion action, along with the replacement command.
				var action =
					new EditorAction(
						string.Format("Change to \"{0}\"", suggestion.Suggestion),
						new HierarchicalPath("/Plugins/Spelling/Change"),
						() => commands.Do(command));

				actions.Add(action);
			}

			// Return all the change actions.
			return actions;
		}

		public void HandleAddedController(
			Project project,
			IProjectPluginController controller)
		{
			var spellingController = controller as ISpellingController;

			if (spellingController != null)
			{
				SpellingControllers.Remove(spellingController);
				SpellingControllers.Add(spellingController);
			}
		}

		public void HandleRemovedController(
			Project project,
			IProjectPluginController controller)
		{
			var spellingController = controller as ISpellingController;

			if (spellingController != null)
			{
				SpellingControllers.Remove(spellingController);
			}
		}

		public void InitializePluginFramework(
			Project project,
			IEnumerable<IProjectPluginController> controllers)
		{
			foreach (IProjectPluginController controller in controllers)
			{
				HandleAddedController(project, controller);
			}
		}

		private C5.IList<SpellingSuggestion> GetSuggestions(string word)
		{
			// Gather up all the suggestions from all the controllers.
			var suggestions = new ArrayList<SpellingSuggestion>();

			foreach (ISpellingController controller in SpellingControllers)
			{
				// Get the suggestions from the controller.
				IEnumerable<SpellingSuggestion> controllerSuggestions =
					controller.GetSuggestions(word);

				// Go through each one and add them, removing lower priority ones
				// as we process.
				foreach (SpellingSuggestion controllerSuggestion in controllerSuggestions)
				{
					// If we already have it and its lower priority, then skip it.
					if (suggestions.Contains(controllerSuggestion))
					{
					}

					// Add it to the list.
					suggestions.Add(controllerSuggestion);
				}
			}

			// Sort the suggestions by priority.
			//suggestions.Sort();

			// Return the suggestions so they can be processed.
			return suggestions;
		}

		private bool IsCorrect(string word)
		{
			return SpellingControllers.Any(controller => controller.IsCorrect(word));
		}

		#endregion

		#region Constructors

		public SpellingFrameworkController()
		{
			SpellingControllers = new ArrayList<ISpellingController>();
			Splitter = new SpellingWordSplitter();
		}

		#endregion
	}
}
