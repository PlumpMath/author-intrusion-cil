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
using System.Collections.Generic;
using System.Diagnostics;

using AuthorIntrusion.Contracts.Collections;

#endregion

namespace AuthorIntrusion.Contracts.Matters
{
	/// <summary>
	/// Represents document matter (i.e., front, body, and back) for a document.
	/// These represent either regions in the document (chapter, section, etc.),
	/// paragraphs (text contents), or breaks of various types.
	/// </summary>
	public abstract class Matter : Element
	{
		#region Fields

		private readonly Dictionary<object, object> dataDictionary;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Matter"/> class.
		/// </summary>
		protected Matter()
		{
			dataDictionary = new Dictionary<object, object>();
		}

		#endregion

		#region Properties

		/// <summary>
		/// Contains a data dictionary which can be used to associate data
		/// with a given structure.
		/// </summary>
		public IDictionary<object, object> DataDictionary
		{
			get { return dataDictionary; }
		}

		/// <summary>
		/// Gets the zero-based depth of the matter inside the document.
		/// </summary>
		public int Depth
		{
			get
			{
				if (ParentContainer == null)
				{
					return 0;
				}

				return ParentContainer.Depth + 1;
			}
		}

		/// <summary>
		/// Gets or sets the document associated with this matter.
		/// </summary>
		/// <value>
		/// The document.
		/// </value>
		public abstract Document ParentDocument { get; }

		/// <summary>
		/// Gets the index of the matter in the document.
		/// </summary>
		/// <value>
		/// The index of the matter.
		/// </value>
		public int DocumentIndex
		{
			get { return ParentDocument.Matters.IndexOf(this); }
		}

		/// <summary>
		/// Gets the type of the structure.
		/// </summary>
		/// <value>The type of the structure.</value>
		public abstract MatterType MatterType { get; }

		/// <summary>
		/// Gets or sets the container that encapsulates this one.
		/// </summary>
		/// <value>
		/// The matter container.
		/// </value>
		public IMattersContainer ParentContainer
		{
			[DebuggerStepThrough]
			get { return Parent as IMattersContainer; }
			[DebuggerStepThrough]
			set { Parent = value as Element; }
		}

		/// <summary>
		/// Gets the index of this instance in its parent container.
		/// </summary>
		/// <value>
		/// The index of the item in the parent.
		/// </value>
		public int ParentIndex
		{
			get
			{
				if (ParentContainer == null)
				{
					return -1;
				}

				return ParentContainer.Matters.IndexOf(this);
			}
		}

		#endregion

		#region Document Editing

		/// <summary>
		/// Occurs when a paragraph contents have been changed.
		/// </summary>
		public event EventHandler<ParagraphChangedEventArgs> ParagraphChanged;

		/// <summary>
		/// Creates an version of itself, but with no text or contents. This is
		/// used to duplicate lines in the text editor.
		/// </summary>
		/// <returns></returns>
		public abstract Matter CreateEmptyClone();

		/// <summary>
		/// Raises the paragraph changed event.
		/// </summary>
		/// <param name="paragraph">The paragraph.</param>
		/// <param name="oldContents">The old contents.</param>
		protected void RaiseParagraphChanged(Paragraph paragraph, ContentList oldContents)
		{
			RaiseParagraphChanged(new ParagraphChangedEventArgs(paragraph, oldContents));
		}

		/// <summary>
		/// Raises the paragraph changed event.
		/// </summary>
		/// <param name="e">The <see cref="AuthorIntrusion.Contracts.Matters.ParagraphChangedEventArgs"/> instance containing the event data.</param>
		protected void RaiseParagraphChanged(ParagraphChangedEventArgs e)
		{
			if (ParagraphChanged != null)
			{
				ParagraphChanged(this, e);
			}
		}

		#endregion

		#region Contents

		/// <summary>
		/// Retrieves the string for the structural context. This will be the
		/// title for sections and content for paragraphs.
		/// </summary>
		/// <returns>An unformatted string representing the contents.</returns>
		public abstract string GetContents();

		/// <summary>
		/// Sets the text of the structure. For sections, this will be the title
		/// and for paragraphs, it will be the unparsed contents.
		/// </summary>
		/// <param name="text">The contexts text to set.</param>
		public abstract void SetContents(string text);

		#endregion
	}
}