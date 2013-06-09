// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System.IO;
using System.Xml;
using AuthorIntrusion.Common.Projects;

namespace AuthorIntrusion.Common.Persistence
{
	public abstract class PersistenceReaderWriterBase<TSettings>
	{
		#region Properties

		public ProjectMacros Macros { get; set; }
		public Project Project { get; set; }
		public TSettings Settings { get; set; }

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
		protected XmlReader GetXmlReader(XmlReader projectReader,
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
	}
}
