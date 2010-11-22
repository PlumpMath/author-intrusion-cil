#region Namespaces

using System.IO;
using System.Xml;

using MfGames.Author.Contract.Constants;
using MfGames.Author.Contract.Interfaces;
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
				writer.WriteString(contentContainer.ContentString);
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

		#endregion
	}
}