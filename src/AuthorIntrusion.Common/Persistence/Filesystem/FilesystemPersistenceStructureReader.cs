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
		/// file depending on the persistence Structure.
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

					case "root-block-structure":
						BlockStructure rootBlockStructure = ReadBlockStructure(reader);
						Project.BlockStructures.RootBlockStructure = rootBlockStructure;
						break;
				}
			}

			// If we created the reader, close it.
			if (createdReader)
			{
				reader.Dispose();
			}
		}

		/// <summary>
		/// Reads the block structure from the XML stream.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <returns></returns>
		private BlockStructure ReadBlockStructure(XmlReader reader)
		{
			// Keep track of the element name since we'll need to stop reading
			// when we get to the end element.
			string elementName = reader.LocalName;
			var blockStructure = new BlockStructure();

			// If we're blank, just skip it.
			if (reader.IsEmptyElement)
			{
				return blockStructure;
			}

			// Loop through the lines until we get to the end.
			while (reader.Read())
			{
				// Check for the end element.
				if (reader.NodeType == XmlNodeType.EndElement
					&& reader.LocalName == elementName)
				{
					break;
				}

				// Ignore anything but start elements at this point.
				if (reader.NodeType != XmlNodeType.Element)
				{
					continue;
				}

				// Figure out what to do from this element.
				switch (reader.LocalName)
				{
					case "block-type":
						string blockTypeName = reader.ReadString();
						blockStructure.BlockType = Project.BlockTypes[blockTypeName];
						break;

					case "occurances":
						int minimumValue = Convert.ToInt32(reader["minimum"]);
						int maximumValue = Convert.ToInt32(reader["maximum"]);

						blockStructure.MinimumOccurances = minimumValue;
						blockStructure.MaximumOccurances = maximumValue;

						break;

					case "child-block-structure":
						BlockStructure childStructure = ReadBlockStructure(reader);
						blockStructure.ChildStructures.Add(childStructure);
						break;
				}
			}

			// Return the resulting structure.
			return blockStructure;
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
