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

using AuthorIntrusion.Contracts.Structures;

#endregion

namespace AuthorIntrusion.Contracts.Algorithms
{
	/// <summary>
	/// Implements a visitor that recurses through structures and content and
	/// allows extending classes to perform some action on the various elements.
	/// </summary>
	public class DocumentVisitor
	{
		#region Visiting

		/// <summary>
		/// Visits the various structural nodes of a document.
		/// </summary>
		public void Visit(Document document)
		{
			// Check for null values.
			if (document == null)
			{
				throw new ArgumentNullException("document");
			}

			// Start by calling the BeingDocument call.
			bool shouldRecurse = OnBeginDocument(document);

			// Loop through the structure of the document.
			if (shouldRecurse)
			{
				Visit(document.Structure);
			}

			// Finish up with the end document.
			OnEndDocument(document);
		}

		/// <summary>
		/// Visits the specified structure, expanding out for sections and
		/// paragraphs.
		/// </summary>
		/// <param name="structure">The structure.</param>
		protected void Visit(Structure structure)
		{
			// We always call the start structure to determine if we
			// should recurse into the structure.
			bool shouldRecurse = OnBeginStructure(structure);

			// Determine the type of structure we should work with.
			if (structure is Section)
			{
				Section section = (Section) structure;

				Visit(section, shouldRecurse);
			}
			else if (structure is Paragraph)
			{
				Paragraph paragraph = (Paragraph) structure;

				Visit(paragraph, shouldRecurse);
			}

			OnEndStructure(structure);
		}

		/// <summary>
		/// Visits the specified section with optional recursion.
		/// </summary>
		/// <param name="section">The section.</param>
		/// <param name="shouldRecurse">if set to <c>true</c> [should recurse].</param>
		private void Visit(
			Section section,
			bool shouldRecurse)
		{
			shouldRecurse |= OnBeginSection(section);

			if (shouldRecurse)
			{
				foreach (var structure in section.Structures)
				{
					Visit(structure);
				}
			}

			OnEndSection(section);
		}

		/// <summary>
		/// Visits the specified paragraph with option recursion into the content.
		/// </summary>
		/// <param name="paragraph">The paragraph.</param>
		/// <param name="shouldRecurse">if set to <c>true</c> [should recurse].</param>
		private void Visit(
			Paragraph paragraph,
			bool shouldRecurse)
		{
			OnBeginParagraph(paragraph);

			OnEndParagraph(paragraph);
		}

		#endregion

		#region Events

		/// <summary>
		/// Called when the visitor enters a document.
		/// </summary>
		/// <param name="document">The document.</param>
		/// <returns>True if the visitor should continue to recurse.</returns>
		protected virtual bool OnBeginDocument(Document document)
		{
			return true;
		}

		/// <summary>
		/// Called when the visitor enters a paragraph.
		/// </summary>
		/// <param name="paragraph">The paragraph.</param>
		/// <returns>True if the visitor should continue to recurse.</returns>
		protected virtual bool OnBeginParagraph(Paragraph paragraph)
		{
			return true;
		}

		/// <summary>
		/// Called when the visitor enters a section.
		/// </summary>
		/// <param name="section">The section.</param>
		/// <returns>True if the visitor should continue to recurse.</returns>
		protected virtual bool OnBeginSection(Section section)
		{
			return true;
		}

		/// <summary>
		/// Called when the visitor enters a structure. This is always called
		/// before <see cref="OnBeginSection"/> and <see cref="OnBeginParagraph"/>.
		/// </summary>
		/// <param name="structure">The structure.</param>
		/// <returns>True if the visitor should continue to recurse.</returns>
		protected virtual bool OnBeginStructure(Structure structure)
		{
			return true;
		}

		/// <summary>
		/// Called when the visitor leaves a document.
		/// </summary>
		/// <param name="document">The document.</param>
		protected virtual void OnEndDocument(Document document)
		{
		}

		/// <summary>
		/// Called when the visitor leaves a paragraph. This is called before
		/// <see cref="OnEndStructure"/>.
		/// </summary>
		/// <param name="paragraph">The paragraph.</param>
		protected virtual void OnEndParagraph(Paragraph paragraph)
		{
		}

		/// <summary>
		/// Called when the visitor leaves a section. This is called before
		/// <see cref="OnEndStructure"/>.
		/// </summary>
		/// <param name="section">The section.</param>
		protected virtual void OnEndSection(Section section)
		{
		}

		/// <summary>
		/// Called when the visitor leaves a structure. This is called after
		/// <see cref="OnEndSection"/> and <see cref="OnEndParagraph"/>.
		/// </summary>
		/// <param name="structure">The structure.</param>
		protected virtual void OnEndStructure(Structure structure)
		{
		}

		#endregion
	}
}