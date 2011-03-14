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

using AuthorIntrusion.Contracts;

using MfGames.GtkExt.LineTextEditor.Buffers;
using MfGames.GtkExt.LineTextEditor.Interfaces;

#endregion

namespace AuthorIntrusionGtk.Editors
{
	/// <summary>
	/// Implements an indicator buffer wrapped around an Author Intrusion
	/// document.
	/// </summary>
	public class DocumentLineIndicatorBuffer : LineBuffer
	{
		#region Constructors

		public DocumentLineIndicatorBuffer(Document document)
		{
			// Save the member variables.
			if (document == null)
			{
				throw new ArgumentNullException("document");
			}

			this.document = document;
		}

		#endregion

		#region Document

		private Document document;

		#endregion

		#region Buffer

		/// <summary>
		/// Gets the number of lines in the buffer.
		/// </summary>
		/// <value>The line count.</value>
		public override int LineCount { get { return 1; } }

		/// <summary>
		/// If set to <see langword="true"/>, the buffer is read-only and the editing
		/// commands should throw an <see cref="InvalidOperationException"/>.
		/// </summary>
		public override bool ReadOnly { get { return false; } }

		#endregion

		#region Lines

		public override int GetLineLength(int lineIndex)
		{
			return document != null ? 3 : 3;
		}

		public override string GetLineNumber(int lineIndex)
		{
			return "1";
		}

		public override string GetLineText(int lineIndex, int startIndex, int endIndex)
		{
			return "Bob";
		}

		#endregion

		#region Operations

		public override void Do(ILineBufferOperation operation)
		{
		}

		#endregion
	}
}