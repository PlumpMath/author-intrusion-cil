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

using AuthorIntrusion.Contracts.Collections;

#endregion

namespace AuthorIntrusion.Contracts.Matters
{
	/// <summary>
	/// An event argument class that contains a paragraph that changed.
	/// </summary>
	public class ParagraphChangedEventArgs : EventArgs
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="ParagraphChangedEventArgs"/> class.
		/// </summary>
		/// <param name="paragraph">The paragraph.</param>
		/// <param name="oldContents">The old contents.</param>
		public ParagraphChangedEventArgs(
			Paragraph paragraph,
			ContentList oldContents)
		{
			Paragraph = paragraph;
			OldContents = oldContents;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Contains the old contents of the paragraph.
		/// </summary>
		public ContentList OldContents { get; private set; }

		/// <summary>
		/// Contains the paragraph of the event.
		/// </summary>
		public Paragraph Paragraph { get; private set; }

		#endregion
	}
}