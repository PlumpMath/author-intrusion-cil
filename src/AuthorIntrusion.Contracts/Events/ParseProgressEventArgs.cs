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

using System;

#endregion

namespace AuthorIntrusion.Contracts.Events
{
	/// <summary>
	/// Defines the event arguments for showing the parsing progress.
	/// </summary>
	public class ParseProgressEventArgs : EventArgs
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="ParseProgressEventArgs"/> class.
		/// </summary>
		/// <param name="paragraphsProcessed">The paragraphs processed.</param>
		/// <param name="paragraphCount">The paragraph count.</param>
		public ParseProgressEventArgs(
			int paragraphsProcessed,
			int paragraphCount)
		{
			this.paragraphsProcessed = paragraphsProcessed;
			this.paragraphCount = paragraphCount;
		}

		#endregion

		#region Progress

		private readonly int paragraphCount;
		private readonly int paragraphsProcessed;

		/// <summary>
		/// Gets the total paragraph count.
		/// </summary>
		/// <value>The paragraph count.</value>
		public int ParagraphCount
		{
			get { return paragraphCount; }
		}

		/// <summary>
		/// Gets the number of paragraphs processed.
		/// </summary>
		/// <value>The paragraphs processed.</value>
		public int ParagraphsProcessed
		{
			get { return paragraphsProcessed; }
		}

		#endregion
	}
}