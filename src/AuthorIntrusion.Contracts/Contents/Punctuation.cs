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

using AuthorIntrusion.Contracts.Enumerations;

#endregion

namespace AuthorIntrusion.Contracts.Contents
{
	/// <summary>
	/// Represents a puncuation symbol in the string. This can be terminating
	/// or non-terminating depending on the type of puncuation.
	/// </summary>
	public class Puncuation : Content
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Puncuation"/> class.
		/// </summary>
		/// <param name="text">The text.</param>
		public Puncuation(string text)
		{
			this.text = text;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Puncuation"/> class.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="isTerminating">if set to <c>true</c> [is terminating].</param>
		public Puncuation(
			string text,
			bool isTerminating)
		{
			this.text = text;
			this.isTerminating = isTerminating;
		}

		#endregion

		#region Contents

		private readonly string text;
		private bool isTerminating;

		/// <summary>
		/// Contains a flattened representation of the content.
		/// </summary>
		/// <value>The content string.</value>
		public override string ContentString
		{
			get { return text; }
		}

		/// <summary>
		/// Gets the type of content this object represents.
		/// </summary>
		/// <value>The type of the content.</value>
		public override ContentType ContentType
		{
			get { return isTerminating ? ContentType.Terminator : ContentType.Punctuation; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this puncuation is
		/// sentence-terminating.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is terminating; otherwise, <c>false</c>.
		/// </value>
		public bool IsTerminating
		{
			get { return isTerminating; }
			set { isTerminating = value; }
		}

		/// <summary>
		/// Gets a value indicating whether this content is normally formatted
		/// with a leading space.
		/// </summary>
		/// <value><c>true</c> if [needs leading space]; otherwise, <c>false</c>.</value>
		public override bool NeedsLeadingSpace
		{
			get { return false; }
		}

		#endregion
	}
}