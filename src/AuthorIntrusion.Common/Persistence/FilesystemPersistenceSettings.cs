// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace AuthorIntrusion.Common.Persistence
{
	/// <summary>
	/// Defines the serializable settings used to control the filesystem persistence
	/// project plugin.
	/// 
	/// All of the string variables in here can have project macros in them, which are
	/// expanded at the point of saving.
	/// </summary>
	[XmlRoot("filesystem-persistence-settings",
		Namespace = XmlConstants.ProjectNamespace)]
	public class FilesystemPersistenceSettings: IXmlSerializable
	{
		#region Properties

		public string ContentDataFilename { get; set; }
		public string ContentFilename { get; set; }
		public string DataDirectory { get; set; }
		public string ExternalSettingsDirectory { get; set; }
		public string ExternalSettingsFilename { get; set; }
		public string InternalContentDataFilename { get; set; }
		public string InternalContentDirectory { get; set; }
		public string InternalContentFilename { get; set; }

		public bool NeedsProjectDirectory
		{
			get { return string.IsNullOrWhiteSpace(ProjectDirectory); }
		}

		public bool NeedsProjectFilename
		{
			get { return string.IsNullOrWhiteSpace(ProjectFilename); }
		}

		public string ProjectDirectory { get; set; }
		public string ProjectFilename { get; set; }

		/// <summary>
		/// Gets or sets the filename for the settings file. This will contains the
		/// settings for plugins and configuration settings will be stored here.
		/// 
		/// External settings are identified by the ExternalSettingsFilename property.
		/// </summary>
		/// <remarks>
		/// This can have macro substitutions (e.g., "{ProjectDir}") in the name
		/// which will be expanded during use.
		/// </remarks>
		public string SettingsFilename { get; set; }

		public string StructureFilename { get; set; }

		#endregion

		#region Methods

		public XmlSchema GetSchema()
		{
			return null;
		}

		public void ReadXml(XmlReader reader)
		{
			// Read until we get to the end element.
			while (reader.Read())
			{
				Console.WriteLine("XML: " + reader.LocalName);

				if (reader.NodeType == XmlNodeType.EndElement
					&& reader.LocalName == "sets"
					&& reader.NamespaceURI == XmlConstants.ProjectNamespace)
				{
					return;
				}
			}
		}

		/// <summary>
		/// Configures a standard file layout that uses an entire directory for
		/// the layout. This layout is set up to optimize the handling of the
		/// project's content, settings, and data with a source control system, such as
		/// Git.
		/// </summary>
		public void SetIndividualDirectoryLayout()
		{
			// Setting project directory to null and setting the filename tells
			// any calling method that we need an output directory instead of a filename
			// dialog box.
			ProjectDirectory = null;
			ProjectFilename = "{ProjectDirectory}/Project.aiproj";

			DataDirectory = "{ProjectDirectory}/Data";

			SettingsFilename = "{ProjectDirectory}/Settings.xml";
			StructureFilename = "{ProjectDirectory}/Structure.xml";

			ContentFilename = "{ProjectDirectory}/Content.xml";
			ContentDataFilename = "{DataDirectory}/Content Data.xml";
			InternalContentDirectory = "{ProjectDirectory}/Content";
			InternalContentFilename = "{InternalContentDirectory}/{ContentName}.xml";
			InternalContentDataFilename =
				"{DataDirectory}/Content/{ContentName} Data.xml";

			ExternalSettingsDirectory = "{ProjectDirectory}/Settings";
			ExternalSettingsFilename =
				"{ExternalSettingsDirectory}/{SettingsProviderName}/{SettingsName}.xml";
		}

		/// <summary>
		/// Converts an object into its XML representation.
		/// </summary>
		/// <param name="writer">The <see cref="T:System.Xml.XmlWriter" /> stream to which the object is serialized.</param>
		public void WriteXml(XmlWriter writer)
		{
			// Always start with the version field, because that will control how
			// we read it back in.
			writer.WriteStartElement("sets", XmlConstants.ProjectNamespace);
			writer.WriteElementString("version", XmlConstants.ProjectNamespace, "1");

			// Write out the various properties.
			WriteKeyValue(writer, "ContentDataFilename", ContentDataFilename);
			WriteKeyValue(writer, "ContentFilename", ContentFilename);
			WriteKeyValue(writer, "DataDirectory", DataDirectory);
			WriteKeyValue(writer, "ExternalSettingsDirectory", ExternalSettingsDirectory);
			WriteKeyValue(writer, "ExternalSettingsFilename", ExternalSettingsFilename);
			WriteKeyValue(
				writer, "InternalContentDataFilename", InternalContentDataFilename);
			WriteKeyValue(writer, "InternalContentDirectory", InternalContentDirectory);
			WriteKeyValue(writer, "InternalContentFilename", InternalContentFilename);
			WriteKeyValue(writer, "ProjectDirectory", ProjectDirectory);
			WriteKeyValue(writer, "ProjectFilename", ProjectFilename);
			WriteKeyValue(writer, "SettingsFilename", SettingsFilename);
			WriteKeyValue(writer, "StructureFilename", StructureFilename);

			// Finish up the element.
			writer.WriteEndElement();
		}

		private void WriteKeyValue(
			XmlWriter writer,
			string key,
			string value)
		{
			if (!string.IsNullOrWhiteSpace(value))
			{
				writer.WriteStartElement("set", XmlConstants.ProjectNamespace);
				writer.WriteAttributeString("key", key);
				writer.WriteAttributeString("value", value);
				writer.WriteEndElement();
			}
		}

		#endregion
	}
}
