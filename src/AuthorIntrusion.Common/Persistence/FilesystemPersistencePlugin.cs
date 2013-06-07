// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Plugins;

namespace AuthorIntrusion.Common.Persistence
{
	/// <summary>
	/// Defines a persistence plugin for handling reading and writing to the filesystem.
	/// </summary>
	public class FilesystemPersistencePlugin: IPersistencePlugin
	{
		#region Properties

		public bool AllowMultiple
		{
			get { return true; }
		}

		public string Name
		{
			get { return "Filesystem Persistence"; }
		}

		#endregion

		#region Methods

		public IProjectPlugin GetProjectPlugin(Project project)
		{
			var projectPlugin = new FilesystemPersistenceProjectPlugin(project);
			return projectPlugin;
		}

		#endregion
	}
}
