// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System.Xml;
using AuthorIntrusion.Common.Blocks;

namespace AuthorIntrusion.Common.Persistence.Filesystem
{
	/// <summary>
	/// A XML-based Writer for the content part of a project, either as a separate
	/// file or as part of the project file itself.
	/// </summary>
	public class FilesystemPersistenceContentWriter:
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
				projectWriter, Macros, Settings.ContentFilename, out createdWriter);

			// Start by creating the initial element.
			writer.WriteStartElement("content", ProjectNamespace);
			writer.WriteElementString("version", "1");

			// Go through the blocks in the project.
			ProjectBlockCollection blocks = Project.Blocks;

			foreach (Block block in blocks)
			{
				// Write the beginning of the block.
				writer.WriteStartElement("block", ProjectNamespace);

				// For this pass, we only include block elements that are
				// user-entered. We'll do a second pass to include the processed
				// data including TextSpans and parsing status later.
				writer.WriteElementString("type", ProjectNamespace, block.BlockType.Name);
				writer.WriteElementString("text", ProjectNamespace, block.Text);
				writer.WriteElementString(
					"text-hash", ProjectNamespace, block.Text.GetHashCode().ToString("X8"));

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

		#endregion

		#region Constructors

		public FilesystemPersistenceContentWriter(
			PersistenceReaderWriterBase<FilesystemPersistenceSettings> baseWriter)
			: base(baseWriter)
		{
		}

		#endregion
	}
}
