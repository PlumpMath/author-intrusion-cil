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

using AuthorIntrusion.Contracts;
using AuthorIntrusion.Contracts.Algorithms;
using AuthorIntrusion.Contracts.Structures;

using C5;

using MfGames.GtkExt.TextEditor.Models;
using MfGames.GtkExt.TextEditor.Models.Buffers;

#endregion

namespace AuthorIntrusionGtk.Editors
{
	/// <summary>
	/// Implements an indicator buffer wrapped around an Author Intrusion
	/// document.
	/// </summary>
	public class DocumentLineBuffer : MultiplexedOperationLineBuffer
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="DocumentLineBuffer"/> class.
		/// </summary>
		/// <param name="document">The document.</param>
		public DocumentLineBuffer(Document document)
		{
			// Save the member variables.
			if (document == null)
			{
				throw new ArgumentNullException("document");
			}

			this.document = document;

			// Perform the initial counts on the document.
			RebuildIndexes();
		}

		#endregion

		#region Document

		private Document document;

		/// <summary>
		/// Gets the document associated with this buffer.
		/// </summary>
		/// <value>The document.</value>
		public Document Document
		{
			get { return document; }
		}

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
				var section = (Section) structure;

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

		/// <summary>
		/// Gets the structure info from a given structure.
		/// </summary>
		/// <param name="structure">The structure.</param>
		/// <returns></returns>
		private StructureInfo GetStructureInfo(Structure structure)
		{
			return (StructureInfo) structure.DataDictionary[this];
		}

		/// <summary>
		/// Gets the text from a given structure index from the beginning.
		/// </summary>
		/// <param name="structureIndex">Index of the structure.</param>
		/// <returns></returns>
		public string GetStructureText(int structureIndex)
		{
			// Get the structure element and use that to determine how we
			// retrieve the contents.
			Structure structure = GetStructure(structureIndex);

			return GetStructureText(structure);
		}

		/// <summary>
		/// Gets the structure text from a given structure element.
		/// </summary>
		/// <param name="structure">The structure.</param>
		/// <returns></returns>
		private static string GetStructureText(Structure structure)
		{
			// For paragraphs, we get the the content string.
			if (structure is Paragraph)
			{
				var paragraph = (Paragraph) structure;
				string contents = paragraph.ContentString;

				return contents;
			}

			// For sections, we return the title of the section.
			if (structure is Section)
			{
				var section = (Section) structure;

				return section.Title ?? "<Untitled>";
			}

			// This is an unknown structure, so throw an exception because we
			// shouldn't be getting this.
			throw new Exception(
				"Cannot find structure text of unknown type: " + structure.GetType());
		}

		/// <summary>
		/// Goes through the entire document and rebuilds the internal indexes.
		/// </summary>
		private void RebuildIndexes()
		{
			var visitor = new LineBufferVisitor(this);

			visitor.Visit(document);
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

		/// <summary>
		/// Gets the length of the line.
		/// </summary>
		/// <param name="lineIndex">The line index in the buffer.</param>
		/// <param name="lineContexts">The line contexts.</param>
		/// <returns>The length of the line.</returns>
		public override int GetLineLength(
			int lineIndex,
			LineContexts lineContexts)
		{
			return GetStructureText(lineIndex).Length;
		}

		/// <summary>
		/// Gets the formatted line number for a given line.
		/// </summary>
		/// <param name="lineIndex">The line index in the buffer.</param>
		/// <returns>A formatted line number.</returns>
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
		public override string GetLineStyleName(
			int lineIndex,
			LineContexts lineContexts)
		{
			// Get the structure and its text.
			Structure structure = GetStructure(lineIndex);
			string text = GetStructureText(structure);

			// The default style is the structure type. If the structure is
			// blank, then we prepend "Blank " to the front to allow the user
			// to style those paragraphs differently.
			string styleName = structure.StructureType.ToString();

			if (text.Length == 0)
			{
				styleName = "Blank " + styleName;
			}

			return styleName;
		}

		/// <summary>
		/// Gets the text of a given line in the buffer.
		/// </summary>
		/// <param name="lineIndex">The line index in the buffer. If the index is beyond the end of the buffer, the last line is used.</param>
		/// <param name="characters">The character range to pull the text.</param>
		/// <param name="lineContexts">The line contexts.</param>
		/// <returns></returns>
		public override string GetLineText(
			int lineIndex,
			CharacterRange characters,
			LineContexts lineContexts)
		{
			// Get the text of the structure.
			Structure structure = GetStructure(lineIndex);
			string text = GetStructureText(structure);

			// TODO: Make the placeholder text user configurable.

			// If the text is blank, change the text to a placeholder text.
			if (text.Length == 0)
			{
				text = "<New " + structure.StructureType + ">";
			}

			// Pull out the requested substring and return it.
			int endIndex = characters.EndIndex;

			endIndex = Math.Min(endIndex, text.Length);

			return text.Substring(
				characters.StartIndex, endIndex - characters.StartIndex);
		}

		#endregion

		#region Operations

		/// <summary>
		/// Inserts text into the buffer.
		/// </summary>
		/// <param name="operation">The operation to perform.</param>
		/// <returns>
		/// The results to the changes to the buffer.
		/// </returns>
		protected override LineBufferOperationResults Do(
			InsertTextOperation operation)
		{
			// Get the structure and its text for a given line.
			int structureIndex = operation.BufferPosition.LineIndex;
			Structure structure = GetStructure(structureIndex);
			string text = GetStructureText(structure);

			// Figure out what the line would look like with the text inserted.
			int characterIndex = Math.Min(
				operation.BufferPosition.CharacterIndex, text.Length);

			string newText = text.Insert(characterIndex, operation.Text);

			// Set the structure text which enables the internal parsing.
			structure.SetText(newText);

			// Fire a line changed operation.
			RaiseLineChanged(new LineChangedArgs(structureIndex));

			// Return the appropriate results.
			return
				new LineBufferOperationResults(
					new BufferPosition(structureIndex, characterIndex + operation.Text.Length));
		}

		/// <summary>
		/// Deletes text from the buffer.
		/// </summary>
		/// <param name="operation">The operation to perform.</param>
		/// <returns>
		/// The results to the changes to the buffer.
		/// </returns>
		protected override LineBufferOperationResults Do(
			DeleteTextOperation operation)
		{
			// Get the structure and its text for a given line.
			int structureIndex = operation.LineIndex;
			Structure structure = GetStructure(structureIndex);
			string text = GetStructureText(structure);

			// Figure out what the line would look like with the text inserted.
			int endCharacterIndex = Math.Min(
				operation.CharacterRange.EndIndex, text.Length);

			string newText = text.Remove(
				operation.CharacterRange.StartIndex,
				endCharacterIndex - operation.CharacterRange.StartIndex);

			// Set the structure text which enables the internal parsing.
			structure.SetText(newText);

			// Fire a line changed operation.
			RaiseLineChanged(new LineChangedArgs(structureIndex));

			// Return the appropriate results.
			return
				new LineBufferOperationResults(
					new BufferPosition(structureIndex, operation.CharacterRange.StartIndex));
		}

		/// <summary>
		/// Performs the set text operation on the buffer.
		/// </summary>
		/// <param name="operation">The operation to perform.</param>
		/// <returns>
		/// The results to the changes to the buffer.
		/// </returns>
		protected override LineBufferOperationResults Do(SetTextOperation operation)
		{
			// Get the structure for a given line.
			int structureIndex = operation.LineIndex;
			Structure structure = GetStructure(structureIndex);

			// Set the text of the line.
			structure.SetText(operation.Text);

			// Fire a line changed operation.
			RaiseLineChanged(new LineChangedArgs(operation.LineIndex));

			// Return the appropriate results.
			return
				new LineBufferOperationResults(
					new BufferPosition(operation.LineIndex, operation.Text.Length));
		}

		/// <summary>
		/// Performs the insert lines operation on the buffer.
		/// </summary>
		/// <param name="operation">The operation to perform.</param>
		/// <returns>
		/// The results to the changes to the buffer.
		/// </returns>
		protected override LineBufferOperationResults Do(
			InsertLinesOperation operation)
		{
			// We don't allow duplication of the top-level element.
			int structureIndex = operation.LineIndex;

			if (structureIndex == 0)
			{
				return new LineBufferOperationResults();
			}

			// When we insert a line, we figure out what the element is at the
			// point of inserting and create an identical element.
			Structure structure = GetStructure(structureIndex);

			// Figure out the parent structure and the index of this structure
			// inside that parent.
			var parent = structure.ParentSection;
			int parentIndex = parent.IndexOf(structure);

			// Now, insert an empty version of the structure after the
			// line index.
			for (int i = 0; i < operation.Count; i++)
			{
				Structure newStructure = structure.CreateEmptyClone();
				parent.Structures.Insert(parentIndex, newStructure);
			}

			// Once we are done, we have to rebuild the indexes.
			RebuildIndexes();

			// Fire an insert line change.
			RaiseLinesInserted(
				new LineRangeEventArgs(operation.LineIndex, operation.Count));

			// Return the appropriate results.
			return
				new LineBufferOperationResults(
					new BufferPosition(operation.LineIndex + operation.Count, 0));
		}

		/// <summary>
		/// Performs the delete lines operation on the buffer.
		/// </summary>
		/// <param name="operation">The operation to perform.</param>
		/// <returns>
		/// The results to the changes to the buffer.
		/// </returns>
		protected override LineBufferOperationResults Do(
			DeleteLinesOperation operation)
		{
			// All of these operations return the same results, so create it
			// once so we can easily return out of this method.
			int startIndex = operation.LineIndex;
			int endIndex = startIndex + operation.Count - 1;

			// Delete the lines.
			InternalDeleteLines(startIndex, endIndex);

			// Return the results, which is the same for all deletes.
			var bufferPosition = new BufferPosition(startIndex, 0);
			var results = new LineBufferOperationResults(bufferPosition);

			return results;
		}

		private void InternalDeleteLines(
			int startIndex,
			int endIndex)
		{
			// The delete operations are based on linear indexes, so we have to
			// unwrap our document structure around the request. In addition,
			// the delete process needs to maintain structure so a chapter
			// doesn't have a subsection below it.

			// For debugging purposes, create a dumper and show the desired
			// output and processing of the delete.
			var dumper = new DocumentDumper(document, Console.Out);
			dumper.StructurePrefixes.Clear();
			dumper.StructurePrefixes[startIndex] = '+';
			dumper.StructurePrefixes[endIndex] = '-';
			dumper.Dump();

			// If we are from the same parent, then we need to remove the items
			// while adding the trailing items from the end index.
			Structure start = GetStructure(startIndex);
			var startParent = start.ParentSection;

			Structure end = GetStructure(endIndex);
			var endSection = end as Section;
			var endParent = end.ParentSection;

			if (startParent == endParent)
			{
				// If we are the top-level, we have a problem.
				if (start.Depth == 0)
				{
					throw new Exception("Cannot remove top-level item.");
				}

				// Single item removal. If this is a container, we need to add
				// all the items inside the container.
				if (endSection != null)
				{
					// Because of integrity, we need to pull the trailing items
					// out of the end before putting them into the startParent's
					// structures.
					var structures = new ArrayList<Structure>();
					structures.AddAll(endSection.Structures);

					endSection.Structures.RemoveAll(structures);

					startParent.Structures.InsertAll(
						end.ParentIndex + 1,
						structures);
				}

				// Remove the item.
				startParent.Structures.RemoveInterval(
					start.ParentIndex,
					end.ParentIndex - start.ParentIndex + 1);

				// We finished with these cases, so return to stop processing.
				RebuildIndexes();
			}

			// Check to see if the start is the first line.
			if (startIndex == 0)
			{
				// Because the first line is top-most structure, we have to do
				// something special for this one. First, delete all the other
				// lines so the second item will be the new document root.
				InternalDeleteLines(startIndex + 1, endIndex);

				// Get the new root which will always be the first child item.
				Structure newRoot = GetStructure(1);

				// If the new root isn't a section, we need to create a new
				// section and 
				Section newSection;

				if (!(newRoot is Section))
				{
					// Create a new section and set the section's title equal
					// to the text of the section we're getting rid of.
					newSection = new Section();
					newSection.SetText(newRoot.GetText());
				}
				else
				{
					newSection = (Section) newRoot;
				}

				// Transplant the contents of the old root into the new section.
				var rootSection = (Section) document.Structure;

				newSection.Structures.AddAll(rootSection.Structures.Slide(2));

				document.Structure = newRoot;

				// Rebuild the indexes with the new document.
				RebuildIndexes();
				return;
			}

			// Flatten out the structure between the start and end point.
			Console.WriteLine("=== flattened");
			FlattenDocument(startIndex, endIndex);
			dumper.Dump();

			// At this point, the end structure will be a child of the start's
			// parent. We want to pull everything up after the end point up
			// until we are at the same level as the start.
			PullEndToStartLevel(startIndex, endIndex);
			Console.WriteLine("=== Pull end to start");
			dumper.Dump();

			// We have the start on the same level as the end, plus there are
			// no elements between the two that have a lower depths than either.
			// We need to see if the end point is a section.
			if (endSection != null)
			{
				startParent.Structures.InsertAll(
					end.ParentIndex + 1,
					endSection.Structures);
			}

			// Remove all the items between the start and end points.
			startParent.Structures.RemoveInterval(
				start.ParentIndex,
				end.ParentIndex - start.ParentIndex + 1);
			RebuildIndexes();
			Console.WriteLine("=== Removing items");
			dumper.Dump();
		}

		#endregion

		#region Structure Manipulations

		/// <summary>
		/// Flattened out the document between the start and end structure.
		/// This pulls down the structure elements into a flattened layer.
		/// </summary>
		/// <param name="startIndex">The start index.</param>
		/// <param name="endIndex">The end index.</param>
		private void FlattenDocument(
			int startIndex,
			int endIndex)
		{
			// Loop through the document from the start index to the end, making
			// sure everything has at least the depth of the start item.
			Structure start = GetStructure(startIndex);
			int startDepth = start.Depth;
			int startParentIndex = start.ParentIndex;
			Section startParent = start.ParentSection;

			for (int index = startIndex + 1; index <= endIndex; index++)
			{
				// Check the depth and see if we need to flattened this.
				Structure structure = GetStructure(index);

				if (structure.Depth < startDepth)
				{
					// We need to move this down, even if it violates the normal
					// section layouts. We'll fix those later.
					structure.ParentSection.Structures.Remove(structure);
					startParent.Structures.Add(structure);

					// We have to rebalance everything.
					RebuildIndexes();
					var dumper = new DocumentDumper(document, Console.Out);
					dumper.StructurePrefixes.Clear();
					dumper.StructurePrefixes[startIndex] = '+';
					dumper.StructurePrefixes[endIndex] = '-';
					dumper.Dump();
					FlattenDocument(startIndex, endIndex);
					return;
				}
			}
		}

		/// <summary>
		/// Pulls the end structure up to the same level as the start index.
		/// </summary>
		/// <param name="startIndex"></param>
		/// <param name="endIndex"></param>
		private void PullEndToStartLevel(
			int startIndex,
			int endIndex)
		{
			// Get the structures for the two points.
			Structure start = GetStructure(startIndex);
			Structure end = GetStructure(endIndex);

			// If we are at the same level, we're done.
			if (start.Parent == end.Parent)
			{
				return;
			}

			// Since the end is lower than the start, we shift the end plus any
			// items after it from its parent up a level.
			Section endParent = end.ParentSection;

			System.Collections.Generic.IList<Structure> trailingStructures = endParent.Structures.View(end.ParentIndex, endParent.Structures.Count - end.ParentIndex);
			endParent.Structures.RemoveInterval(
				end.ParentIndex,
				endParent.Structures.Count - end.ParentIndex);

			Section superParent = endParent.ParentSection;

			superParent.Structures.InsertAll(
				endParent.ParentIndex + 1,
				trailingStructures);

			// Rebuild the indexes.
			RebuildIndexes();

			PullEndToStartLevel(startIndex, endIndex);
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