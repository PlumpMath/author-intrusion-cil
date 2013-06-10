// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System.Xml;
using AuthorIntrusion.Common.Plugins;
using C5;

namespace AuthorIntrusion.Common.Persistence.Filesystem
{
	public class FilesystemPersistenceSettingsWriter:
		PersistenceReaderWriterBase<FilesystemPersistenceSettings>
	{
		#region Methods

		/// <summary>
		/// Writes the structure file, to either the project Writer or the settings
		/// file depending on the persistence settings.
		/// </summary>
		/// <param name="projectWriter">The project Writer.</param>
		public void Write(XmlWriter projectWriter)
		{
			// Figure out which Writer we'll be using.
			bool createdWriter;
			XmlWriter writer = GetXmlWriter(
				projectWriter, Macros, Settings.SettingsFilename, out createdWriter);

			// Start by creating the initial element.
			writer.WriteStartElement("settings", ProjectNamespace);
			writer.WriteElementString("version", "1");

			// Write out the project settings.
			Project project = Project;

			project.Settings.Save(writer);

			// Write out the plugin controllers.
			IList<ProjectPluginController> projectControllers =
				project.Plugins.Controllers;

			if (!projectControllers.IsEmpty)
			{
				// We always work with sorted plugins to simplify change control.
				var pluginNames = new ArrayList<string>();

				foreach (ProjectPluginController controller in projectControllers)
				{
					pluginNames.Add(controller.Name);
				}

				pluginNames.Sort();

				// Write out a list of plugins.
				writer.WriteStartElement("plugins", ProjectNamespace);

				foreach (string pluginName in pluginNames)
				{
					writer.WriteElementString("plugin", ProjectNamespace, pluginName);
				}

				writer.WriteEndElement();
			}

			// Finish up the blocks element.
			writer.WriteEndElement();

			// If we created the Writer, close it.
			if (createdWriter)
			{
				writer.Dispose();
			}
		}

		#endregion

		#region Constructors

		public FilesystemPersistenceSettingsWriter(
			PersistenceReaderWriterBase<FilesystemPersistenceSettings> baseWriter)
			: base(baseWriter)
		{
		}

		#endregion
	}
}
