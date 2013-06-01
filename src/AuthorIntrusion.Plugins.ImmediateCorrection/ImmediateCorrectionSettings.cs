// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using C5;
using MfGames.HierarchicalPaths;

namespace AuthorIntrusion.Plugins.ImmediateCorrection
{
	/// <summary>
	/// A configuration settings for an immediate correction controller.
	/// </summary>
	public class ImmediateCorrectionSettings
	{
		#region Properties

		public static HierarchicalPath SettingsPath { get; private set; }
		public ArrayList<RegisteredSubstitution> Substitutions { get; set; }

		#endregion

		#region Constructors

		static ImmediateCorrectionSettings()
		{
			SettingsPath = new HierarchicalPath("/Plugins/Immediate Correction");
		}

		public ImmediateCorrectionSettings()
		{
			Substitutions = new ArrayList<RegisteredSubstitution>();
		}

		#endregion
	}
}
