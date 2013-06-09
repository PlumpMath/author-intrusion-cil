// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System.IO;
using System.Xml;
using AuthorIntrusion.Common.Projects;

namespace AuthorIntrusion.Common.Persistence.Filesystem
{
	/// <summary>
	/// Handles the persistence (reading and writing) of the primary project
	/// files.
	/// </summary>
	public class FilesystemPersistenceProjectReader:
		PersistenceReaderWriterBase<FilesystemPersistenceSettings>
	{
		#region Methods

		/// <summary>
		/// Reads the specified file and loads a project from it.
		/// </summary>
		/// <param name="projectFile">The project file.</param>
		public void Read(FileInfo projectFile)
		{
			// Create a new, empty project.
			var project = new Project();

			// Open up an XML stream for the project.
			using (XmlReader reader = GetXmlReader(projectFile))
			{
				// We don't have to do anything initially with the project file.
				// However, if the settings indicate that there is a file inside
				// this one, we'll use this reader.
				//
				// The reading must be performed in the same order as writing.

				// Read in the settings.
				var settingsReader = new FilesystemPersistenceSettingsReader(this);

				settingsReader.Read(reader);

				// Read in the structure.
				var structureReader = new FilesystemPersistenceStructureReader(this);

				structureReader.Read(reader);

				//SaveContent(writer,macros);
				//SaveContentData(writer,macros);
			}
		}

		#endregion

		#region Constructors

		public FilesystemPersistenceProjectReader(
			Project project,
			FilesystemPersistenceSettings settings,
			ProjectMacros macros)
			: base(project, settings, macros)
		{
		}

		#endregion
	}
}
