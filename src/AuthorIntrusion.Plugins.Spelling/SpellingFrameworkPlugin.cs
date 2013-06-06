// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common;
using AuthorIntrusion.Common.Plugins;

namespace AuthorIntrusion.Plugins.Spelling
{
	/// <summary>
	/// Primary plugin for the entire spelling (spell-checking) framework. This
	/// base plugin is used to register and coordinate other spelling plugins.
	/// </summary>
	public class SpellingFrameworkPlugin: IProjectPlugin
	{
		#region Properties

		public bool AllowMultiple
		{
			get { return false; }
		}

		public string Name
		{
			get { return "Spelling"; }
		}

		#endregion

		#region Methods

		public IProjectPluginController GetController(Project project)
		{
			return new SpellingFrameworkController();
		}

		#endregion
	}
}
