// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System.Collections.Generic;
using AuthorIntrusion.Plugins.Spelling.Common;

namespace AuthorIntrusion.Plugins.Spelling.NHunspell
{
	public class DisabledSpellingProjectPlugin: CommonSpellingProjectPlugin
	{
		#region Methods

		public override IEnumerable<SpellingSuggestion> GetSuggestions(string word)
		{
			// If the plugin is disabled, then don't do anything.
			return new SpellingSuggestion[]
			{
			};
		}

		public override WordCorrectness IsCorrect(string word)
		{
			// Because we are disabled, we are always indeterminate.
			return WordCorrectness.Indeterminate;
		}

		#endregion
	}
}
