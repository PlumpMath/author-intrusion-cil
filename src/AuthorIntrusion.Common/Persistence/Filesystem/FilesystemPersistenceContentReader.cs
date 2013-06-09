// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System.Xml;
using AuthorIntrusion.Common.Blocks;

namespace AuthorIntrusion.Common.Persistence.Filesystem
{
	/// <summary>
	/// A XML-based reader for the content part of a project, either as a separate
	/// file or as part of the project file itself.
	/// </summary>
	public class FilesystemPersistenceContentReader:
		PersistenceReaderWriterBase<FilesystemPersistenceSettings>
	{
		#region Methods

		/// <summary>
		/// Reads the content file, either from the project reader or the Structure
		/// file depending on the persistence settings.
		/// </summary>
		/// <param name="projectReader">The project reader.</param>
		public void Read(XmlReader projectReader)
		{
			// Figure out which reader we'll be using.
			bool createdReader;
			XmlReader reader = GetXmlReader(
				projectReader, Settings.ContentFilename, out createdReader);

			// Loop through the resulting file until we get to the end of the
			// XML element we care about.
			ProjectBlockCollection blocks = Project.Blocks;
			bool reachedContents = reader.NamespaceURI == XmlConstants.ProjectNamespace
				&& reader.LocalName == "content";
			string text = null;
			BlockType blockType = null;
			bool firstBlock = true;

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
						case "content":
							return;

						case "block":
							// Create the block and insert it into the list.
							var block = new Block(blocks, blockType, text);

							if (firstBlock)
							{
								// Because we have a rule that the collection may
								// not have less than one block, the first block is
								// a replacement instead of adding to the list.
								blocks[0] = block;
								firstBlock = false;
							}
							else
							{
								blocks.Add(block);
							}

							break;
					}
				}

				// For the rest of this loop, we only deal with begin elements.
				if (reader.NodeType != XmlNodeType.Element)
				{
					continue;
				}

				// If we haven't reached the Structure, we just cycle through the XML.
				if (!reachedContents)
				{
					// Flip the flag if we are starting to read the Structure.
					if (reader.NamespaceURI == XmlConstants.ProjectNamespace
						&& reader.LocalName == "content")
					{
						reachedContents = true;
					}

					// Continue on since we're done with this if clause.
					continue;
				}

				// We process the remaining elements based on their local name.
				switch (reader.LocalName)
				{
					case "type":
						string blockTypeName = reader.ReadString();
						blockType = Project.BlockTypes[blockTypeName];
						break;

					case "text":
						text = reader.ReadString();
						break;
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

		public FilesystemPersistenceContentReader(
			PersistenceReaderWriterBase<FilesystemPersistenceSettings> baseReader)
			: base(baseReader)
		{
		}

		#endregion
	}
}
