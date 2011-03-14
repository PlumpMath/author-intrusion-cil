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

namespace AuthorIntrusion.Contracts.Enumerations
{
	/// <summary>
	/// Defines the type of contents that the system uses.
	/// </summary>
	public enum ContentType
	{
		/// <summary>
		/// Indicates that the content is unparsed text.
		/// </summary>
		Unparsed,

		/// <summary>
		/// Indicates that the content is a single or effectively single
		/// word.
		/// </summary>
		Word,

		/// <summary>
		/// Indicates that the content represents non-sentence-terminating
		/// puncuation.
		/// </summary>
		Punctuation,

		/// <summary>
		/// Indicates that the content is sentence-terminating puncuation.
		/// </summary>
		Terminator,

		/// <summary>
		/// Indicates that the content is a quoted string.
		/// </summary>
		Quote,

		/// <summary>
		/// Indicates that the content is a sentence.
		/// </summary>
		Sentence,

		/// <summary>
		/// Indicates a phrase of text.
		/// </summary>
		Phrase,
	}
}