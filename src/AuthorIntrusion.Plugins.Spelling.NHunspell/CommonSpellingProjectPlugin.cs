// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System;
using System.Collections.Generic;
using AuthorIntrusion.Common.Plugins;
using AuthorIntrusion.Plugins.Spelling.Common;
using MfGames.Enumerations;

namespace AuthorIntrusion.Plugins.Spelling.NHunspell
{
	public abstract class CommonSpellingProjectPlugin: IProjectPlugin,
		ISpellingProjectPlugin
	{
		#region Properties

		public string Key
		{
			get { return "NHunspell"; }
		}

		public Importance Weight { get; set; }

		#endregion

		#region Methods

		public virtual IEnumerable<SpellingSuggestion> GetSuggestions(string word)
		{
			throw new NotImplementedException();
		}

		public virtual WordCorrectness IsCorrect(string word)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
