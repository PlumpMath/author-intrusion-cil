#region Copyright and License

// Copyright (c) 2005-2011, Moonfire Games
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

#endregion

#region Namespaces

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

using AuthorIntrusion.Contracts;
using AuthorIntrusion.Contracts.Collections;
using AuthorIntrusion.Contracts.Constants;
using AuthorIntrusion.Contracts.Contents;
using AuthorIntrusion.Contracts.Interfaces;
using AuthorIntrusion.Contracts.IO;
using AuthorIntrusion.Contracts.Matters;

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
		/// Gets the MIME types associated with this writer.
		/// </summary>
		/// <value>The MIME types.</value>
		public string[] MimeTypes
		{
			get { return new[] { "text/html" }; }
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
		/// Writes the specified writer.
		/// </summary>
		/// <param name="writer">The writer.</param>
		/// <param name="structure">The structure.</param>
		/// <param name="depth">The depth.</param>
		private static void Write(
			XmlWriter writer,
			Matter structure,
			int depth)
		{
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

				foreach (Matter childStructure in structureContainer.Structures)
				{
					Write(writer, childStructure, depth + 1);
				}
			}
		}

		/// <summary>
		/// Writes out the root structure to the given output stream.
		/// </summary>
		/// <param name="outputStream">The output stream.</param>
		/// <param name="document">The document to write out.</param>
		public void Write(
			Stream outputStream,
			Document document)
		{
			// Verify our values.
			if (outputStream == null)
			{
				throw new ArgumentNullException("writer");
			}

			if (document == null)
			{
				throw new ArgumentNullException("document");
			}

			// Write this using the XHTML writer.
			var settings = new XmlWriterSettings();
			settings.Indent = false;
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
				Write(writer, document.Structure, 0);
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

				// Figure out all the classes associated with this.
				List<string> classes = new List<string>();
				classes.Add(content.ContentType.ToString());

				foreach (IElementTag tag in content.Tags)
				{
					classes.Add(tag.ToString());
				}

				// Write out the span and class.
				writer.WriteStartElement("span");
				writer.WriteAttributeString(
					"class",
					String.Join(" ", classes.ToArray()).ToLower());

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