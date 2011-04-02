#region Copyright and License

// Copyright (c) 2011, Moonfire Games
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

using System.Text;

using AuthorIntrusion.Contracts.Algorithms;

#endregion

namespace AuthorIntrusion.Contracts.Structures
{
	/// <summary>
	/// Creates a visitor that generates a thumbnail view of a document. The
	/// thumbnail is a single character for every structure element, usually
	/// by their initial abbreviation and with paragraphs as "p".
	/// </summary>
	public class DocumentThumbnailer : DocumentVisitor
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="DocumentThumbnailer"/> class.
		/// </summary>
		public DocumentThumbnailer()
		{
			thumbnail = new StringBuilder();
		}

		#endregion

		#region Properties

		private readonly StringBuilder thumbnail;

		/// <summary>
		/// Gets the thumbnail of the document.
		/// </summary>
		public string Thumbnail
		{
			get { return thumbnail.ToString(); }
		}

		#endregion

		#region Visiting

		/// <summary>
		/// Called when the visitor enters a paragraph.
		/// </summary>
		/// <param name="paragraph">The paragraph.</param>
		/// <returns>
		/// True if the visitor should continue to recurse.
		/// </returns>
		protected override bool OnBeginParagraph(Paragraph paragraph)
		{
			// Paragraphs are lower-case P.
			thumbnail.Append('p');

			// We don't want to go into content.
			return false;
		}

		/// <summary>
		/// Called when the visitor enters a section.
		/// </summary>
		/// <param name="section">The section.</param>
		/// <returns>
		/// True if the visitor should continue to recurse.
		/// </returns>
		protected override bool OnBeginSection(Section section)
		{
			// Add an abbrevation of the type.
			switch (section.StructureType)
			{
				case StructureType.Section:
					thumbnail.Append('1');
					break;
				case StructureType.SubSection:
					thumbnail.Append('2');
					break;
				case StructureType.SubSubSection:
					thumbnail.Append('3');
					break;
				default:
					// Just add the first letter of the structure.
					thumbnail.Append(section.StructureType.ToString()[0]);
					break;
			}

			// Recurse into the next sections.
			return true;
		}

		#endregion
	}
}