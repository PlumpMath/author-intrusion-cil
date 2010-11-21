#region Namespaces

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

using MfGames.Author.Contract.Constants;
using MfGames.Author.Contract.Contents;
using MfGames.Author.Contract.IO;
using MfGames.Author.Contract.Structures;

#endregion

namespace MfGames.Author.IO
{
	/// <summary>
	/// Implements a writer that takes the internal structure and outputs a single
	/// xHTML file with the contents.
	/// </summary>
	public class XhtmlOutputWriter : IOutputWriter
	{
		#region Identification

		/// <summary>
		/// Gets the file extensions that are commonly associated with this
		/// writer.
		/// </summary>
		/// <value>The file mask.</value>
		public string[] FileExtensions
		{
			get { return new[] { ".html" }; }
		}

		/// <summary>
		/// Gets the name of the output format.
		/// </summary>
		/// <value>The name.</value>
		public string Name
		{
			get { return "xHTML"; }
		}

		#endregion

		#region Writing

		/// <summary>
		/// Writes out the root structure to the given output stream.
		/// </summary>
		/// <param name="outputStream">The output stream.</param>
		/// <param name="rootStructure">The root structure.</param>
		public void Write(
			Stream outputStream,
			StructureBase rootStructure)
		{
			// Write this using the XHTML writer.
			var settings = new XmlWriterSettings();
			settings.Indent = true;
			settings.CloseOutput = false;
			settings.OmitXmlDeclaration = false;

			using (XmlWriter writer = XmlWriter.Create(outputStream, settings))
			{
				// Write out the XML headers.
				writer.WriteStartElement("html", Namespaces.Xhtml11);

				// Write out the body tag.
				writer.WriteStartElement("body", Namespaces.Xhtml11);
				WriteStructure(writer, rootStructure, 0);
				writer.WriteEndElement();

				// Finish up the XHTML.
				writer.WriteEndElement();
			}
		}

		/// <summary>
		/// Writes out the contents of a paragraph.
		/// </summary>
		/// <param name="writer">The writer.</param>
		/// <param name="paragraph">The paragraph.</param>
		private static void WriteParagraph(
			XmlWriter writer,
			Paragraph paragraph)
		{
			// Build up a list of strings sentences in the paragraph.
			List<string> sentences = new List<string>();

			// Go through the unparsed content.
			foreach (UnparsedString unparsedString in paragraph.UnparsedContents)
			{
				sentences.Add(unparsedString.Contents);
			}

			// Write out the resulting paragraph.
			writer.WriteString(String.Join(" ", sentences.ToArray()));
		}

		/// <summary>
		/// Writes out the structure element to the given writer.
		/// </summary>
		/// <param name="writer">The writer.</param>
		/// <param name="structure">The structure.</param>
		/// <param name="depth">The depth.</param>
		private static void WriteStructure(
			XmlWriter writer,
			StructureBase structure,
			int depth)
		{
			// Write out the header for the section.
			writer.WriteStartElement("h" + (depth + 1), Namespaces.Xhtml11);
			writer.WriteString(structure.GetType().Name);
			writer.WriteEndElement();

			// Write out any paragraphs associated with the item.
			if (structure is SectionParagraphContainerBase)
			{
				SectionParagraphContainerBase container = (SectionParagraphContainerBase) structure;

				// Write out the paragraphs inside the container.
				foreach (Paragraph paragraph in container.Paragraphs)
				{
					writer.WriteStartElement("p", Namespaces.Xhtml11);
					WriteParagraph(writer, paragraph);
					writer.WriteEndElement();
				}

				// Write out the inner sections.
				foreach (Section section in container.Sections)
				{
					WriteStructure(writer, section, depth + 1);
				}
			}

			// Catch any other components.
			if (structure is Book)
			{
				Book book = (Book) structure;

				foreach (Chapter chapter in book.Chapters)
				{
					WriteStructure(writer, chapter, depth + 1);
				}
			}
		}

		#endregion
	}
}