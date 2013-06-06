// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System.Collections.Generic;
using AuthorIntrusion.Common;
using AuthorIntrusion.Common.Plugins;
using AuthorIntrusion.Plugins.Spelling.Common;
using MfGames.Enumerations;

namespace AuthorIntrusion.Plugins.Spelling.LocalWords
{
	/// <summary>
	/// Defines a "local words" dictionary plugin. This functions like Emacs'
	/// LocalWords lines in a file in that it identifies correct spelling of files.
	/// It has both a case sensitive and case insensitive version (the latter being
	/// the same as all lowercase terms in Emacs).
	/// </summary>
	public class LocalWordsController: IProjectPluginController,
		ISpellingController
	{
		#region Properties

		public C5.HashSet<string> CaseInsensitiveDictionary { get; private set; }
		public C5.HashSet<string> CaseSensitiveDictionary { get; private set; }
		public Importance Weight { get; set; }
		private Project Project { get; set; }

		#endregion

		#region Methods

		public IEnumerable<SpellingSuggestion> GetSuggestions(string word)
		{
			// The local words controller doesn't provide suggestions at all.
			return new SpellingSuggestion[]
			{
			};
		}

		public bool IsCorrect(string word)
		{
			// First check the case-sensitive dictionary.
			bool isCaseSensitiveCorrect = CaseSensitiveDictionary.Contains(word);

			if (isCaseSensitiveCorrect)
			{
				return true;
			}

			// Check the case-insensitive version by making it lowercase and trying
			// again.
			word = word.ToLowerInvariant();

			bool isCaseInsensitiveCorrect = CaseInsensitiveDictionary.Contains(word);

			return isCaseInsensitiveCorrect;
		}

		/// <summary>
		/// Retrieves the setting substitutions and rebuilds the internal list.
		/// </summary>
		public void RetrieveSettings()
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
				CaseInsensitiveDictionary.AddAll(settings.CaseInsensitiveDictionary);
				CaseSensitiveDictionary.AddAll(settings.CaseSensitiveDictionary);
			}
		}

		#endregion

		#region Constructors

		public LocalWordsController(Project project)
		{
			// Save the variables so we can access them later.
			Project = project;

			// Create the collections we'll use.
			CaseInsensitiveDictionary = new C5.HashSet<string>();
			CaseSensitiveDictionary = new C5.HashSet<string>();

			// Load in the initial settings.
			RetrieveSettings();
		}

		#endregion
	}
}
