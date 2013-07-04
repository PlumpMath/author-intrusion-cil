// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System;
using System.Collections.Generic;
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
			return projectPlugin;
		}

		/// <summary>
		/// Searches through the calculated paths and attempts to find a
		/// file in that directory.
		/// </summary>
		/// <param name="basename">The basename to look for (en_US.aff).</param>
		/// <param name="filename">The resulting filename, if found..</param>
		/// <returns>True if a file was found, otherwise false.</returns>
		private bool GetDictionaryPath(
			string basename,
			out string filename)
		{
			// Go through all the paths and try to find it.
			foreach (string searchPath in searchPaths)
			{
				// Figure out the full name for this file.
				filename = Path.Combine(searchPath, basename);

				if (File.Exists(filename))
				{
					// We found it, so use this one.
					return true;
				}
			}

			// If we got out of the loop, then we couldn't find it at all.
			filename = null;
			return false;
		}

		/// <summary>
		/// Gets the paths for the affix and dictionary files by searching for
		/// them at various locations in the filesystem.
		/// </summary>
		/// <param name="languageCode">The language code needed.</param>
		/// <param name="affixFilename">The resulting affix filename, if found.</param>
		/// <param name="dictFilename">The resulting dictionary filename, if found.</param>
		/// <returns>True if both the affix and dictionary files were found, otherwise false.</returns>
		private bool GetDictionaryPaths(
			string languageCode,
			out string affixFilename,
			out string dictFilename)
		{
			// Try to get the affix filename.
			string affixBasename = languageCode + ".aff";
			bool affixFound = GetDictionaryPath(affixBasename, out affixFilename);

			if (!affixFound)
			{
				dictFilename = null;
				return false;
			}

			// We have the affix, now try the dictionary.
			string dictBasename = languageCode + ".dic";
			bool dictFound = GetDictionaryPath(dictBasename, out dictFilename);

			return dictFound;
		}

		#endregion

		#region Constructors

		static NHunspellSpellingPlugin()
		{
			// Build up an array of locations we'll look for the dictionaries.
			string assemblyFilename = typeof (NHunspellSpellingPlugin).Assembly.Location;
			string assemblyPath = Directory.GetParent(assemblyFilename).FullName;
			string assemblyDictPath = Path.Combine(assemblyPath, "dicts");

			var paths = new List<string>
			{
				// Add in the paths relative to the assembly.
				assemblyPath,
				assemblyDictPath,

				// Add in the Linux-specific paths.
				"/usr/share/hunspell",
				"/usr/share/myspell",
				"/usr/share/myspell/dicts",

				// Add in the Windows-specific paths.
				"C:\\Program Files\\OpenOffice.org 3\\share\\dict\\ooo",
			};

			searchPaths = paths.ToArray();
		}

		public NHunspellSpellingPlugin()
		{
			// Set up the spell engine for multi-threaded access.
			SpellEngine = new SpellEngine();

			// Assume we are disabled unless we can configure it properly.
			projectPlugin = new DisabledSpellingProjectPlugin();

			// Figure out the paths to the dictionary files. For the time being,
			// we're going to assume we're using U.S. English.
			string affixFilename;
			string dictionaryFilename;

			if (!GetDictionaryPaths("en_US", out affixFilename, out dictionaryFilename))
			{
				// We couldn't get the dictionary paths, so just stick with the
				// disabled spelling plugin.
				return;
			}

			// Attempt to load the NHunspell plugin. This is a high-quality
			// plugin based on Managed C++. This works nicely in Windows, but
			// has no support for Linux.
			try
			{
				// Attempt to load the U.S. English language. This will throw
				// an exception if it cannot load. If it does, then we use it.
				var englishUnitedStates = new LanguageConfig
				{
					LanguageCode = "en_US",
					HunspellAffFile = affixFilename,
					HunspellDictFile = dictionaryFilename,
					HunspellKey = string.Empty
				};

				SpellEngine.AddLanguage(englishUnitedStates);

				// If we got this far, set the project plugin to the one that
				// uses the SpellEngine from NHunspell.
				projectPlugin = new SpellEngineSpellingProjectPlugin(this);
				return;
			}
			catch (Exception exception)
			{
				// Report that we can't load the first attempt.
				Console.WriteLine("Cannot load NHunspell: " + exception);
			}

			// If we got this far, we couldn't load the NHunspell plugin.
			// Attempt to load in the PInvoke implementation instead.
			try
			{
				// Create a new Hunspell P/Invoke loader.
				var pinvokePlugin = new PInvokeSpellingProjectPlugin(
					affixFilename, dictionaryFilename);
				projectPlugin = pinvokePlugin;
			}
			catch (Exception exception)
			{
				// Report that we can't load the first attempt.
				Console.WriteLine("Cannot load NHunspell via P/Invoke: " + exception);
			}
		}

		#endregion

		#region Fields

		private readonly CommonSpellingProjectPlugin projectPlugin;
		private static readonly string[] searchPaths;

		#endregion
	}
}
