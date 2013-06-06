// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common;
using AuthorIntrusion.Common.Plugins;

namespace AuthorIntrusion.Plugins.ImmediateCorrection
{
	/// <summary>
	/// Implements an immediate editor that checks for inline corrections while the
	/// user is typing. This establishes the basic framework for other plugins to
	/// provide the corrections.
	/// </summary>
	public class ImmediateCorrectionPlugin: IProjectPluginProviderPlugin
	{
		#region Properties

		public bool AllowMultiple
		{
			get { return false; }
		}

		public string Name
		{
			get { return "Immediate Correction"; }
		}

		#endregion

		#region Methods

		public IProjectPlugin GetProjectPlugin(Project project)
		{
			var controller = new ImmediateCorrectionProjectPlugin(this, project);
			return controller;
		}

		#endregion
	}
}
