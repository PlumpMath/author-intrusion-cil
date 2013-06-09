// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System;
using System.Xml;
using AuthorIntrusion.Common.Blocks;
using MfGames.HierarchicalPaths;

namespace AuthorIntrusion.Common.Persistence.Filesystem
{
	/// <summary>
	/// A XML-based reader for the content data part of a project, either as a
	/// separate file or as part of the project file itself.
	/// </summary>
	public class FilesystemPersistenceContentDataReader:
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
				projectReader, Settings.ContentDataFilename, out createdReader);

			// Loop through the resulting file until we get to the end of the
			// XML element we care about.
			ProjectBlockCollection blocks = Project.Blocks;
			bool reachedData = reader.NamespaceURI == XmlConstants.ProjectNamespace
				&& reader.LocalName == "content-data";
			int blockIndex = 0;

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
						case "content-data":
							return;

						case "block-data":
							// Increment the block index.
							blockIndex++;

							// If we exceeded the end of the blocks, then we won't
							// be able to process anymore and should just return.
							if (blockIndex >= blocks.Count)
							{
								return;
							}

							// Otherwise, continue on.
							break;
					}
				}

				// For the rest of this loop, we only deal with begin elements.
				if (reader.NodeType != XmlNodeType.Element)
				{
					continue;
				}

				// If we haven't reached the Structure, we just cycle through the XML.
				if (!reachedData)
				{
					// Flip the flag if we are starting to read the Structure.
					if (reader.NamespaceURI == XmlConstants.ProjectNamespace
						&& reader.LocalName == "content-data")
					{
						reachedData = true;
					}

					// Continue on since we're done with this if clause.
					continue;
				}

				// We process the remaining elements based on their local name.
				switch (reader.LocalName)
				{
					case "text-hash":
						// Grab the text hash. If one doesn't match, then we stop
						// processing entirely since the data file appears to be out
						// of sync with the rest of the file.
						string textHashValue = reader.ReadString();
						int textHash = Convert.ToInt32(textHashValue, 16);

						if (textHash != blocks[blockIndex].Text.GetHashCode())
						{
							return;
						}
						break;

					case "property":
						var path = new HierarchicalPath(reader["path"]);
						string value = reader.ReadString();
						blocks[blockIndex].Properties[path] = value;
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

		public FilesystemPersistenceContentDataReader(
			PersistenceReaderWriterBase<FilesystemPersistenceSettings> baseReader)
			: base(baseReader)
		{
		}

		#endregion
	}
}
