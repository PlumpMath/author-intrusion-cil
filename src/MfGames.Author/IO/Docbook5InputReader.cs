#region Namespaces

using System;
using System.Collections.Generic;
using System.Xml;

using MfGames.Author.Contract.Constants;
using MfGames.Author.Contract.Interfaces;
using MfGames.Author.Contract.Structures;

#endregion

namespace MfGames.Author.IO
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
		/// Reads the specified input stream and returns a structure elements.
		/// If there is any problems with reading the input, this should throw
		/// an exception and never return a null root structure.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <returns></returns>
		protected override Structure Read(XmlReader reader)
		{
			// This implements a very simple Docbook 5 XML reader that ignores
			// all the elements outside of the scope of this application and 
			// creates a simplified structure.
			List<Structure> structureContext = new List<Structure>();
			Structure rootStructure = null;

			while (reader.Read())
			{
				switch (reader.NodeType)
				{
					case XmlNodeType.Element:
						ReadElement(reader, structureContext);

						if (structureContext.Count == 1)
						{
							rootStructure = structureContext[0];
						}
						break;

					case XmlNodeType.EndElement:
						ReadEndElement(reader, structureContext);
						break;

					case XmlNodeType.Text:
						ReadText(reader, structureContext);
						break;
				}
			}

			// If we still have a null root structure, something is wrong.
			if (rootStructure == null)
			{
				throw new Exception("Cannot identify the root level element");
			}

			if (!(rootStructure is Structure))
			{
				throw new Exception("Root structure does not define StructureBase");
			}

			// There is nothing wrong with the parse, so return the root.
			return rootStructure as Structure;
		}

		/// <summary>
		/// Reads the element from the XML reader and parses it.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <param name="context">The context.</param>
		private static void ReadElement(
			XmlReader reader,
			List<Structure> context)
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
				parent = context[context.Count - 1];
			}

			// Switch based on the local tag.
			Structure structure;

			switch (reader.LocalName)
			{
				case "book":
					structure = new StructureContainerStructure();
					break;

				case "chapter":
					var chapter = new StructureContentContainerStructure();
					structure = chapter;

					if (parent != null && parent is IStructureContainer)
					{
						((IStructureContainer) parent).Structures.Add(chapter);
					}
					break;

				case "article":
					structure = new StructureContentContainerStructure();
					break;

				case "section":
					var section = new StructureContainerStructure();
					structure = section;

					if (parent != null && parent is IStructureContainer)
					{
						((IStructureContainer) parent).Structures.Add(chapter);
					}
					break;

				case "para":
					var paragraph = new ContentContainerStructure();
					structure = paragraph;

					if (parent != null && parent is IStructureContainer)
					{
						((IStructureContainer) parent).Structures.Add(chapter);
					}
					break;

				default:
					// Unknown type, so just skip it.
					return;
			}

			// Add the structure to the context.
			context.Add(structure);
		}

		/// <summary>
		/// Reads the element from the XML reader and parses it.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <param name="context">The context.</param>
		private static void ReadEndElement(
			XmlReader reader,
			List<Structure> context)
		{
			// If we aren't a DocBook element, just ignore it.
			if (reader.NamespaceURI != Namespaces.Docbook5)
			{
				return;
			}

			// Switch based on the local tag.
			switch (reader.LocalName)
			{
				case "book":
				case "chapter":
				case "article":
				case "section":
				case "para":
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
		private static void ReadText(XmlReader reader, List<Structure> context)
		{
			// Figure out where to put this text content.
			if (context.Count == 0)
			{
				throw new Exception("Cannot figure out context to add contents");
			}

			// Make sure the last item in the context can contain contents.
			IContentContainer container = context[context.Count - 1] as IContentContainer;

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