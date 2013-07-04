// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System.Collections.Generic;
using System.Linq;
using AuthorIntrusion.Plugins.Spelling.Common;
using NHunspell;
using IStringList = System.Collections.Generic.IList<string>;

namespace AuthorIntrusion.Plugins.Spelling.NHunspell
{
	/// <summary>
	/// Controller that uses the NHunspell to handle spell-checking.
	/// </summary>
	public class SpellEngineSpellingProjectPlugin: CommonSpellingProjectPlugin
	{
		#region Properties

		private NHunspellSpellingPlugin Plugin { get; set; }

		#endregion

		#region Methods

		public override IEnumerable<SpellingSuggestion> GetSuggestions(string word)
		{
			// Get the checker and then get the suggestions.
			SpellFactory checker = Plugin.SpellEngine["en_US"];
			IStringList suggestedWords = checker.Suggest(word);

			// Wrap the list in suggestions.
			var suggestions = new List<SpellingSuggestion>(suggestedWords.Count);

			suggestions.AddRange(
				suggestedWords.Select(
					suggestedWord => new SpellingSuggestion(suggestedWord)));

			// Return the resulting suggestions.
			return suggestions;
		}

		public override WordCorrectness IsCorrect(string word)
		{
			// Check the spelling.
			SpellFactory checker = Plugin.SpellEngine["en_US"];
			bool isCorrect = checker.Spell(word);
			return isCorrect
				? WordCorrectness.Correct
				: WordCorrectness.Incorrect;
		}

		#endregion

		#region Constructors

		public SpellEngineSpellingProjectPlugin(NHunspellSpellingPlugin plugin)
		{
			Plugin = plugin;
		}

		#endregion
	}
}
