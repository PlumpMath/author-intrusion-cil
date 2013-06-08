// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using AuthorIntrusion.Common.Blocks;
using AuthorIntrusion.Common.Plugins;
using AuthorIntrusion.Common.Projects;
using C5;
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
			string projectFilename = macros.ExpandMacros("{ProjectFilename}");

			if (string.IsNullOrWhiteSpace(projectFilename))
			{
				throw new InvalidOperationException(
					"Project filename is not defined in Settings property.");
			}

			// We need a write lock on the blocks to avoid changes. This also prevents
			// any background tasks from modifying the blocks during the save process.
			ProjectBlockCollection blocks = Project.Blocks;

			using (blocks.AcquireWriteLock())
			{
				// Create the project file.
				using (XmlWriter writer = SaveProjectFile(new FileInfo(projectFilename)))
				{
					// Write out the various components.
					SaveStructure(writer, macros);
					SaveSettings(writer, macros);
					SaveContent(writer, macros);
					SaveContentData(writer, macros);

					// Finish up by closing the project file.
					writer.Close();
				}
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
		/// Gets the macro or project XML writer. If the given variable expands to
		/// a value, an XML writer is created and returned. Otherwise, the given
		/// project writer is used instead.
		/// </summary>
		/// <param name="projectWriter">The project writer.</param>
		/// <param name="macros">The macros.</param>
		/// <param name="variable">The variable.</param>
		/// <param name="createdWriter">if set to <c>true</c> [created writer].</param>
		/// <returns></returns>
		private static XmlWriter GetMacroOrProjectXmlWriter(
			XmlWriter projectWriter,
			ProjectMacros macros,
			string variable,
			out bool createdWriter)
		{
			// Expand the variable to get the filename.
			string filename = macros.ExpandMacros(variable);

			// If the value is null, then we use the project writer.
			if (string.IsNullOrWhiteSpace(filename))
			{
				createdWriter = false;
				return projectWriter;
			}

			// Make sure the parent directory exists for this writer.
			new FileInfo(filename).EnsureParentExists();

			// Create an XML writer for this file and return it.
			XmlWriterSettings xmlSettings = CreateXmlSettings();
			XmlWriter writer = XmlWriter.Create(filename, xmlSettings);

			createdWriter = true;

			return writer;
		}

		/// <summary>
		/// Saves the project blocks starting with the main project while allowing
		/// for external files to be saved in the appropriate location.
		/// </summary>
		/// <param name="projectWriter">The project writer.</param>
		/// <param name="macros">The macros.</param>
		private void SaveContent(
			XmlWriter projectWriter,
			ProjectMacros macros)
		{
			// Figure out which writer we'll be using.
			bool createdWriter;
			XmlWriter writer = GetMacroOrProjectXmlWriter(
				projectWriter, macros, Settings.ContentFilename, out createdWriter);

			// Start by creating the initial element.
			const string ns = XmlConstants.ProjectNamespace;

			writer.WriteStartElement("contents", ns);
			writer.WriteElementString("version", "1");

			// Go through the blocks in the project.
			ProjectBlockCollection blocks = Project.Blocks;

			for (int blockIndex = 0;
				blockIndex < blocks.Count;
				blockIndex++)
			{
				// Write out this block.
				Block block = blocks[blockIndex];
				writer.WriteStartElement("block", ns);
				writer.WriteAttributeString(
					"index", blockIndex.ToString(CultureInfo.InvariantCulture));

				// For this pass, we only include block elements that are
				// user-entered. We'll do a second pass to include the processed
				// data including TextSpans and parsing status later.
				writer.WriteElementString("type", ns, block.BlockType.Name);
				writer.WriteElementString("text", ns, block.Text);

				// Finish up the block.
				writer.WriteEndElement();
			}

			// Finish up the blocks element.
			writer.WriteEndElement();

			// If we aren't using the project file, then close the writer.
			if (createdWriter)
			{
				writer.Close();
				writer.Dispose();
			}
		}

		/// <summary>
		/// Saves the parsed/processed data of blocks.
		/// </summary>
		/// <param name="projectWriter">The XML writer for the project file.</param>
		/// <param name="macros">The macros.</param>
		private void SaveContentData(
			XmlWriter projectWriter,
			ProjectMacros macros)
		{
			// Figure out which writer we'll be using.
			bool createdWriter;
			XmlWriter writer = GetMacroOrProjectXmlWriter(
				projectWriter, macros, Settings.ContentDataFilename, out createdWriter);

			// Start by creating the initial element.
			writer.WriteStartElement("content-data", ns);
			writer.WriteElementString("version", "1");

			// Go through the blocks in the list.
			ProjectBlockCollection blocks = Project.Blocks;

			for (int blockIndex = 0;
				blockIndex < blocks.Count;
				blockIndex++)
			{
				// If we don't have any data, skip it.
				Block block = blocks[blockIndex];

				if (block.Properties.IsEmpty
					&& block.TextSpans.IsEmpty)
				{
					continue;
				}

				// Write out this block.
				writer.WriteStartElement("block-data", ns);
				writer.WriteAttributeString(
					"index", blockIndex.ToString(CultureInfo.InvariantCulture));

				// For this pass, we write out the data generates by the plugins
				// and internal state.
				WriteBlockProperties(writer, block);
				WriteTextSpans(writer, block);

				// Finish up the block.
				writer.WriteEndElement();
			}

			// Finish up the blocks element.
			writer.WriteEndElement();

			// If we aren't using the project file, then close the writer.
			if (createdWriter)
			{
				writer.Close();
				writer.Dispose();
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
			writer.WriteStartElement("project", XmlConstants.ProjectNamespace);
			writer.WriteElementString("version", "1");

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
		/// Saves the settings of the project.
		/// </summary>
		/// <param name="projectWriter">The project writer.</param>
		/// <param name="macros">The macros.</param>
		private void SaveSettings(
			XmlWriter projectWriter,
			ProjectMacros macros)
		{
			// Figure out which writer we'll be using.
			bool createdWriter;
			XmlWriter writer = GetMacroOrProjectXmlWriter(
				projectWriter, macros, Settings.SettingsFilename, out createdWriter);

			// Start by creating the initial element.
			writer.WriteStartElement("settings", ns);
			writer.WriteElementString("version", "1");

			// Write out the project settings.
			// TODO This breaks everything. Project.Settings.Save(writer);

			// Finish up the blocks element.
			writer.WriteEndElement();

			// If we aren't using the project file, then close the writer.
			if (createdWriter)
			{
				writer.Close();
				writer.Dispose();
			}
		}

		/// <summary>
		/// Saves the structure of the document including both the block types
		/// and block structure so they can be reconstructed later.
		/// </summary>
		/// <param name="projectWriter">The XML writer for the project file.</param>
		/// <param name="macros">The macros.</param>
		private void SaveStructure(
			XmlWriter projectWriter,
			ProjectMacros macros)
		{
			// Create an order list of block types so we have a reliable order.
			var blockTypeNames = new ArrayList<string>();
			blockTypeNames.AddAll(Project.BlockTypes.BlockTypes.Keys);
			blockTypeNames.Sort();

			// Figure out which writer we'll be using.
			bool createdWriter;
			XmlWriter writer = GetMacroOrProjectXmlWriter(
				projectWriter, macros, Settings.StructureFilename, out createdWriter);

			// Start by creating the initial element.
			const string ns = XmlConstants.ProjectNamespace;

			writer.WriteStartElement("structure", ns);
			writer.WriteElementString("version", "1");

			// Write out the blocks types first.
			writer.WriteStartElement("block-types", ns);

			foreach (string blockTypeName in blockTypeNames)
			{
				// We don't write out system types since they are controlled via code.
				BlockType blockType = Project.BlockTypes[blockTypeName];

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

			writer.WriteEndElement();

			// Write out the structural elements, which is an ordered tree already.
			BlockStructure rootBlockStructure =
				Project.BlockStructures.RootBlockStructure;

			WriteBlockStructure(writer, rootBlockStructure, "root-block-structure");

			// If we aren't using the project file, then close the writer.
			if (createdWriter)
			{
				writer.Close();
				writer.Dispose();
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

		/// <summary>
		/// Writes out the block properties of a block.
		/// </summary>
		/// <param name="writer">The writer.</param>
		/// <param name="block">The block.</param>
		private static void WriteBlockProperties(
			XmlWriter writer,
			Block block)
		{
			// If we don't have properties, then don't write out anything.
			if (block.Properties.Count <= 0)
			{
				return;
			}

			// Write out the start element for the properties list.
			writer.WriteStartElement("properties", ns);

			// Go through all the properties, in order, and write it out.
			var propertyPaths = new ArrayList<HierarchicalPath>();
			propertyPaths.AddAll(block.Properties.Keys);
			propertyPaths.Sort();

			foreach (HierarchicalPath propertyPath in propertyPaths)
			{
				writer.WriteStartElement("property");
				writer.WriteAttributeString("path", propertyPath.ToString());
				writer.WriteString(block.Properties[propertyPath]);
				writer.WriteEndElement();
			}

			// Finish up the properties element.
			writer.WriteEndElement();
		}

		private void WriteBlockStructure(
			XmlWriter writer,
			BlockStructure blockStructure,
			string elementName)
		{
			// Start by writing out the element.
			const string ns = XmlConstants.ProjectNamespace;

			writer.WriteStartElement(elementName, ns);

			// Write out the elements of this structure.
			writer.WriteElementString("block-type", ns, blockStructure.BlockType.Name);
			writer.WriteStartElement("occurances", ns);
			writer.WriteAttributeString(
				"minimum", blockStructure.MinimumOccurances.ToString());
			writer.WriteAttributeString(
				"maximum", blockStructure.MaximumOccurances.ToString());
			writer.WriteEndElement();

			// Write out the child elements, if we have one.
			if (blockStructure.ChildStructures.Count > 0)
			{
				writer.WriteStartElement("child-block-structures");

				foreach (BlockStructure child in blockStructure.ChildStructures)
				{
					WriteBlockStructure(writer, child, "child-block-structure");
				}

				writer.WriteEndElement();
			}

			// Finish up the element.
			writer.WriteEndElement();
		}

		/// <summary>
		/// Writes the text spans of a block.
		/// </summary>
		/// <param name="writer">The writer.</param>
		/// <param name="block">The block.</param>
		private static void WriteTextSpans(
			XmlWriter writer,
			Block block)
		{
			// If we don't have spans, then skip them.
			if (block.TextSpans.Count <= 0)
			{
				return;
			}

			// Write out the text spans.
			writer.WriteStartElement("text-spans", ns);

			foreach (TextSpan textSpan in block.TextSpans)
			{
			}

			writer.WriteEndElement();
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

		#region Fields

		private const string ns = XmlConstants.ProjectNamespace;

		#endregion
	}
}
