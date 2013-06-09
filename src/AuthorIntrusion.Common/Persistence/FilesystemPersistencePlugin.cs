// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System.IO;
using System.Xml;
using System.Xml.Serialization;
using AuthorIntrusion.Common.Persistence.Filesystem;
using AuthorIntrusion.Common.Plugins;
using AuthorIntrusion.Common.Projects;

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

		public bool CanRead(FileInfo projectFile)
		{
			bool results = projectFile.Extension == ".aiproj";
			return results;
		}

		public IProjectPlugin GetProjectPlugin(Project project)
		{
			var projectPlugin = new FilesystemPersistenceProjectPlugin(project);
			return projectPlugin;
		}

		public Project ReadProject(FileInfo projectFile)
		{
			// Open up an XML reader to pull out the critical components we
			// need to finish loading the file.
			FilesystemPersistenceSettings settings = null;

			using (FileStream stream = projectFile.Open(FileMode.Open, FileAccess.Read))
			{
				using (XmlReader reader = XmlReader.Create(stream))
				{
					// Read until we get the file-persistent-settings file.
					while (reader.Read())
					{
						// Ignore everything but the settings object we need to read.
						if (reader.NamespaceURI != XmlConstants.ProjectNamespace
							|| reader.NodeType != XmlNodeType.Element
							|| reader.LocalName != FilesystemPersistenceSettings.XmlElementName)
						{
							continue;
						}

						// Load the settings object into memory.
						var serializer = new XmlSerializer(typeof (FilesystemPersistenceSettings));
						settings = (FilesystemPersistenceSettings) serializer.Deserialize(reader);
					}
				}
			}

			// If we finish reading the file without getting the settings, blow up.
			if (settings == null)
			{
				throw new FileLoadException("Cannot load project: " + projectFile);
			}

			// Populate the macros we'll be using.
			var macros = new ProjectMacros();

			macros.Substitutions["ProjectDirectory"] = projectFile.Directory.FullName;
			macros.Substitutions["ProjectFile"] = projectFile.FullName;
			macros.Substitutions["DataDirectory"] = settings.DataDirectory;
			macros.Substitutions["InternalContentDirectory"] =
				settings.InternalContentDirectory;
			macros.Substitutions["ExternalSettingsDirectory"] =
				settings.ExternalSettingsDirectory;

			// Load the project starting with the project.
			var project = new Project();
			var projectReaderWriter = new FilesystemPersistenceProjectReader(
				project, settings, macros);
			projectReaderWriter.Read(projectFile);

			return project;
		}

		#endregion
	}
}
