// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System.Collections.Generic;
using AuthorIntrusion.Common.Plugins;
using AuthorIntrusion.Plugins.Spelling.Common;
using C5;
using MfGames.Enumerations;
using NHunspell;
using IStringList = System.Collections.Generic.IList<string>;

namespace AuthorIntrusion.Plugins.Spelling.NHunspell
{
	/// <summary>
	/// Controller that uses the NHunspell to handle spell-checking.
	/// </summary>
	public class NHunspellSpellingProjectPlugin: IProjectPlugin,
		ISpellingProjectPlugin
	{
		#region Properties

		public string Key
		{
			get { return "NHunspell"; }
		}

		public Importance Weight { get; set; }
		private NHunspellSpellingPlugin Plugin { get; set; }

		#endregion

		#region Methods

		public IEnumerable<SpellingSuggestion> GetSuggestions(string word)
		{
			// If the plugin is disabled, then don't do anything.
			if (Plugin.Disabled)
			{
				return new SpellingSuggestion[]
				{
				};
			}

			// Get the checker and then get the suggestions.
			SpellFactory checker = Plugin.SpellEngine["en_US"];
			IStringList suggestedWords = checker.Suggest(word);

			// Wrap the list in suggestions.
			var suggestions = new ArrayList<SpellingSuggestion>(suggestedWords.Count);

			foreach (string suggestedWord in suggestedWords)
			{
				suggestions.Add(new SpellingSuggestion(suggestedWord));
			}

			// Return the resulting suggestions.
			return suggestions;
		}

		public bool IsCorrect(string word)
		{
			// If the plugin is disabled, then don't do anything.
			if (Plugin.Disabled)
			{
				return false;
			}

			// Check the spelling.
			SpellFactory checker = Plugin.SpellEngine["en_US"];
			bool isCorrect = checker.Spell(word);
			return isCorrect;
		}

		#endregion

		#region Constructors

		public NHunspellSpellingProjectPlugin(NHunspellSpellingPlugin plugin)
		{
			Plugin = plugin;
		}

		#endregion
	}
}
