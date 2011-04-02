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

using AuthorIntrusion.Contracts.Collections;
using AuthorIntrusion.Contracts.Interfaces;
using AuthorIntrusion.Contracts.Matters;

#endregion

namespace AuthorIntrusion.Contracts.Structures
{
	/// <summary>
	/// Represents a single paragraph within the document.
	/// </summary>
	public class Paragraph : Matter, IContentContainer
	{
		#region Fields

		private readonly ContentList contents;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Paragraph"/> class.
		/// </summary>
		public Paragraph()
		{
			contents = new ContentList();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Paragraph"/> class.
		/// </summary>
		/// <param name="initialText">The initial text.</param>
		public Paragraph(string initialText)
			: this()
		{
			SetContents(initialText);
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the type of the structure.
		/// </summary>
		/// <value>The type of the structure.</value>
		public override MatterType MatterType
		{
			get { return MatterType.Paragraph; }
		}

		#endregion

		#region Relationships

		/// <summary>
		/// Creates an version of itself, but with no text or contents.
		/// </summary>
		/// <returns></returns>
		public override Matter CreateEmptyClone()
		{
			return new Paragraph();
		}

		#endregion

		#region Contents

		/// <summary>
		/// Gets the contents inside the structure.
		/// </summary>
		/// <value>The contents.</value>
		public ContentList Contents
		{
			get { return contents; }
		}

		/// <summary>
		/// Gets a flattened string that represents the entire container.
		/// </summary>
		/// <value>
		/// The content string.
		/// </value>
		public string ContentString
		{
			get { return contents.ContentString; }
		}

		/// <summary>
		/// Retrieves the string for the structural context. This will be the
		/// title for sections and content for paragraphs.
		/// </summary>
		/// <returns></returns>
		public override string GetContents()
		{
			return ContentString;
		}

		/// <summary>
		/// Sets the text of the structure. For sections, this will be the title
		/// and for paragraphs, it will be the unparsed contents.
		/// </summary>
		/// <param name="text">The text.</param>
		public override void SetContents(string text)
		{
			// Clear out the previous contents and set the new contents.
			contents.Clear();
			contents.Add(text);
		}

		#endregion

		#region Conversion

		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </returns>
		public override string ToString()
		{
			// Get the content string and trim it to 50 characters.
			string trimmedContents = ContentString;

			if (trimmedContents.Length > 50)
			{
				trimmedContents = trimmedContents.Substring(0, 47) + "...";
			}

			return trimmedContents;
		}

		#endregion
	}
}