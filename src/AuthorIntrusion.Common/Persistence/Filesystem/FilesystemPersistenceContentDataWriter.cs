// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System.Globalization;
using System.Xml;
using AuthorIntrusion.Common.Blocks;
using C5;
using MfGames.HierarchicalPaths;

namespace AuthorIntrusion.Common.Persistence.Filesystem
{
	/// <summary>
	/// A XML-based Writer for the content data part of a project, either as a
	/// separate file or as part of the project file itself.
	/// </summary>
	public class FilesystemPersistenceContentDataWriter:
		PersistenceReaderWriterBase<FilesystemPersistenceSettings>
	{
		#region Methods

		/// <summary>
		/// Writes the content file, to either the project Writer or the Structure
		/// file depending on the persistence settings.
		/// </summary>
		/// <param name="projectWriter">The project Writer.</param>
		public void Write(XmlWriter projectWriter)
		{
			// Figure out which Writer we'll be using.
			bool createdWriter;
			XmlWriter writer = GetXmlWriter(
				projectWriter, Macros, Settings.ContentDataFilename, out createdWriter);

			// Start by creating the initial element.
			writer.WriteStartElement("content-data", ProjectNamespace);
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
				writer.WriteStartElement("block-data", ProjectNamespace);

				// Write out the text of the block so we can identify it later. It
				// normally will be in order, but this is a second verification
				// that won't change.
				writer.WriteStartElement("block-key", ProjectNamespace);
				writer.WriteAttributeString(
					"block-index", blockIndex.ToString(CultureInfo.InvariantCulture));
				writer.WriteAttributeString(
					"text-hash", block.Text.GetHashCode().ToString("X8"));
				writer.WriteEndElement();

				// For this pass, we write out the data generates by the plugins
				// and internal state.
				WriteBlockProperties(writer, block);
				WriteTextSpans(writer, block);

				// Finish up the block.
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
			writer.WriteStartElement("properties", ProjectNamespace);

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
			writer.WriteStartElement("text-spans", ProjectNamespace);

			foreach (TextSpan textSpan in block.TextSpans)
			{
				// Write out the beginning of the text span element.
				writer.WriteStartElement("text-span", ProjectNamespace);

				// Write out the common elements.
				writer.WriteStartElement("index", ProjectNamespace);
				writer.WriteAttributeString(
					"start", textSpan.StartTextIndex.ToString(CultureInfo.InvariantCulture));
				writer.WriteAttributeString(
					"stop", textSpan.StopTextIndex.ToString(CultureInfo.InvariantCulture));
				writer.WriteEndElement();

				// If we have an associated controller, we need to write it out and
				// it's data. Since we don't know how to write it out, we pass the
				// writing to the controller.
				if (textSpan.Controller != null)
				{
					// Write out the name of the controller.
					writer.WriteElementString(
						"plugin", ProjectNamespace, textSpan.Controller.Key);

					// If we have data, then write out the plugin data tag and pass
					// the writing to the controller.
					if (textSpan.Data != null)
					{
						writer.WriteStartElement("plugin-data", ProjectNamespace);
						textSpan.Controller.WriteTextSpanData(writer, textSpan.Data);
						writer.WriteEndElement();
					}
				}

				// Finish up the tag.
				writer.WriteEndElement();
			}

			writer.WriteEndElement();
		}

		#endregion

		#region Constructors

		public FilesystemPersistenceContentDataWriter(
			PersistenceReaderWriterBase<FilesystemPersistenceSettings> baseWriter)
			: base(baseWriter)
		{
		}

		#endregion
	}
}
