// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using C5;
using MfGames.HierarchicalPaths;

namespace AuthorIntrusion.Plugins.Spelling.LocalWords
{
	/// <summary>
	/// Contains the serialiable settings for the Local Words plugin.
	/// </summary>
	public class LocalWordsSettings
	{
		#region Properties

		public HashSet<string> CaseInsensitiveDictionary { get; private set; }
		public HashSet<string> CaseSensitiveDictionary { get; private set; }

		#endregion

		#region Constructors

		static LocalWordsSettings()
		{
			SettingsPath = new HierarchicalPath("/Plugins/Spelling/Local Words");
		}

		public LocalWordsSettings()
		{
			CaseInsensitiveDictionary = new HashSet<string>();
			CaseSensitiveDictionary = new HashSet<string>();
		}

		#endregion

		#region Fields

		public static readonly HierarchicalPath SettingsPath;

		#endregion
	}
}
