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

using C5;

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
	public class Region : Matter
	{
		#region Fields

		private IList<Matter> matters;
		private string title;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Region"/> class.
		/// </summary>
		public Region()
		{
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
		/// Gets a value indicating whether this instance is connected to an
		/// underlying list.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is connected; otherwise, <c>false</c>.
		/// </value>
		public bool IsConnected
		{
			get { return matters != null; }
		}

		/// <summary>
		/// Contains a list of matters associated with this region.
		/// </summary>
		public IList<Matter> Matters
		{
			get
			{
				VerifyUnderlyingMatters();
				return matters;
			}
		}

		/// <summary>
		/// Connects a list of matters to the region.
		/// </summary>
		public void ConnectToDocument(IList<Matter> newMatters)
		{
			if (newMatters == null)
			{
				throw new ArgumentNullException("newMatters");
			}

			matters = newMatters;
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
		/// Disconnects the region's list from the underlying document list.
		/// It also disposes of the list to ensure the view is properly removed.
		/// </summary>
		public void DisconnectFromDocument()
		{
			if (matters != null)
			{
				matters.Dispose();
			}

			matters = null;
		}

		/// <summary>
		/// Verifies that the underlying matters exists in the region.
		/// </summary>
		private void VerifyUnderlyingMatters()
		{
			if (matters == null)
			{
				throw new InvalidOperationException(
					"Cannot use a region's matters until it has been set.");
			}
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