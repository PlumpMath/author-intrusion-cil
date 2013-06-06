// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Plugins;

namespace AuthorIntrusion.Common.Persistance
{
	/// <summary>
	/// Defines a system plugin for handling the persistance layer. This manages the
	/// various ways a file can be loaded and saved from the filesystem and network.
	/// </summary>
	public class PersistanceFrameworkPlugin: IPlugin
	{
		#region Properties

		public bool AllowMultiple
		{
			get { return false; }
		}

		public string Name
		{
			get { return "Persistance Framework"; }
		}

		#endregion

		#region Methods

		public IProjectPlugin GetProjectPlugin(Project project)
		{
			return null;
		}

		#endregion
	}
}
