// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System;
using System.Collections.Generic;
using AuthorIntrusion.Common.Actions;
using AuthorIntrusion.Common.Plugins;
using AuthorIntrusion.Plugins.Spelling.Common;
using MfGames.Enumerations;

namespace AuthorIntrusion.Plugins.Spelling.NHunspell
{
	/// <summary>
	/// Contains the common project plugin settings for the Hunspell spelling.
	/// There are multiple versions of this, based on how and if it can be
	/// loaded.
	/// </summary>
	public abstract class CommonSpellingProjectPlugin: IProjectPlugin,
		ISpellingProjectPlugin
	{
		#region Properties

		public IBlockAnalyzerProjectPlugin BlockAnalyzer { get; set; }

		public string Key
		{
			get { return "Hunspell"; }
		}

		public Importance Weight { get; set; }

		#endregion

		#region Methods

		public IEnumerable<IEditorAction> GetAdditionalEditorActions(string word)
		{
			return new IEditorAction[]
			{
			};
		}

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
