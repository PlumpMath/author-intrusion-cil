#region Namespaces

using System;
using System.IO;
using System.Xml;

using AuthorIntrusion.Contracts.Collections;
using AuthorIntrusion.Contracts.Constants;
using AuthorIntrusion.Contracts.Contents;
using AuthorIntrusion.Contracts.Interfaces;
using AuthorIntrusion.Contracts.IO;
using AuthorIntrusion.Contracts.Structures;

#endregion

namespace AuthorIntrusion.IO
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
		/// Writes out the structure element to the given writer.
		/// </summary>
		/// <param name="writer">The writer.</param>
		/// <param name="structure">The structure.</param>
		/// <param name="depth">The depth.</param>
		private static void Write(
			XmlWriter writer,
			Structure structure,
			int depth)
		{
			// Write out the header for the section.
			writer.WriteStartElement("h" + (depth + 1), Namespaces.Xhtml11);
			writer.WriteString(structure.GetType().Name);
			writer.WriteEndElement();

			// Write out any content associated with the item.
			if (structure is IContentContainer)
			{
				var contentContainer = (IContentContainer) structure;

				writer.WriteStartElement("p", Namespaces.Xhtml11);
				WriteContents(writer, contentContainer.Contents);
				writer.WriteEndElement();
			}

			// Write out any child structures associated with the item.
			if (structure is IStructureContainer)
			{
				var structureContainer = (IStructureContainer) structure;

				foreach (Structure childStructure in structureContainer.Structures)
				{
					Write(writer, childStructure, depth + 1);
				}
			}
		}

		/// <summary>
		/// Writes out the root structure to the given output stream.
		/// </summary>
		/// <param name="outputStream">The output stream.</param>
		/// <param name="rootStructure">The root structure.</param>
		public void Write(
			Stream outputStream,
			Structure rootStructure)
		{
			// Write this using the XHTML writer.
			var settings = new XmlWriterSettings();
			settings.Indent = true;
			settings.CloseOutput = false;
			settings.OmitXmlDeclaration = false;

			using (XmlWriter writer = XmlWriter.Create(outputStream, settings))
			{
				// Check for null coming out of the create.
				if (writer == null)
				{
					throw new Exception("Could not create XML writer");
				}

				// Write out the XML headers.
				writer.WriteStartElement("html", Namespaces.Xhtml11);

				// Write out the head tag.
				writer.WriteStartElement("head");
				writer.WriteStartElement("link");
				writer.WriteAttributeString("rel", "stylesheet");
				writer.WriteAttributeString("type", "text/css");
				writer.WriteAttributeString("href", "style.css");
				writer.WriteEndElement();
				writer.WriteEndElement();

				// Write out the body tag.
				writer.WriteStartElement("body", Namespaces.Xhtml11);
				Write(writer, rootStructure, 0);
				writer.WriteEndElement();

				// Finish up the XHTML.
				writer.WriteEndElement();
			}
		}

		/// <summary>
		/// Writes out the content, wrapping each element in a span tag.
		/// </summary>
		/// <param name="writer">The writer.</param>
		/// <param name="contents">The contents.</param>
		private static void WriteContents(
			XmlWriter writer,
			ContentList contents)
		{
			// Go through the contents and write out each content with
			// classes for each content type.
			bool isFirst = true;

			foreach (Content content in contents)
			{
				// Check to see if we need a space before this content.
				if (isFirst)
				{
					// No leading space in the buffer.
					isFirst = false;
				}
				else if (content.NeedsLeadingSpace)
				{
					writer.WriteString(" ");
				}

				// Write out the span and class.
				writer.WriteStartElement("span");
				writer.WriteAttributeString(
					"class", content.ContentType.ToString().ToLower());

				// If we are writing a container, we need to recursively go into
				// the container, otherwise just write out the string.
				if (content is IContentContainer)
				{
					var contentContainer = (IContentContainer) content;
					WriteContents(writer, contentContainer.Contents);
				}
				else
				{
					// Write out the content string of a non-container.
					writer.WriteString(content.ContentString);
				}

				// Finish up the span tag.
				writer.WriteEndElement();
			}
		}

		#endregion
	}
}