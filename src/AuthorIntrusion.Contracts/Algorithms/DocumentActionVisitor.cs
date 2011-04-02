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

using AuthorIntrusion.Contracts.Matters;
using AuthorIntrusion.Contracts.Structures;

#endregion

namespace AuthorIntrusion.Contracts.Algorithms
{
	/// <summary>
	/// Extends the <see cref="DocumentVisitor"/> to allow for a 
	/// <see cref="Func{T,TResult}"/> or <see cref="Action{T}"/> to be called at
	/// the various points.
	/// </summary>
	public class DocumentActionVisitor : DocumentVisitor
	{
		#region Actions

		/// <summary>
		/// Gets or sets the function to be called during 
		/// <see cref="OnBeginDocument"/>.
		/// </summary>
		public Func<Document, bool> BeginDocument { get; set; }

		/// <summary>
		/// Gets or sets the function to be called during 
		/// <see cref="OnBeginMatter"/>.
		/// </summary>
		public Func<Matter, bool> BeginMatter { get; set; }

		/// <summary>
		/// Gets or sets the function to be called during 
		/// <see cref="OnBeginParagraph"/>.
		/// </summary>
		public Func<Paragraph, bool> BeginParagraph { get; set; }

		/// <summary>
		/// Gets or sets the function to be called during 
		/// <see cref="OnBeginRegion"/>.
		/// </summary>
		public Func<Region, bool> BeginRegion { get; set; }

		/// <summary>
		/// Gets or sets the action to be called during <see cref="OnEndDocument"/>.
		/// </summary>
		public Action<Document> EndDocument { get; set; }

		/// <summary>
		/// Gets or sets the action to be called during <see cref="OnEndMatter"/>.
		/// </summary>
		public Action<Matter> EndMatter { get; set; }

		/// <summary>
		/// Gets or sets the action to be called during <see cref="OnEndParagraph"/>.
		/// </summary>
		public Action<Paragraph> EndParagraph { get; set; }

		/// <summary>
		/// Gets or sets the action to be called during <see cref="OnEndRegion"/>.
		/// </summary>
		public Action<Region> EndRegion { get; set; }

		#endregion

		#region Events

		/// <summary>
		/// Called when the visitor enters a document.
		/// </summary>
		/// <param name="document">The document.</param>
		/// <returns>
		/// True if the visitor should continue to recurse.
		/// </returns>
		protected override bool OnBeginDocument(Document document)
		{
			if (BeginDocument != null)
			{
				return BeginDocument(document);
			}

			return base.OnBeginDocument(document);
		}

		/// <summary>
		/// Called when the visitor enters a structure. This is always called
		/// before <see cref="OnBeginRegion"/> and <see cref="OnBeginParagraph"/>.
		/// </summary>
		/// <param name="structure">The structure.</param>
		/// <returns>
		/// True if the visitor should continue to recurse.
		/// </returns>
		protected override bool OnBeginMatter(Matter structure)
		{
			if (BeginMatter != null)
			{
				return BeginMatter(structure);
			}

			return base.OnBeginMatter(structure);
		}

		/// <summary>
		/// Called when the visitor enters a paragraph.
		/// </summary>
		/// <param name="paragraph">The paragraph.</param>
		/// <returns>
		/// True if the visitor should continue to recurse.
		/// </returns>
		protected override bool OnBeginParagraph(Paragraph paragraph)
		{
			if (BeginParagraph != null)
			{
				return BeginParagraph(paragraph);
			}

			return base.OnBeginParagraph(paragraph);
		}

		/// <summary>
		/// Called when the visitor enters a section.
		/// </summary>
		/// <param name="section">The section.</param>
		/// <returns>
		/// True if the visitor should continue to recurse.
		/// </returns>
		protected override bool OnBeginRegion(Region section)
		{
			if (BeginRegion != null)
			{
				return BeginRegion(section);
			}

			return base.OnBeginRegion(section);
		}

		/// <summary>
		/// Called when the visitor leaves a document.
		/// </summary>
		/// <param name="document">The document.</param>
		protected override void OnEndDocument(Document document)
		{
			if (EndDocument != null)
			{
				EndDocument(document);
			}
		}

		/// <summary>
		/// Called when the visitor leaves a structure. This is called after
		/// <see cref="OnEndRegion"/> and <see cref="OnEndParagraph"/>.
		/// </summary>
		/// <param name="structure">The structure.</param>
		protected override void OnEndMatter(Matter structure)
		{
			if (EndMatter != null)
			{
				EndMatter(structure);
			}
		}

		/// <summary>
		/// Called when the visitor leaves a paragraph. This is called before
		/// <see cref="OnEndMatter"/>.
		/// </summary>
		/// <param name="paragraph">The paragraph.</param>
		protected override void OnEndParagraph(Paragraph paragraph)
		{
			if (EndParagraph != null)
			{
				EndParagraph(paragraph);
			}
		}

		/// <summary>
		/// Called when the visitor leaves a section. This is called before
		/// <see cref="OnEndRegion"/>.
		/// </summary>
		/// <param name="section">The section.</param>
		protected override void OnEndRegion(Region section)
		{
			if (EndRegion != null)
			{
				EndRegion(section);
			}
		}

		#endregion
	}
}