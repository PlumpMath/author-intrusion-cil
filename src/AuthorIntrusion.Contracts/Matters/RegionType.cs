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

namespace AuthorIntrusion.Contracts.Matters
{
	/// <summary>
	/// Identifies the type of Region.
	/// </summary>
	public enum RegionType
	{
		/// <summary>
		/// An article or stand-alone piece outside of a book.
		/// </summary>
		Article,

		/// <summary>
		/// A section of text, typically in an article or chapter.
		/// </summary>
		Section1,

		/// <summary>
		/// A sub-section inside a section.
		/// </summary>
		Section2,

		/// <summary>
		/// A sub-section inside a sub-section.
		/// </summary>
		Section3,

		/// <summary>
		/// A chapter of a book.
		/// </summary>
		Chapter,

		/// <summary>
		/// A collection of chapters.
		/// </summary>
		Book,
	}
}