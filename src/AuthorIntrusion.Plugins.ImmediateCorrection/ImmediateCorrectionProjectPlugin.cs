// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common;
using AuthorIntrusion.Common.Blocks;
using AuthorIntrusion.Common.Commands;
using AuthorIntrusion.Common.Plugins;
using C5;
using MfGames.Settings;

namespace AuthorIntrusion.Plugins.ImmediateCorrection
{
	public class ImmediateCorrectionProjectPlugin: IImmediateEditorProjectPlugin
	{
		#region Properties

		public ImmediateCorrectionPlugin Plugin { get; set; }
		public Project Project { get; set; }
		public ArrayList<RegisteredSubstitution> Substitutions { get; private set; }

		#endregion

		#region Methods

		public void AddSubstitution(
			string search,
			string replacement,
			SubstitutionOptions options)
		{
			AddSubstitution(Project.Settings, search, replacement, options);
		}

		public void AddSubstitution(
			SettingsManager settingsManager,
			string search,
			string replacement,
			SubstitutionOptions options)
		{
			// Create the substitution we'll be registering.
			var substitution = new RegisteredSubstitution(search, replacement, options);

			// Grab the configuration object for this settings manager or create one if
			// it doesn't already exist.
			var settings =
				settingsManager.Get<ImmediateCorrectionSettings>(
					ImmediateCorrectionSettings.SettingsPath);

			settings.Substitutions.Add(substitution);

			// Mark that our substituions are out of date.
			optimizedSubstitions = false;
		}

		public void ProcessImmediateEdits(
			Block block,
			int textIndex)
		{
			// If we aren't optimized, we have to pull the settings back in from the
			// project settings and optimize them.
			if (!optimizedSubstitions)
			{
				RetrieveSettings();
			}

			// Pull out the edit text and add a leading space to simplify the
			// "whole word" substitutions.
			string editText = block.Text.Substring(0, textIndex);
			char finalCharacter = editText[editText.Length - 1];
			bool isWordBreak = char.IsPunctuation(finalCharacter)
				|| char.IsWhiteSpace(finalCharacter);

			// Go through the substitution elements and look for each one.
			foreach (RegisteredSubstitution substitution in Substitutions)
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
					{
						continue;
					}

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

		/// <summary>
		/// Retrieves the setting substitutions and rebuilds the internal list.
		/// </summary>
		private void RetrieveSettings()
		{
			// Clear out the existing settings.
			Substitutions.Clear();

			// Go through all of the settings in the various projects.
			System.Collections.Generic.IList<ImmediateCorrectionSettings> settingsList =
				Project.Settings.GetAll<ImmediateCorrectionSettings>(
					ImmediateCorrectionSettings.SettingsPath);

			foreach (ImmediateCorrectionSettings settings in settingsList)
			{
				// Go through the substitions inside the settings.
				foreach (RegisteredSubstitution substitution in settings.Substitutions)
				{
					// Check to see if we already have it in the collection.
					if (!Substitutions.Contains(substitution))
					{
						Substitutions.Add(substitution);
					}
				}
			}

			// Clear out the optimization list so we rebuild it on the first request.
			optimizedSubstitions = true;
		}

		#endregion

		#region Constructors

		public ImmediateCorrectionProjectPlugin(
			ImmediateCorrectionPlugin plugin,
			Project project)
		{
			// Save the various properties we need for the controller.
			Plugin = plugin;
			Project = project;

			// Set up the substitions from the configuration settings.
			Substitutions = new ArrayList<RegisteredSubstitution>();
			RetrieveSettings();
		}

		#endregion

		#region Fields

		private bool optimizedSubstitions;

		#endregion
	}
}
