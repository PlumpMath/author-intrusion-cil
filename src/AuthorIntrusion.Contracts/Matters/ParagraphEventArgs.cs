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

#endregion

namespace AuthorIntrusion.Contracts.Matters
{
	/// <summary>
	/// An event argument that only contains the paragraph.
	/// </summary>
	public class ParagraphEventArgs : EventArgs
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="ParagraphEventArgs"/> class.
		/// </summary>
		/// <param name="paragraph">The paragraph.</param>
		public ParagraphEventArgs(Paragraph paragraph)
		{
			if (paragraph == null)
			{
				throw new ArgumentNullException("paragraph");
			}

			Paragraph = paragraph;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Contains the paragraph of the event.
		/// </summary>
		public Paragraph Paragraph { get; private set; }

		#endregion
	}
}