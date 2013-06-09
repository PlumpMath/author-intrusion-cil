// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System;
using System.IO;
using System.Linq;
using C5;

namespace AuthorIntrusion.Common.Persistence
{
	/// <summary>
	/// Manager class for handling reading files from the file system.
	/// </summary>
	public class PersistenceManager
	{
		#region Properties

		/// <summary>
		/// Gets or sets the singleton instance of the persistence manager.
		/// </summary>
		public static PersistenceManager Instance { get; set; }

		#endregion

		#region Methods

		public Project ReadProject(FileInfo projectFile)
		{
			// Make sure we have a sane state.
			if (projectFile == null)
			{
				throw new ArgumentNullException("projectFile");
			}

			// Query the plugins to determine which persistence plugins are capable
			// of reading this project.
			var validPlugins = new ArrayList<IPersistencePlugin>();

			foreach (IPersistencePlugin persistencePlugin in
				plugin.PersistentPlugins.Where(
					persistencePlugin => persistencePlugin.CanRead(projectFile)))
			{
				validPlugins.Add(persistencePlugin);
			}

			// If we don't have a plugin, then we have nothing that will open it.
			if (validPlugins.IsEmpty)
			{
				throw new FileLoadException("Cannot load the project file: " + projectFile);
			}

			// If we have more than one plugin, we can't handle it.
			if (validPlugins.Count > 1)
			{
				throw new FileLoadException(
					"Too many plugins claim they can read the file: " + projectFile);
			}

			// Pass the loading process to the actual plugin we'll be using.
			IPersistencePlugin persistentPlugin = validPlugins[0];

			Project project = persistentPlugin.ReadProject(projectFile);
			return project;
		}

		#endregion

		#region Constructors

		public PersistenceManager(PersistenceFrameworkPlugin plugin)
		{
			this.plugin = plugin;
		}

		#endregion

		#region Fields

		private readonly PersistenceFrameworkPlugin plugin;

		#endregion
	}
}
