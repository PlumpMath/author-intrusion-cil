// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System.Collections.Generic;
using AuthorIntrusion.Plugins.Spelling.Common;
using AuthorIntrusion.Plugins.Spelling.NHunspell.Interop;

namespace AuthorIntrusion.Plugins.Spelling.NHunspell
{
	/// <summary>
	/// A P/Invoke-based spelling plugin for Hunspell.
	/// </summary>
	public class PInvokeSpellingProjectPlugin: CommonSpellingProjectPlugin
	{
		#region Methods

		public override IEnumerable<SpellingSuggestion> GetSuggestions(string word)
		{
			// Get the suggestions from Hunspell.
			string[] words = hunspell.Suggest(word);

			// Wrap each suggested word in a spelling suggestion.
			var suggestions = new List<SpellingSuggestion>();

			foreach (string suggestedWord in words)
			{
				var suggestion = new SpellingSuggestion(suggestedWord);
				suggestions.Add(suggestion);
			}

			// Return the resulting suggestions.
			return suggestions;
		}

		public override WordCorrectness IsCorrect(string word)
		{
			bool results = hunspell.CheckWord(word);
			return results
				? WordCorrectness.Correct
				: WordCorrectness.Incorrect;
		}

		#endregion

		#region Constructors

		public PInvokeSpellingProjectPlugin(
			string affixFilename,
			string dictionaryFilename)
		{
			// Create the Hunspell wrapper.
			hunspell = new Hunspell(affixFilename, dictionaryFilename);
		}

		#endregion

		#region Fields

		private readonly Hunspell hunspell;

		#endregion
	}
}
