// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System.IO;
using System.Text;
using System.Xml;
using AuthorIntrusion.Common.Projects;
using MfGames.Extensions.System.IO;

namespace AuthorIntrusion.Common.Persistence
{
	public abstract class PersistenceReaderWriterBase<TSettings>
	{
		#region Properties

		public ProjectMacros Macros { get; set; }
		public Project Project { get; set; }
		public TSettings Settings { get; set; }

		#endregion

		#region Methods

		/// <summary>
		/// Creates the common XML settings for most writers.
		/// </summary>
		/// <returns></returns>
		protected static XmlWriterSettings CreateXmlSettings()
		{
			var settings = new XmlWriterSettings
			{
				Encoding = Encoding.UTF8,
				Indent = true,
				IndentChars = "\t",
			};

			return settings;
		}

		/// <summary>
		/// Gets the XML reader for a given file.
		/// </summary>
		/// <remarks>
		/// It is responsiblity of the calling method to close the given reader.
		/// </remarks>
		/// <param name="fileInfo">The file info.</param>
		/// <returns>An XML reader for the file.</returns>
		protected XmlReader GetXmlReader(FileInfo fileInfo)
		{
			FileStream stream = fileInfo.Open(
				FileMode.Open, FileAccess.Read, FileShare.Read);
			XmlReader reader = XmlReader.Create(stream);
			return reader;
		}

		/// <summary>
		/// Gets the XML reader for a file, either the given project reader or
		/// a constructed reader based on the filename if the filename expands
		/// into a non-blank value.
		/// </summary>
		/// <param name="projectReader">The project reader.</param>
		/// <param name="filename">The filename.</param>
		/// <param name="createdReader">if set to <c>true</c> then the reader was created.</param>
		/// <returns>The XML reader to use.</returns>
		protected XmlReader GetXmlReader(
			XmlReader projectReader,
			string filename,
			out bool createdReader)
		{
			// Try to resolve the filename. If this is null or empty, then we
			// use the project reader.
			string expandedFilename = Macros.ExpandMacros(filename);

			if (string.IsNullOrWhiteSpace(expandedFilename))
			{
				createdReader = false;
				return projectReader;
			}

			// We need to create a new reader.
			var file = new FileInfo(expandedFilename);
			XmlReader reader = GetXmlReader(file);

			createdReader = true;
			return reader;
		}

		/// <summary>
		/// Gets the macro or project XML writer. If the given variable expands to
		/// a value, an XML writer is created and returned. Otherwise, the given
		/// project writer is used instead.
		/// </summary>
		/// <param name="projectWriter">The project writer.</param>
		/// <param name="macros">The macros.</param>
		/// <param name="variable">The variable.</param>
		/// <param name="createdWriter">if set to <c>true</c> [created writer].</param>
		/// <returns></returns>
		protected static XmlWriter GetXmlWriter(
			XmlWriter projectWriter,
			ProjectMacros macros,
			string variable,
			out bool createdWriter)
		{
			// Expand the variable to get the filename.
			string filename = macros.ExpandMacros(variable);

			// If the value is null, then we use the project writer.
			if (string.IsNullOrWhiteSpace(filename))
			{
				createdWriter = false;
				return projectWriter;
			}

			// Create the writer and return it.
			var file = new FileInfo(filename);
			XmlWriter writer = GetXmlWriter(file);

			createdWriter = true;

			return writer;
		}

		/// <summary>
		/// Constructs an XML writer and returns it.
		/// </summary>
		/// <param name="file">The file.</param>
		/// <returns></returns>
		protected static XmlWriter GetXmlWriter(FileInfo file)
		{
			// Make sure the parent directory exists for this writer.
			file.EnsureParentExists();

			// Create an XML writer for this file and return it.
			XmlWriterSettings xmlSettings = CreateXmlSettings();
			XmlWriter writer = XmlWriter.Create(file.FullName, xmlSettings);

			// Start the writer's document tag.
			writer.WriteStartDocument(true);

			// Return the resulting writer.
			return writer;
		}

		#endregion

		#region Constructors

		protected PersistenceReaderWriterBase(
			PersistenceReaderWriterBase<TSettings> baseReader)
			: this(baseReader.Project, baseReader.Settings, baseReader.Macros)
		{
		}

		protected PersistenceReaderWriterBase(
			Project project,
			TSettings settings,
			ProjectMacros macros)
		{
			Project = project;
			Settings = settings;
			Macros = macros;
		}

		#endregion

		#region Fields

		protected const string ProjectNamespace = XmlConstants.ProjectNamespace;

		#endregion
	}
}
