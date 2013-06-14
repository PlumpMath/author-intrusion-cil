// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System;
using System.Xml;
using AuthorIntrusion.Common.Blocks;
using AuthorIntrusion.Common.Plugins;
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
			int startTextIndex = 0;
			int stopTextIndex = 0;
			ITextControllerProjectPlugin plugin = null;
			object pluginData = null;

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

						case "text-span":
							// Construct a text span from the gathered properties.
							var textSpan = new TextSpan(
								startTextIndex, stopTextIndex, plugin, pluginData);
							blocks[blockIndex].TextSpans.Add(textSpan);

							// Clear out the data to catch any additional errors.
							plugin = null;
							pluginData = null;
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
					case "block-key":
						// Grab the information to identify the block. We can't use
						// block key directly so we have an index into the original
						// block data and a hash of the text.
						blockIndex = Convert.ToInt32(reader["block-index"]);
						int textHash = Convert.ToInt32(reader["text-hash"], 16);

						// Figure out if we are asking for an index that doesn't exist
						// or if the hash doesn't work. If they don't, then stop
						// processing entirely.
						if (blockIndex >= blocks.Count
							|| blocks[blockIndex].Text.GetHashCode() != textHash)
						{
							return;
						}

						break;

					case "property":
						var path = new HierarchicalPath(reader["path"]);
						string value = reader.ReadString();
						blocks[blockIndex].Properties[path] = value;
						break;

					case "index":
						startTextIndex = Convert.ToInt32(reader["start"]);
						stopTextIndex = Convert.ToInt32(reader["stop"]);
						break;

					case "plugin":
						string projectKey = reader.ReadString();
						plugin = (ITextControllerProjectPlugin) Project.Plugins[projectKey];
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

		public FilesystemPersistenceContentDataReader(
			PersistenceReaderWriterBase<FilesystemPersistenceSettings> baseReader)
			: base(baseReader)
		{
		}

		#endregion
	}
}
