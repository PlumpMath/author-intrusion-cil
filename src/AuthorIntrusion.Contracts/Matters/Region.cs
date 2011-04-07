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

using System;
using System.Diagnostics;

#endregion

namespace AuthorIntrusion.Contracts.Matters
{
	/// <summary>
	/// Represents a region of a document, such as a chapter, section, or
	/// a book. All regions have a title, which is considered it contents and
	/// zero or more <see cref="Matter"/> objects.
	/// 
	/// All regions are a subset of the document's matter list and have a view
	/// into that list that represents the region's extents. A region can be
	/// created without an underlying list, but will throw an exception for
	/// many of the properties and methods.
	/// </summary>
	public class Region : Matter, IMattersContainer
	{
		#region Fields

		private readonly MatterCollection matters;
		private string title;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Region"/> class.
		/// </summary>
		public Region()
		{
			matters = new MatterCollection(this);
			matters.ParagraphChanged += OnParagraphChanged;
			title = String.Empty;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Region"/> class.
		/// </summary>
		public Region(RegionType regionType)
			: this()
		{
			RegionType = regionType;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Region"/> class.
		/// </summary>
		public Region(
			RegionType regionType,
			string title)
			: this(regionType)
		{
			Title = title;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the document associated with this matter.
		/// </summary>
		/// <value>
		/// The document.
		/// </value>
		public override Document ParentDocument
		{
			get
			{
				return ParentContainer == null ? null : ParentContainer.ParentDocument;
			}
		}

		/// <summary>
		/// Gets the type of the structure.
		/// </summary>
		/// <value>The type of the structure.</value>
		public override MatterType MatterType
		{
			get { return MatterType.Region; }
		}

		/// <summary>
		/// Gets or sets the type of region.
		/// </summary>
		/// <value>
		/// The type of the region.
		/// </value>
		public RegionType RegionType { get; set; }

		/// <summary>
		/// Gets or sets the title of the section.
		/// </summary>
		/// <value>The title.</value>
		public string Title
		{
			get { return title; }
			set { title = value ?? String.Empty; }
		}

		#endregion

		#region Editing

		/// <summary>
		/// Contains a list of matters associated with this region.
		/// </summary>
		public MatterCollection Matters
		{
			[DebuggerStepThrough]
			get { return matters; }
		}

		/// <summary>
		/// Creates an version of itself, but with no text or contents.
		/// </summary>
		/// <returns></returns>
		public override Matter CreateEmptyClone()
		{
			var section = new Region(RegionType);
			return section;
		}

		/// <summary>
		/// Called when a contained paragraph changes.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="AuthorIntrusion.Contracts.Matters.ParagraphChangedEventArgs"/> instance containing the event data.</param>
		private void OnParagraphChanged(
			object sender,
			ParagraphChangedEventArgs e)
		{
			RaiseParagraphChanged(e);
		}

		#endregion

		#region Contents

		/// <summary>
		/// Retrieves the string for the structural context. This will be the
		/// title for sections and content for paragraphs.
		/// </summary>
		/// <returns></returns>
		public override string GetContents()
		{
			return Title;
		}

		/// <summary>
		/// Sets the text of the structure. For sections, this will be the title
		/// and for paragraphs, it will be the unparsed contents.
		/// </summary>
		/// <param name="text">The text.</param>
		public override void SetContents(string text)
		{
			Title = text;
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
			return string.Format("Region ({0}): {1}", RegionType, Title);
		}

		#endregion
	}
}