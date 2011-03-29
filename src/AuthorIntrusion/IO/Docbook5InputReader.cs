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
using System.Text.RegularExpressions;
using System.Xml;

using AuthorIntrusion.Contracts;
using AuthorIntrusion.Contracts.Constants;
using AuthorIntrusion.Contracts.Enumerations;
using AuthorIntrusion.Contracts.Interfaces;
using AuthorIntrusion.Contracts.Structures;

#endregion

namespace AuthorIntrusion.IO
{
	/// <summary>
	/// Defines an input reader that takes Docbook 5 XML and produces the
	/// internal structure. Docbook elements that are not understood are ignored
	/// and dropped.
	/// </summary>
	public class Docbook5InputReader : XmlInputReaderBase
	{
		#region Identification

		/// <summary>
		/// Gets the file masks that are commonly associated with this input
		/// file format.
		/// </summary>
		/// <value>The file mask.</value>
		public override string[] FileExtensions
		{
			get { return new[] { ".xml" }; }
		}

		/// <summary>
		/// Gets the MIME types associated with this writer.
		/// </summary>
		/// <value>The MIME types.</value>
		public override string[] MimeTypes
		{
			get { return new[] { "text/xml" }; }
		}

		/// <summary>
		/// Gets the name of the input file.
		/// </summary>
		/// <value>The name.</value>
		public override string Name
		{
			get { return "Docbook 5"; }
		}

		/// <summary>
		/// Determines whether this instance can read XML files with the given root
		/// element.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <returns>
		/// 	<c>true</c> if this instance [can read element] the specified reader; otherwise, <c>false</c>.
		/// </returns>
		protected override bool CanReadElement(XmlReader reader)
		{
			if (reader.NamespaceURI != Namespaces.Docbook5 || reader["version"] != "5.0")
			{
				return false;
			}

			return true;
		}

		#endregion

		#region Reading

		/// <summary>
		/// Contains the number of recursive &lt;sect&gt; elements we've parsed.
		/// </summary>
		private int sectionDepth = 0;

		/// <summary>
		/// Reads the specified input stream and returns a structure elements.
		/// If there is any problems with reading the input, this should throw
		/// an exception and never return a null root structure.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <returns></returns>
		protected override Document Read(XmlReader reader)
		{
			// This implements a very simple Docbook 5 XML reader that ignores
			// all the elements outside of the scope of this application and 
			// creates a simplified structure.
			var context = new List<Element>();
			Structure rootStructure = null;

			while (reader.Read())
			{
				switch (reader.NodeType)
				{
					case XmlNodeType.Element:
						ReadElement(reader, context);

						if (context.Count == 1)
						{
							rootStructure = context[0] as Structure;
						}
						break;

					case XmlNodeType.EndElement:
						ReadEndElement(reader, context);
						break;

					case XmlNodeType.Text:
						ReadText(reader, context);
						break;
				}
			}

			// If we still have a null root structure, something is wrong.
			if (rootStructure == null)
			{
				throw new Exception("Cannot identify the root level element");
			}

			if (rootStructure == null)
			{
				throw new Exception("Root element is not a structure");
			}

			// There is nothing wrong with the parse, so return the root.
			var document = new Document();

			document.Structure = rootStructure;

			return document;
		}

		/// <summary>
		/// Reads the element from the XML reader and parses it.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <param name="context">The context.</param>
		private void ReadElement(
			XmlReader reader,
			List<Element> context)
		{
			// If we aren't a DocBook element, just ignore it.
			if (reader.NamespaceURI != Namespaces.Docbook5)
			{
				return;
			}

			// Get the last item in the context.
			Structure parent = null;

			if (context.Count > 0)
			{
				parent = context[context.Count - 1] as Structure;
			}

			// Switch based on the local tag.
			Element element;

			switch (reader.LocalName)
			{
				case "book":
					element = new Section(StructureType.Book);
					break;

				case "chapter":
					var chapter = new Section(StructureType.Chapter);
					element = chapter;

					if (parent != null && parent is IStructureContainer)
					{
						((IStructureContainer) parent).Structures.Add(chapter);
					}
					break;

				case "article":
					element = new Section(StructureType.Article);
					break;

				case "section":
					// Increment the section counter and figure out the section.
					Section section;

					sectionDepth++;

					switch (sectionDepth)
					{
						case 1:
							section = new Section(StructureType.Section);
							break;
						case 2:
							section = new Section(StructureType.SubSection);
							break;
						case 3:
							section = new Section(StructureType.SubSubSection);
							break;
						default:
							throw new Exception("Cannot handle a <sect> depths greater than 3.");
					}

					element = section;

					if (parent != null && parent is IStructureContainer)
					{
						((IStructureContainer) parent).Structures.Add(section);
					}
					break;

				case "para":
				case "simpara":
					var paragraph = new Paragraph();
					element = paragraph;

					if (parent != null && parent is IStructureContainer)
					{
						((IStructureContainer) parent).Structures.Add(paragraph);
					}

					break;

				case "quote":
					if (parent != null && parent is IContentContainer)
					{
						((IContentContainer) parent).Contents.Add("\"");
					}

					return;

				case "title":
					Section sectionInfo = parent as Section;

					if (sectionInfo != null)
					{
						sectionInfo.Title = reader.ReadElementString();
					}

					return;

				default:
					// Unknown type, so just skip it.
					return;
			}

			// Add the structure to the context.
			context.Add(element);
		}

		/// <summary>
		/// Reads the element from the XML reader and parses it.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <param name="context">The context.</param>
		private void ReadEndElement(
			XmlReader reader,
			List<Element> context)
		{
			// If we aren't a DocBook element, just ignore it.
			if (reader.NamespaceURI != Namespaces.Docbook5)
			{
				return;
			}

			// Switch based on the local tag.
			switch (reader.LocalName)
			{
				case "quote":
					var parent = (IContentContainer) context[context.Count - 1];
					parent.Contents.Add("\"");
					break;

				case "section":
					// Remove the last item which should be this element.
					context.RemoveAt(context.Count - 1);

					// Decrement the section depths.
					sectionDepth--;
					break;

				case "book":
				case "chapter":
				case "article":
					// Remove the last item which should be this element.
					context.RemoveAt(context.Count - 1);
					break;

				case "para":
				case "simpara":
					// For paragraphs, also trim the contents.
					var paragraph = (Paragraph) context[context.Count - 1];
					paragraph.SetText(
						Regex.Replace(paragraph.ContentString.Trim(), @"\s+", " "));

					// Remove the last item which should be this element.
					context.RemoveAt(context.Count - 1);
					break;
			}
		}

		/// <summary>
		/// Reads the text from the given XML string.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <param name="context">The context.</param>
		private static void ReadText(
			XmlReader reader,
			List<Element> context)
		{
			// Figure out where to put this text content.
			if (context.Count == 0)
			{
				throw new Exception("Cannot figure out context to add contents");
			}

			// Make sure the last item in the context can contain contents.
			var container = context[context.Count - 1] as IContentContainer;

			if (container == null)
			{
				// Ignore it since the item can't handle it. This is used for things like
				// titles.
				return;
			}

			// Wrap the text into an unparsed string and add it to the container.
			container.Contents.Add(reader.Value);
		}

		#endregion
	}
}