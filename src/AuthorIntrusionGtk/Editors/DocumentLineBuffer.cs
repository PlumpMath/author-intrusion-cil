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
using System.Text.RegularExpressions;

using AuthorIntrusion.Contracts;
using AuthorIntrusion.Contracts.Algorithms;
using AuthorIntrusion.Contracts.Structures;

using MfGames.GtkExt.LineTextEditor.Buffers;
using MfGames.GtkExt.LineTextEditor.Interfaces;

#endregion

namespace AuthorIntrusionGtk.Editors
{
	/// <summary>
	/// Implements an indicator buffer wrapped around an Author Intrusion
	/// document.
	/// </summary>
	public class DocumentLineBuffer : LineBuffer
	{
		#region Constructors

		public DocumentLineBuffer(Document document)
		{
			// Save the member variables.
			if (document == null)
			{
				throw new ArgumentNullException("document");
			}

			this.document = document;

			// Perform the initial counts on the document.
			LineBufferVisitor visitor = new LineBufferVisitor(this);

			visitor.Visit(document);
		}

		#endregion

		#region Document

		private Document document;

		private Structure GetStructure(int structureIndex)
		{
			// Go through the structure tree until we find the correct one.
			return GetStructure(document.Structure, structureIndex);
		}

		private Structure GetStructure(
			Structure structure,
			int structureIndex)
		{
			// If the index is zero, then we mean this structure.
			if (structureIndex == 0)
			{
				return structure;
			}

			// Decrement the count and cycle through the children.
			structureIndex--;

			if (structure is Section)
			{
				Section section = (Section) structure;

				foreach (Structure childStructure in section.Structures)
				{
					// Pull out the structure info and use that to determine
					// if the index is inside this child structure.
					StructureInfo structureInfo =
						GetStructureInfo(childStructure);

					if (structureIndex < structureInfo.StructureCount)
					{
						// This is the child structure we need to recurse into.
						return GetStructure(childStructure, structureIndex);
					}

					// This child isn't it, but we need to decrement the count
					// from this child to keep the relative index.
					structureIndex -= structureInfo.StructureCount;
				}
			}

			// We can't find it, so throw an exception.
			throw new ArgumentOutOfRangeException(
				"structureIndex",
				"Cannot find the structure from the given index.");
		}

		private StructureInfo GetStructureInfo(Structure structure)
		{
			return (StructureInfo) structure.DataDictionary[this];
		}

		private string GetStructureText(int structureIndex)
		{
			Structure structure = GetStructure(structureIndex);

			if (structure is Paragraph)
			{
				var paragraph = (Paragraph) structure;
				string contents = paragraph.ContentString.Trim();

				return Regex.Replace(contents, "\\s+", " ");
			}

			if (structure is Section)
			{
				var section = (Section) structure;

				return section.Title ?? "<Untitled>";
			}

			return "UNKNOWN";
		}

		#endregion

		#region Buffer

		/// <summary>
		/// Gets the number of lines in the buffer.
		/// </summary>
		/// <value>The line count.</value>
		public override int LineCount
		{
			get { return GetStructureInfo(document.Structure).StructureCount; }
		}

		/// <summary>
		/// If set to <see langword="true"/>, the buffer is read-only and the editing
		/// commands should throw an <see cref="InvalidOperationException"/>.
		/// </summary>
		public override bool ReadOnly
		{
			get { return false; }
		}

		#endregion

		#region Lines

		public override int GetLineLength(int lineIndex)
		{
			return GetStructureText(lineIndex).Length;
		}

		public override string GetLineNumber(int lineIndex)
		{
			return String.Empty;
		}

		/// <summary>
		/// Returns the style based on the structure found at the given line
		/// index.
		/// </summary>
		/// <param name="lineIndex">The line index in the buffer or Int32.MaxValue for
		/// the last line.</param>
		/// <returns></returns>
		public override string GetLineStyleName(int lineIndex)
		{
			Structure structure = GetStructure(lineIndex);

			return structure.StructureType.ToString();
		}

		public override string GetLineText(
			int lineIndex,
			int startIndex,
			int endIndex)
		{
			string text = GetStructureText(lineIndex);

			endIndex = Math.Min(endIndex, text.Length);

			return text.Substring(startIndex, endIndex - startIndex);
		}

		#endregion

		#region Operations

		public override void Do(ILineBufferOperation operation)
		{
		}

		#endregion

		#region Nested type: LineBufferVisitor

		private class LineBufferVisitor : DocumentVisitor
		{
			private DocumentLineBuffer buffer;

			public LineBufferVisitor(DocumentLineBuffer buffer)
			{
				this.buffer = buffer;
			}

			protected override bool OnBeginStructure(Structure structure)
			{
				// Create a structure info and assign it to the structure. Every
				// structure has at least one item, which will be the title for
				// sections and the paragraph itself.
				var structureInfo = new StructureInfo();

				structureInfo.StructureCount = 1;

				structure.DataDictionary[buffer] = structureInfo;

				// Recurse into the inner structures.
				return true;
			}

			protected override void OnEndSection(Section section)
			{
				// For sections, add to the structure info.
				StructureInfo structureInfo = (StructureInfo) section.DataDictionary[buffer];

				foreach (Structure structure in section.Structures)
				{
					StructureInfo innerInfo = (StructureInfo) structure.DataDictionary[buffer];

					structureInfo.StructureCount += innerInfo.StructureCount;
				}
			}
		}

		#endregion

		#region Nested type: StructureInfo

		/// <summary>
		/// Contains information about a structure that is used by the parent
		/// class to provide LineBuffer implementation.
		/// </summary>
		private class StructureInfo
		{
			public int StructureCount { get; set; }
		}

		#endregion
	}
}