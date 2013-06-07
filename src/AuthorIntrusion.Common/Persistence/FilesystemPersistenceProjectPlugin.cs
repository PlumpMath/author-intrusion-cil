// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System;
using System.IO;
using System.Text;
using System.Xml;
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
		public const string ProjectNamespace = "urn:mfgames.com/author-intrusion/project/0";

		#region Properties

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
			string projectFilename = macros.ExpandMacros("<ProjectPath>");

			if (string.IsNullOrWhiteSpace(projectFilename))
			{
				throw new InvalidOperationException(
					"Project filename is not defined in Settings property.");
			}

			// Create the project file.
			using (XmlWriter writer = SaveProjectFile(new FileInfo(projectFilename)))
			{
				// Finish up by closing the project file.
				writer.Close();
			}
		}

		/// <summary>
		/// Saves the project file to the given path.
		/// </summary>
		/// <param name="file">The file.</param>
		private XmlWriter SaveProjectFile(FileInfo file)
		{
			// Set up the XML writer.
			XmlWriterSettings writerSettings = CreateXmlSettings();
			XmlWriter writer = XmlWriter.Create(file.FullName, writerSettings);

			// Start the document and tag.
			writer.WriteStartDocument(true);
			writer.WriteStartElement("ai", "project", ProjectNamespace);

			// Return the resulting writer. We don't finish the outermost XML
			// tag since we'll handle it through the close method.
			return writer;
		}

		private static XmlWriterSettings CreateXmlSettings()
		{
			var settings = new XmlWriterSettings
			{
				Encoding = Encoding.UTF8,
				Indent = true,
				IndentChars = "\t",
			};

			return settings;
		}

		/// <summary>
		/// Configures a standard file layout that uses an entire directory for
		/// the layout.
		/// </summary>
		public void SetIndividualDirectoryLayout()
		{
			FilesystemPersistenceSettings settings = Settings;

			settings.ProjectFilename = "<ProjectDir>/project.aiproj";
			settings.ExternalBlocksDirectory = "<ProjectDir>/Blocks";
			settings.ExternalSettingsDirectory = "<ProjectDir>/Settings";
			settings.ProjectSettingsDirectory = "<ProjectDir>/Settings";
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

			macros.Substitutions["ProjectDir"] = directory.FullName;
			macros.Substitutions["ProjectPath"] = Settings.ProjectFilename;

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
			SettingsKey = new HierarchicalPath(PluginId.ToString(), RootSettingsKey);
		}

		#endregion
	}
}
