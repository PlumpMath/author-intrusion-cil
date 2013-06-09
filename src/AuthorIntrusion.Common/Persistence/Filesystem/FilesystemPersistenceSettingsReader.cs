// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System.Xml;
using MfGames.Settings;

namespace AuthorIntrusion.Common.Persistence.Filesystem
{
	public class FilesystemPersistenceSettingsReader:
		PersistenceReaderWriterBase<FilesystemPersistenceSettings>
	{
		#region Methods

		/// <summary>
		/// Reads the structure file, either from the project reader or the settings
		/// file depending on the persistence settings.
		/// </summary>
		/// <param name="projectReader">The project reader.</param>
		public void Read(XmlReader projectReader)
		{
			// Figure out which reader we'll be using.
			bool createdReader;
			XmlReader reader = GetXmlReader(
				projectReader, Settings.SettingsFilename, out createdReader);

			// Loop through the resulting file until we get to the end of the
			// XML element we care about.
			bool reachedSettings = reader.NamespaceURI == XmlConstants.ProjectNamespace
				&& reader.LocalName == "settings";

			while (reader.Read())
			{
				// If we get the full settings, then load it in.
				if (reader.NamespaceURI == SettingsManager.SettingsNamespace
					&& reader.LocalName == "settings")
				{
					Project.Settings.Load(reader);
					continue;
				}

				// Ignore anything outside of our namespace.
				if (reader.NamespaceURI != XmlConstants.ProjectNamespace)
				{
					continue;
				}

				// Check to see if we're done reading.
				if (reader.NodeType == XmlNodeType.EndElement
					&& reader.LocalName == "settings")
				{
					// We're done reading the settings.
					break;
				}

				// For the rest of this loop, we only deal with begin elements.
				if (reader.NodeType != XmlNodeType.Element)
				{
					continue;
				}

				// If we haven't reached the settings, we just cycle through the XML.
				if (!reachedSettings)
				{
					// Flip the flag if we are starting to read the settings.
					if (reader.NamespaceURI == XmlConstants.ProjectNamespace
						&& reader.LocalName == "settings")
					{
						reachedSettings = true;
					}

					// Continue on since we're done with this if clause.
					continue;
				}

				// We process the remaining elements based on their local name.
				if (reader.LocalName == "plugin")
				{
					string value = reader.ReadString();
					Project.Plugins.Add(value);
				}
			}

			// If we created the reader, close it.
			if (createdReader)
			{
				reader.Dispose();
			}
		}

		#endregion

		#region Constructors

		public FilesystemPersistenceSettingsReader(
			PersistenceReaderWriterBase<FilesystemPersistenceSettings> baseReader)
			: base(baseReader)
		{
		}

		#endregion
	}
}
