// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System;
using System.IO;
using AuthorIntrusion.Common;
using AuthorIntrusion.Common.Plugins;
using NHunspell;

namespace AuthorIntrusion.Plugins.Spelling.NHunspell
{
	public class NHunspellSpellingPlugin: IProjectPluginProviderPlugin
	{
		#region Properties

		public bool AllowMultiple
		{
			get { return false; }
		}

		/// <summary>
		/// Gets the disabled state for the plugin.
		/// </summary>
		public bool Disabled { get; private set; }

		public string Key
		{
			get { return "NHunspell"; }
		}

		/// <summary>
		/// Gets the spell engine associated with this plugin.
		/// </summary>
		public SpellEngine SpellEngine { get; private set; }

		#endregion

		#region Methods

		public IProjectPlugin GetProjectPlugin(Project project)
		{
			return new NHunspellSpellingProjectPlugin(this);
		}

		#endregion

		#region Constructors

		public NHunspellSpellingPlugin()
		{
			// Set up the spell engine for multi-threaded access.
			SpellEngine = new SpellEngine();
			Disabled = false;

			// For the time being, we are going to assume a en_US dictionary.
			try
			{
				// Get the application directory for the dictionaries.
				string location = Directory.GetParent(GetType().Assembly.Location).FullName;
				string affFilename = Path.Combine(location, "en_US.aff");
				string dicFilename = Path.Combine(location, "en_US.dic");

				var englishUnitedStates = new LanguageConfig
				{
					LanguageCode = "en_US",
					HunspellAffFile = affFilename,
					HunspellDictFile = dicFilename,
					HunspellKey = string.Empty
				};

				SpellEngine.AddLanguage(englishUnitedStates);
			}
			catch (Exception exception)
			{
				// Report what we can't do.
				Console.WriteLine("Cannot load dictionary: " + exception);

				// Disable the spell-checking for now.
				Disabled = true;
			}
		}

		#endregion
	}
}
