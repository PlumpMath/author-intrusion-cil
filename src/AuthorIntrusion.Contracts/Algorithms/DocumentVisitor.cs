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

using AuthorIntrusion.Contracts.Matters;
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
				Visit(document.Matters);
			}

			// Finish up with the end document.
			OnEndDocument(document);
		}

		/// <summary>
		/// Goes through the given list, processing the matter and contents
		/// in a recursive manner.
		/// </summary>
		/// <param name="list">The list.</param>
		private void Visit(DocumentMatterList list)
		{
			// Start at the top of the list and go through it. We don't provide
			for (int index = 0; index < list.Count; index++)
			{
				// Visit each item in turn.
				Visit(list[index], ref index);
			}
		}

		/// <summary>
		/// Visits a single matter item at the given index. The processing
		/// of the matter may result in the index being incremented.
		/// </summary>
		/// <param name="matter">The matter.</param>
		/// <param name="index">The index.</param>
		private void Visit(Matter matter, ref int index)
		{
			// Start with the begin which determines if we continue.
			bool shouldRecurse = OnBeginMatter(matter);

			if (shouldRecurse)
			{
				// Figure out what to do based on the type.
				switch (matter.MatterType)
				{
					case MatterType.Paragraph:
						Visit((Paragraph) matter, ref index);
						break;

					case MatterType.Region:
						Visit((Region) matter, ref index);
						break;

					default:
						throw new Exception("Unknown matter type: " + matter.MatterType);
				}
			}

			// Finish up the matter.
			OnEndMatter(matter);
		}

		/// <summary>
		/// Visits the specified paragraph.
		/// </summary>
		/// <param name="paragraph">The paragraph.</param>
		/// <param name="index">The index.</param>
		private void Visit(Paragraph paragraph, ref int index)
		{
			OnBeginParagraph(paragraph);
			index++;
			OnEndParagraph(paragraph);
		}

		/// <summary>
		/// Visits the specified region and increments it by its length.
		/// </summary>
		/// <param name="region">The region.</param>
		/// <param name="index">The index.</param>
		private void Visit(Region region, ref int index)
		{
			// Increment the index for the title of the region.
			index++;

			// Start processing the region and determine if we should recurse.
			bool shouldRecurse = OnBeginRegion(region);

			if (shouldRecurse)
			{
				// Loop through the region's matters and allow the index to
				// be incremented for those items.
				foreach (Matter matter in region.Matters)
				{
					Visit(matter, ref index);
				}
			}

			// Finish up the region and return.
			OnEndRegion(region);
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
		/// Called when the visitor enters a region.
		/// </summary>
		/// <param name="section">The section.</param>
		/// <returns>True if the visitor should continue to recurse.</returns>
		protected virtual bool OnBeginRegion(Region section)
		{
			return true;
		}

		/// <summary>
		/// Called when the visitor enters a structure. This is always called
		/// before <see cref="OnBeginRegion"/> and <see cref="OnBeginParagraph"/>.
		/// </summary>
		/// <param name="structure">The structure.</param>
		/// <returns>True if the visitor should continue to recurse.</returns>
		protected virtual bool OnBeginMatter(Matter structure)
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
		/// <see cref="OnEndMatter"/>.
		/// </summary>
		/// <param name="paragraph">The paragraph.</param>
		protected virtual void OnEndParagraph(Paragraph paragraph)
		{
		}

		/// <summary>
		/// Called when the visitor leaves a section. This is called before
		/// <see cref="OnEndRegion"/>.
		/// </summary>
		/// <param name="section">The section.</param>
		protected virtual void OnEndRegion(Region section)
		{
		}

		/// <summary>
		/// Called when the visitor leaves a structure. This is called after
		/// <see cref="OnEndRegion"/> and <see cref="OnEndParagraph"/>.
		/// </summary>
		/// <param name="structure">The structure.</param>
		protected virtual void OnEndMatter(Matter structure)
		{
		}

		#endregion
	}
}