// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common;
using AuthorIntrusion.Common.Blocks;
using AuthorIntrusion.Common.Commands;
using AuthorIntrusion.Common.Plugins;
using C5;

namespace AuthorIntrusion.Plugins.ImmediateCorrection
{
	public class ImmediateCorrectionController: IImmediateBlockEditor
	{
		#region Properties

		public ImmediateCorrectionPlugin Plugin { get; set; }
		public Project Project { get; set; }

		private ArrayList<Substitution> Substitutions { get; set; }

		#endregion

		#region Methods

		public void AddSubstitution(
			string search,
			string replacement,
			SubstitutionOptions options)
		{
			var substitution = new Substitution(search, replacement, options);

			Substitutions.Add(substitution);
			Substitutions.Sort();
		}

		public void CheckForImmediateEdits(
			Block block,
			int textIndex)
		{
			// Pull out the edit text and add a leading space to simplify the
			// "whole word" substitutions.
			string editText = block.Text.Substring(0, textIndex);
			char finalCharacter = editText[editText.Length - 1];
			bool isWordBreak = char.IsPunctuation(finalCharacter)
				|| char.IsWhiteSpace(finalCharacter);

			// Go through the substitution elements and look for each one.
			foreach (Substitution substitution in Substitutions)
			{
				// If we are doing whole word searches, then we don't bother if
				// the final character isn't a word break or if it isn't a word
				// break before it.
				ReplaceTextCommand command;
				int searchLength = substitution.Search.Length;
				int startSearchIndex = editText.Length - searchLength;

				if (substitution.IsWholeWord)
				{
					// Check to see if we have a valid search term.
					if (!isWordBreak)
						continue;

					if (startSearchIndex > 0
						&& char.IsPunctuation(editText[startSearchIndex - 1]))
					{
						continue;
					}

					// Make sure the string we're looking at actually is the same.
					string editSubstring = editText.Substring(
						startSearchIndex - 1, substitution.Search.Length);

					if (editSubstring != substitution.Search)
					{
						// The words don't match.
						continue;
					}

					// Perform the substitution with a replace operation.
					command =
						new ReplaceTextCommand(
							new BlockPosition(block.BlockKey, startSearchIndex - 1),
							searchLength + 1,
							substitution.Replacement + finalCharacter);
				}
				else
				{
					// Perform a straight comparison search.
					if (!editText.EndsWith(substitution.Search))
					{
						continue;
					}

					// Figure out the replace operation.
					command =
						new ReplaceTextCommand(
							new BlockPosition(block.BlockKey, startSearchIndex),
							searchLength,
							substitution.Replacement);
				}

				// Add the command to the deferred execution so the command could
				// be properly handled via the undo/redo management.
				block.Project.Commands.DeferredDo(command);
			}
		}

		#endregion

		#region Constructors

		public ImmediateCorrectionController(
			ImmediateCorrectionPlugin plugin,
			Project project)
		{
			Plugin = plugin;
			Project = project;
			Substitutions = new ArrayList<Substitution>();
		}

		#endregion

		#region Nested Type: Substitution

		private class Substitution
		{
			#region Properties

			public bool IsWholeWord
			{
				get { return (Options & SubstitutionOptions.WholeWord) != 0; }
			}

			private string OriginalSearch { get; set; }

			#endregion

			#region Constructors

			public Substitution(
				string search,
				string replacement,
				SubstitutionOptions options)
			{
				// Save the fields for the substitution.
				Search = search;
				Replacement = replacement;
				Options = options;
			}

			#endregion

			#region Fields

			public readonly SubstitutionOptions Options;
			public readonly string Replacement;
			public readonly string Search;

			#endregion
		}

		#endregion
	}
}
