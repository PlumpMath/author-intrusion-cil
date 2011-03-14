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

using System.Collections.Generic;

using AuthorIntrusion.Contracts.Collections;
using AuthorIntrusion.Contracts.Interfaces;

#endregion

namespace AuthorIntrusion.Contracts.Structures
{
	/// <summary>
	/// Represents a single paragraph within the document.
	/// </summary>
	public class Paragraph : Structure, IContentContainer
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Paragraph"/> class.
		/// </summary>
		public Paragraph()
		{
			contents = new ContentList();
		}

		#endregion

		#region Properties

		public override StructureType StructureType {
			get {
				return StructureType.Paragraph;
			}
		}

		#endregion

		#region Contents

		private readonly ContentList contents;

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
		/// <value>The content string.</value>
		public string ContentString
		{
			get { return contents.ContentString; }
		}

		/// <summary>
		/// Gets a count of content container content (i.e. paragraphs) in this
		/// object or child objects.
		/// </summary>
		public override int ParagraphCount
		{
			get
			{
				// This has content, so just include itself. It won't have child
				// structures so the result is just 1.
				return 1;
			}
		}

		/// <summary>
		/// Gets a flattened list of all paragraphs inside the structure.
		/// </summary>
		/// <value>The paragraph list.</value>
		public override IList<Paragraph> ParagraphList
		{
			get
			{
				var paragraphs = new List<Paragraph>();
				paragraphs.Add(this);
				return paragraphs;
			}
		}

		#endregion
	}
}