// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System;
using System.IO;
using AuthorIntrusion.Common.Blocks;
using AuthorIntrusion.Common.Blocks.Locking;
using AuthorIntrusion.Common.Persistence.Filesystem;
using AuthorIntrusion.Common.Plugins;
using AuthorIntrusion.Common.Projects;
using MfGames.HierarchicalPaths;

namespace AuthorIntrusion.Common.Persistence
{
	/// <summary>
	/// The project plugin to describe an export/save format for a project.
	/// </summary>
	public class FilesystemPersistenceProjectPlugin: IProjectPlugin
	{
		#region Properties

		public string Key
		{
			get { return "Filesystem Persistence"; }
		}

		public Guid PluginId { get; private set; }
		public Project Project { get; private set; }
		public static HierarchicalPath RootSettingsKey { get; private set; }

		/// <summary>
		/// Gets the settings associated with this plugin.
		/// </summary>
		public FilesystemPersistenceSettings Settings
		{
			get
			{
				HierarchicalPath key = SettingsKey;
				var settings = Project.Settings.Get<FilesystemPersistenceSettings>(key);
				return settings;
			}
		}

		public HierarchicalPath SettingsKey { get; private set; }

		#endregion

		#region Methods

		/// <summary>
		/// Writes out the project file to a given directory.
		/// </summary>
		/// <param name="directory">The directory to save the file.</param>
		public void Save(DirectoryInfo directory)
		{
			// Set up the project macros we'll be expanding.
			ProjectMacros macros = SetupMacros(directory);

			// Validate the state.
			string projectFilename = macros.ExpandMacros("{ProjectFilename}");

			if (string.IsNullOrWhiteSpace(projectFilename))
			{
				throw new InvalidOperationException(
					"Project filename is not defined in Settings property.");
			}

			// We need a write lock on the blocks to avoid changes. This also prevents
			// any background tasks from modifying the blocks during the save process.
			ProjectBlockCollection blocks = Project.Blocks;

			using (blocks.AcquireLock(RequestLock.Write))
			{
				// Create a new project writer and write out the results.
				var projectWriter = new FilesystemPersistenceProjectWriter(
					Project, Settings, macros);
				var projectFile = new FileInfo(projectFilename);

				projectWriter.Write(projectFile);
			}
		}

		/// <summary>
		/// Setups the macros.
		/// </summary>
		/// <param name="directory">The directory.</param>
		/// <returns>The populated macros object.</returns>
		private ProjectMacros SetupMacros(DirectoryInfo directory)
		{
			// Create the macros and substitute the values.
			var macros = new ProjectMacros();

			macros.Substitutions["ProjectDirectory"] = directory.FullName;
			macros.Substitutions["ProjectFilename"] = Settings.ProjectFilename;
			macros.Substitutions["DataDirectory"] = Settings.DataDirectory;
			macros.Substitutions["InternalContentDirectory"] =
				Settings.InternalContentDirectory;
			macros.Substitutions["ExternalSettingsDirectory"] =
				Settings.ExternalSettingsDirectory;

			// Return the resulting macros.
			return macros;
		}

		#endregion

		#region Constructors

		static FilesystemPersistenceProjectPlugin()
		{
			RootSettingsKey = new HierarchicalPath("/Persistence/Filesystem/");
		}

		public FilesystemPersistenceProjectPlugin(Project project)
		{
			Project = project;
			PluginId = Guid.NewGuid();
			SettingsKey = RootSettingsKey;
			// TODO new HierarchicalPath(PluginId.ToString(),RootSettingsKey);
		}

		#endregion

		#region Fields

		private const string ProjectNamespace = XmlConstants.ProjectNamespace;

		#endregion
	}
}
