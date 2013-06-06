﻿// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common;
using AuthorIntrusion.Common.Plugins;
using NHunspell;

namespace AuthorIntrusion.Plugins.Spelling.NHunspell
{
	public class NHunspellSpellingPlugin: IProjectPlugin
	{
		#region Properties

		public bool AllowMultiple
		{
			get { return false; }
		}

		public string Name
		{
			get { return "NHunspell"; }
		}

		/// <summary>
		/// Gets the spell engine associated with this plugin.
		/// </summary>
		public SpellEngine SpellEngine { get; private set; }

		#endregion

		#region Methods

		public IProjectPluginController GetController(Project project)
		{
			return new NHunspellSpellingController(this);
		}

		#endregion

		#region Constructors

		public NHunspellSpellingPlugin()
		{
			// Set up the spell engine for multi-threaded access.
			SpellEngine = new SpellEngine();

			// For the time being, we are going to assume a en_US dictionary.
			var englishUnitedStates = new LanguageConfig
			{
				LanguageCode = "en_US",
				HunspellAffFile = "en_US.aff",
				HunspellDictFile = "en_US.dic",
				HunspellKey = string.Empty
			};

			SpellEngine.AddLanguage(englishUnitedStates);
		}

		#endregion
	}
}
