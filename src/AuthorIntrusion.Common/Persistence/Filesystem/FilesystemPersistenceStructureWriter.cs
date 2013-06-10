// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System.Globalization;
using System.Xml;
using AuthorIntrusion.Common.Blocks;
using C5;

namespace AuthorIntrusion.Common.Persistence.Filesystem
{
	public class FilesystemPersistenceStructureWriter:
		PersistenceReaderWriterBase<FilesystemPersistenceSettings>
	{
		#region Methods

		/// <summary>
		/// Writes the structure file, to either the project Writer or the Structure
		/// file depending on the persistence settings.
		/// </summary>
		/// <param name="projectWriter">The project Writer.</param>
		public void Write(XmlWriter projectWriter)
		{
			// Figure out which Writer we'll be using.
			bool createdWriter;
			XmlWriter writer = GetXmlWriter(
				projectWriter, Macros, Settings.StructureFilename, out createdWriter);

			// Create an order list of block types so we have a reliable order.
			var blockTypeNames = new ArrayList<string>();
			blockTypeNames.AddAll(Project.BlockTypes.BlockTypes.Keys);
			blockTypeNames.Sort();

			// Start by creating the initial element.
			writer.WriteStartElement("structure", ProjectNamespace);
			writer.WriteElementString("version", "1");

			// Write out the blocks types first.
			writer.WriteStartElement("block-types", ProjectNamespace);

			foreach (string blockTypeName in blockTypeNames)
			{
				// We don't write out system types since they are controlled via code.
				BlockType blockType = Project.BlockTypes[blockTypeName];

				if (blockType.IsSystem)
				{
					continue;
				}

				// Write out this item.
				writer.WriteStartElement("block-type", ProjectNamespace);

				// Write out the relevant fields.
				writer.WriteElementString("name", ProjectNamespace, blockType.Name);
				writer.WriteElementString(
					"is-structural", ProjectNamespace, blockType.IsStructural.ToString());

				// Finish up the item element.
				writer.WriteEndElement();
			}

			writer.WriteEndElement();

			// Write out the structural elements, which is an ordered tree already.
			BlockStructure rootBlockStructure =
				Project.BlockStructures.RootBlockStructure;

			WriteBlockStructure(writer, rootBlockStructure, "root-block-structure");

			// Finish up the tag.
			writer.WriteEndElement();

			// If we created the Writer, close it.
			if (createdWriter)
			{
				writer.Dispose();
			}
		}

		private void WriteBlockStructure(
			XmlWriter writer,
			BlockStructure blockStructure,
			string elementName)
		{
			// Start by writing out the element.
			writer.WriteStartElement(elementName, ProjectNamespace);

			// Write out the elements of this structure.
			writer.WriteElementString(
				"block-type", ProjectNamespace, blockStructure.BlockType.Name);
			writer.WriteStartElement("occurances", ProjectNamespace);
			writer.WriteAttributeString(
				"minimum",
				blockStructure.MinimumOccurances.ToString(CultureInfo.InvariantCulture));
			writer.WriteAttributeString(
				"maximum",
				blockStructure.MaximumOccurances.ToString(CultureInfo.InvariantCulture));
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

		#endregion

		#region Constructors

		public FilesystemPersistenceStructureWriter(
			PersistenceReaderWriterBase<FilesystemPersistenceSettings> baseWriter)
			: base(baseWriter)
		{
		}

		#endregion
	}
}
