// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System.Collections.Generic;
using AuthorIntrusion.Common;
using AuthorIntrusion.Common.Actions;
using AuthorIntrusion.Common.Commands;
using AuthorIntrusion.Common.Plugins;
using AuthorIntrusion.Plugins.Spelling.Common;
using MfGames.Enumerations;
using MfGames.HierarchicalPaths;

namespace AuthorIntrusion.Plugins.Spelling.LocalWords
{
	/// <summary>
	/// Defines a "local words" dictionary plugin. This functions like Emacs'
	/// LocalWords lines in a file in that it identifies correct spelling of files.
	/// It has both a case sensitive and case insensitive version (the latter being
	/// the same as all lowercase terms in Emacs).
	/// </summary>
	public class LocalWordsProjectPlugin: IProjectPlugin,
		ISpellingProjectPlugin
	{
		#region Properties

		public HashSet<string> CaseInsensitiveDictionary { get; private set; }
		public HashSet<string> CaseSensitiveDictionary { get; private set; }

		public string Key
		{
			get { return "Local Words"; }
		}

		public Importance Weight { get; set; }
		private Project Project { get; set; }

		#endregion

		#region Methods

		public IEnumerable<IEditorAction> GetAdditionalEditorActions(string word)
		{
			// We have two additional editor actions.
			var addSensitiveAction = new EditorAction(
				"Add case-sensitive local words",
				new HierarchicalPath("/Plugins/Local Words/Add to Sensitive"),
				context => AddToSensitiveList(context, word));
			var addInsensitiveAction =
				new EditorAction(
					"Add case-insensitive local words",
					new HierarchicalPath("/Plugins/Local Words/Add to Insensitive"),
					context => AddToInsensitiveList(context, word));

			// Return the resutling list.
			var results = new IEditorAction[]
			{
				addSensitiveAction, addInsensitiveAction
			};

			return results;
		}

		public IEnumerable<SpellingSuggestion> GetSuggestions(string word)
		{
			// The local words controller doesn't provide suggestions at all.
			return new SpellingSuggestion[]
			{
			};
		}

		public WordCorrectness IsCorrect(string word)
		{
			// First check the case-sensitive dictionary.
			bool isCaseSensitiveCorrect = CaseSensitiveDictionary.Contains(word);

			if (isCaseSensitiveCorrect)
			{
				return WordCorrectness.Correct;
			}

			// Check the case-insensitive version by making it lowercase and trying
			// again.
			word = word.ToLowerInvariant();

			bool isCaseInsensitiveCorrect = CaseInsensitiveDictionary.Contains(word);

			// The return value is either correct or indeterminate since this
			// plugin is intended to be a supplemental spell-checking instead of
			// a conclusive one.
			return isCaseInsensitiveCorrect
				? WordCorrectness.Correct
				: WordCorrectness.Indeterminate;
		}

		/// <summary>
		/// Retrieves the setting substitutions and rebuilds the internal list.
		/// </summary>
		public void ReadSettings()
		{
			// Clear out the existing settings.
			CaseInsensitiveDictionary.Clear();
			CaseSensitiveDictionary.Clear();

			// Go through all of the settings in the various projects.
			IList<LocalWordsSettings> settingsList =
				Project.Settings.GetAll<LocalWordsSettings>(LocalWordsSettings.SettingsPath);

			foreach (LocalWordsSettings settings in settingsList)
			{
				// Add the two dictionaries.
				foreach (string word in settings.CaseInsensitiveDictionary)
				{
					CaseInsensitiveDictionary.Add(word);
				}

				foreach (string word in settings.CaseSensitiveDictionary)
				{
					CaseSensitiveDictionary.Add(word);
				}
			}
		}

		/// <summary>
		/// Write out the settings to the settings.
		/// </summary>
		public void WriteSettings()
		{
			// Get the settings and clear out the lists.
			var settings =
				Project.Settings.Get<LocalWordsSettings>(LocalWordsSettings.SettingsPath);

			settings.CaseInsensitiveDictionary.Clear();
			settings.CaseSensitiveDictionary.Clear();

			// Add in the words from the settings.
			foreach (string word in CaseInsensitiveDictionary)
			{
				settings.CaseInsensitiveDictionary.Add(word);
			}

			foreach (string word in CaseSensitiveDictionary)
			{
				settings.CaseSensitiveDictionary.Add(word);
			}
		}

		private void AddToInsensitiveList(
			BlockCommandContext context,
			string word)
		{
			// Update the internal dictionaries.
			CaseInsensitiveDictionary.Add(word.ToLowerInvariant());

			// Make sure the settings are written out.
			WriteSettings();
		}

		private void AddToSensitiveList(
			BlockCommandContext context,
			string word)
		{
			// Update the internal dictionaries.
			CaseInsensitiveDictionary.Remove(word.ToLowerInvariant());
			CaseSensitiveDictionary.Add(word);

			// Make sure the settings are written out.
			WriteSettings();
		}

		#endregion

		#region Constructors

		public LocalWordsProjectPlugin(Project project)
		{
			// Save the variables so we can access them later.
			Project = project;

			// Create the collections we'll use.
			CaseInsensitiveDictionary = new HashSet<string>();
			CaseSensitiveDictionary = new HashSet<string>();

			// Load in the initial settings.
			ReadSettings();
		}

		#endregion
	}
}
