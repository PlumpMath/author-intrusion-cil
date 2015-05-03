// <copyright file="DocBookBufferFormat.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System;
using System.IO;
using System.Text;
using System.Xml;

using AuthorIntrusion.Buffers;

namespace AuthorIntrusion.IO
{
	/// <summary>
	/// Encapsulates the functionality for a buffer format that handles a DocBook 5
	/// file.
	/// </summary>
	public class DocBookBufferFormat : IFileBufferFormat
	{
		#region Constants

		/// <summary>
		/// The DocBook 5 XML namespace.
		/// </summary>
		public const string DocBookNamespace = "http://docbook.org/ns/docbook";

		#endregion

		#region Fields

		/// <summary>
		/// The settings for the DocBook format.
		/// </summary>
		private readonly DocBookBufferFormatSettings settings;

		#endregion

		#region Constructors and Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="DocBookBufferFormat"/> class.
		/// </summary>
		public DocBookBufferFormat()
		{
			settings = new DocBookBufferFormatSettings();
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets the current settings associated with the format.
		/// </summary>
		public IBufferFormatSettings Settings { get { return settings; } }

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Loads a profile of a specific format into memory. Profiles are either
		/// internally identified by the format and may be stored as part of
		/// a project's settings.
		/// </summary>
		/// <param name="profileName">
		/// The name of the profile to load.
		/// </param>
		public void LoadProfile(string profileName)
		{
		}

		/// <summary>
		/// Loads project data from the persistence layer and populates the project.
		/// </summary>
		/// <param name="context">
		/// </param>
		/// <exception cref="System.InvalidOperationException">
		/// Cannot read DocBook 5 files.
		/// </exception>
		public void LoadProject(BufferLoadContext context)
		{
			throw new InvalidOperationException("Cannot read DocBook 5 files.");
		}

		/// <summary>
		/// Writes out the project to the given persistence using the
		/// format instance.
		/// </summary>
		/// <param name="context">The context of the storing process.</param>
		public void StoreProject(BufferStoreContext context)
		{
			// Write out the project's stream first. This will recursively call
			// all of the other write operations.
			using (Stream stream = context.Persistence.GetProjectWriteStream())
			{
				// Create the XML writer for the file.
				var settings = new XmlWriterSettings
				{
					Indent = true,
					IndentChars = "\t",
					NamespaceHandling = NamespaceHandling.OmitDuplicates,
					CloseOutput = true,
					ConformanceLevel = ConformanceLevel.Document,
					Encoding = Encoding.UTF8
				};

				using (XmlWriter writer = XmlWriter.Create(
					stream,
					settings))
				{
					// Write out the document start.
					writer.WriteStartDocument();
					writer.WriteStartElement(
						this.settings.RootElement,
						DocBookNamespace);
					writer.WriteAttributeString(
						"version",
						"5.0");

					// Write out the info tag.
					writer.WriteStartElement("info");
					writer.WriteElementString(
						"title",
						context.Project.Titles.Title);
					writer.WriteEndElement();

					// Loop through and add all the lines.
					foreach (Block block in context.Project.Blocks)
					{
						writer.WriteElementString(
							"para",
							block.Text);
					}

					// Write out the end of the document.
					writer.WriteEndElement();
					writer.WriteEndDocument();
					writer.Close();
				}

				// Close the stream.
				stream.Close();
			}
		}

		#endregion
	}
}
