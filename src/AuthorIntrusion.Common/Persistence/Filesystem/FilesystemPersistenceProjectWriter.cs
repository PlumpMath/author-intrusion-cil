// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System.IO;
using System.Xml;
using System.Xml.Serialization;
using AuthorIntrusion.Common.Projects;

namespace AuthorIntrusion.Common.Persistence.Filesystem
{
	/// <summary>
	/// Handles the persistence (reading and writing) of the primary project
	/// files.
	/// </summary>
	public class FilesystemPersistenceProjectWriter:
		PersistenceReaderWriterBase<FilesystemPersistenceSettings>
	{
		#region Methods

		/// <summary>
		/// Writes the project to the specified file.
		/// </summary>
		/// <param name="projectFile">The project file.</param>
		public void Write(FileInfo projectFile)
		{
			// Open up an XML stream for the project.
			using (XmlWriter writer = GetXmlWriter(projectFile))
			{
				// Start the document.
				writer.WriteStartElement("project", ProjectNamespace);

				// Write out the version string.
				writer.WriteElementString("version", ProjectNamespace, "1");

				// Write out the settings we'll be using with this project.
				FilesystemPersistenceSettings settings = Settings;
				var settingsSerializer = new XmlSerializer(settings.GetType());
				settingsSerializer.Serialize(writer, settings);

				// Write out the various components.
				var settingsWriter = new FilesystemPersistenceSettingsWriter(this);
				var structureWriter = new FilesystemPersistenceStructureWriter(this);
				var contentWriter = new FilesystemPersistenceContentWriter(this);
				var contentDataWriter = new FilesystemPersistenceContentDataWriter(this);

				settingsWriter.Write(writer);
				structureWriter.Write(writer);
				contentWriter.Write(writer);
				contentDataWriter.Write(writer);

				// Finish up the project tag.
				writer.WriteEndElement();
			}
		}

		#endregion

		#region Constructors

		public FilesystemPersistenceProjectWriter(
			Project project,
			FilesystemPersistenceSettings settings,
			ProjectMacros macros)
			: base(project, settings, macros)
		{
		}

		#endregion
	}
}
