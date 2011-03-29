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

namespace AuthorIntrusion.Contracts.Structures
{
	/// <summary>
	/// Defines the types of structure. Except for Paragraph, all the others are
	/// more of hints used by the system for rendering.
	/// </summary>
	public enum StructureType
	{
		/// <summary>
		/// Represents a stand-alone article.
		/// </summary>
		[ContainsStructure(Section)]
		[ContainsStructure(Paragraph)]
		Article,

		/// <summary>
		/// Represents a book which usually contains chapters.
		/// </summary>
		[ContainsStructure(Chapter)]
		Book,

		/// <summary>
		/// Represents a chapter which contains sections.
		/// </summary>
		[ContainsStructure(Section)]
		[ContainsStructure(Paragraph)]
		Chapter,

		/// <summary>
		/// Represents a section in a chapter or article.
		/// </summary>
		[ContainsStructure(SubSection)]
		[ContainsStructure(Paragraph)]
		Section,

		/// <summary>
		/// Represents a section within a section.
		/// </summary>
		[ContainsStructure(SubSubSection)]
		[ContainsStructure(Paragraph)]
		SubSection,

		/// <summary>
		/// Represents a section within a subsection.
		/// </summary>
		[ContainsStructure(Paragraph)]
		SubSubSection,

		/// <summary>
		/// Represents a paragraph, a structure with content but no other
		/// structures.
		/// </summary>
		Paragraph,
	}
}