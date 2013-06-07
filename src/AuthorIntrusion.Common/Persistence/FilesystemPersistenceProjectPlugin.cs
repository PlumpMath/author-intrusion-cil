// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using AuthorIntrusion.Common.Blocks;
using AuthorIntrusion.Common.Plugins;
using AuthorIntrusion.Common.Projects;
using MfGames.Extensions.System.IO;
using MfGames.HierarchicalPaths;

namespace AuthorIntrusion.Common.Persistence
{
	/// <summary>
	/// The project plugin to describe an export/save format for a project.
	/// </summary>
	public class FilesystemPersistenceProjectPlugin: IProjectPlugin
	{
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
			string projectFilename = macros.ExpandMacros("{ProjectPath}");

			if (string.IsNullOrWhiteSpace(projectFilename))
			{
				throw new InvalidOperationException(
					"Project filename is not defined in Settings property.");
			}

			// Create the project file.
			using (XmlWriter writer = SaveProjectFile(new FileInfo(projectFilename)))
			{
				// Write out the various components.
				SaveProjectBlockTypes(writer, macros);
				SaveProjectBlocks(writer, macros);

				// Finish up by closing the project file.
				writer.Close();
			}
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
		/// Saves the project blocks types to the appropriate location so they can
		/// be reconstructed later.
		/// </summary>
		/// <param name="projectWriter">The XML writer for the project file.</param>
		/// <param name="macros">The macros.</param>
		private void SaveProjectBlockTypes(
			XmlWriter projectWriter,
			ProjectMacros macros)
		{
			// Figure out which writer we'll be using.
			XmlWriter writer = projectWriter;
			string filename = macros.ExpandMacros(Settings.ProjectBlockTypesFilename);

			if (!string.IsNullOrWhiteSpace(filename))
			{
				// Make sure the directory exists.
				new FileInfo(filename).EnsureParentExists();

				// Create an XML writer for this filename.
				XmlWriterSettings xmlSettings = CreateXmlSettings();
				writer = XmlWriter.Create(filename, xmlSettings);
			}

			// Start by creating the initial element.
			const string ns = XmlConstants.ProjectNamespace;

			writer.WriteStartElement("block-types", ns);

			// Go through the blocks in the list.
			foreach (BlockType blockType in Project.BlockTypes.BlockTypes.Values)
			{
				// We don't write out system types.
				if (blockType.IsSystem)
				{
					continue;
				}

				// Write out this item.
				writer.WriteStartElement("block-type", ns);

				// Write out the relevant fields.
				writer.WriteElementString("name", ns, blockType.Name);
				writer.WriteElementString(
					"is-structural", ns, blockType.IsStructural.ToString());

				// Finish up the item element.
				writer.WriteEndElement();
			}

			// Finish up the blocks element.
			writer.WriteEndElement();

			// If we aren't using the project file, then close the writer.
			if (projectWriter != writer)
			{
				writer.Close();
				writer.Dispose();
			}
		}

		/// <summary>
		/// Saves the project blocks starting with the main project while allowing
		/// for external files to be saved in the appropriate location.
		/// </summary>
		/// <param name="writer">The XML writer for the project file.</param>
		/// <param name="macros">The macros.</param>
		private void SaveProjectBlocks(
			XmlWriter writer,
			ProjectMacros macros)
		{
			// Start by creating the initial element.
			const string ns = XmlConstants.ProjectNamespace;

			writer.WriteStartElement("blocks", ns);

			// We need a write lock on the blocks to avoid changes. This also prevents
			// any background tasks from modifying the blocks during the save process.
			ProjectBlockCollection blocks = Project.Blocks;

			using (blocks.AcquireWriteLock())
			{
				// Go through the blocks in the list.
				foreach (Block block in blocks)
				{
					// Write out this block.
					writer.WriteStartElement("block", ns);

					// For this pass, we only include block elements that are
					// user-entered. We'll do a second pass to include the processed
					// data including TextSpans and parsing status later.
					writer.WriteElementString("type", ns, block.BlockType.Name);
					writer.WriteElementString("text", ns, block.Text);

					// Finish up the block.
					writer.WriteEndElement();
				}
			}

			// Finish up the blocks element.
			writer.WriteEndElement();
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
			writer.WriteStartElement("project", XmlConstants.ProjectNamespace);

			// The first time in the project file needs to be the settings we used to
			// serialize them. This allows the loading process to identify where the
			// other files will be located.
			FilesystemPersistenceSettings settings = Settings;
			var settingsSerializer = new XmlSerializer(settings.GetType());
			settingsSerializer.Serialize(writer, settings);

			// Return the resulting writer. We don't finish the outermost XML
			// tag since we'll handle it through the close method.
			return writer;
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
