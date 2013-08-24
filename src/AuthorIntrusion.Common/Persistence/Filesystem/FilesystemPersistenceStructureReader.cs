// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System;
using System.Xml;
using AuthorIntrusion.Common.Blocks;

namespace AuthorIntrusion.Common.Persistence.Filesystem
{
	public class FilesystemPersistenceStructureReader:
		PersistenceReaderWriterBase<FilesystemPersistenceSettings>
	{
		#region Methods

		/// <summary>
		/// Reads the structure file, either from the project reader or the Structure
		/// file depending on the persistence settings.
		/// </summary>
		/// <param name="projectReader">The project reader.</param>
		public void Read(XmlReader projectReader)
		{
			// Figure out which reader we'll be using.
			bool createdReader;
			XmlReader reader = GetXmlReader(
				projectReader, Settings.StructureFilename, out createdReader);

			// Loop through the resulting file until we get to the end of the
			// XML element we care about.
			bool reachedStructure = reader.NamespaceURI == XmlConstants.ProjectNamespace
				&& reader.LocalName == "structure";
			BlockType lastBlockType = null;

			while (reader.Read())
			{
				// Ignore anything outside of our namespace.
				if (reader.NamespaceURI != XmlConstants.ProjectNamespace)
				{
					continue;
				}

				// Check to see if we're done reading.
				if (reader.NodeType == XmlNodeType.EndElement)
				{
					switch (reader.LocalName)
					{
						case "structure":
							return;

						case "block-type":
							lastBlockType = null;
							break;
					}
				}

				// For the rest of this loop, we only deal with begin elements.
				if (reader.NodeType != XmlNodeType.Element)
				{
					continue;
				}

				// If we haven't reached the Structure, we just cycle through the XML.
				if (!reachedStructure)
				{
					// Flip the flag if we are starting to read the Structure.
					if (reader.NamespaceURI == XmlConstants.ProjectNamespace
						&& reader.LocalName == "structure")
					{
						reachedStructure = true;
					}

					// Continue on since we're done with this if clause.
					continue;
				}

				// We process the remaining elements based on their local name.
				switch (reader.LocalName)
				{
					case "block-type":
						lastBlockType = new BlockType(Project.BlockTypes);
						break;

					case "name":
						string nameValue = reader.ReadString();

						if (lastBlockType != null)
						{
							lastBlockType.Name = nameValue;
							Project.BlockTypes.BlockTypes[nameValue] = lastBlockType;
						}

						break;

					case "is-structural":
						bool structuralValue = Convert.ToBoolean(reader.ReadString());
						lastBlockType.IsStructural = structuralValue;
						break;
				}
			}

			// If we created the reader, close it.
			if (createdReader)
			{
				reader.Close();
				reader.Dispose();
			}
		}

		#endregion

		#region Constructors

		public FilesystemPersistenceStructureReader(
			PersistenceReaderWriterBase<FilesystemPersistenceSettings> baseReader)
			: base(baseReader)
		{
		}

		#endregion
	}
}
