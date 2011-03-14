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
	/// Contains a single sematic word for parsing.
	/// </summary>
	public class Word : Content
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Word"/> class.
		/// </summary>
		/// <param name="text">The text.</param>
		public Word(string text)
		{
			this.text = text;
		}

		#endregion

		#region Contents

		private readonly string text;

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
			get { return ContentType.Word; }
		}

		/// <summary>
		/// Gets the text of this word.
		/// </summary>
		/// <value>The text.</value>
		public string Text
		{
			get { return text; }
		}

		#endregion
	}
}