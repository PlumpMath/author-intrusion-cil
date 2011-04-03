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
using AuthorIntrusion.Contracts.Matters;
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

		#endregion

		#region Buffer

		/// <summary>
		/// Gets the number of lines in the buffer.
		/// </summary>
		/// <value>The line count.</value>
		public override int LineCount
		{
			get { return document.DocumentMatters.Count; }
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
			return document.DocumentMatters[lineIndex].GetContents().Length;
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
		/// <param name="lineContexts">The line contexts.</param>
		/// <returns></returns>
		public override string GetLineStyleName(
			int lineIndex,
			LineContexts lineContexts)
		{
			// Get the structure and its text.
			Matter structure = document.DocumentMatters[lineIndex];
			string text = structure.GetContents();

			// The default style is the structure type. If the structure is
			// blank, then we prepend "Blank " to the front to allow the user
			// to style those paragraphs differently.
			string styleName = structure.MatterType.ToString();

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
			Matter matter = document.DocumentMatters[lineIndex];
			string text = matter.GetContents();

			// TODO: Make the placeholder text user configurable.

			// If the text is blank, change the text to a placeholder text.
			if (text.Length == 0)
			{
				text = "<New " + matter.MatterType + ">";
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
			int lineIndex = operation.BufferPosition.LineIndex;
			Matter matter = document.DocumentMatters[lineIndex];
			string text = matter.GetContents();

			// Figure out what the line would look like with the text inserted.
			int characterIndex = Math.Min(
				operation.BufferPosition.CharacterIndex, text.Length);

			string newText = text.Insert(characterIndex, operation.Text);

			// Set the structure text which enables the internal parsing.
			matter.SetContents(newText);

			// Fire a line changed operation.
			RaiseLineChanged(new LineChangedArgs(lineIndex));

			// Return the appropriate results.
			return
				new LineBufferOperationResults(
					new BufferPosition(lineIndex, characterIndex + operation.Text.Length));
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
			int lineIndex = operation.LineIndex;
			Matter matter = document.DocumentMatters[lineIndex];
			string text = matter.GetContents();

			// Figure out what the line would look like with the text inserted.
			int endCharacterIndex = Math.Min(
				operation.CharacterRange.EndIndex, text.Length);

			string newText = text.Remove(
				operation.CharacterRange.StartIndex,
				endCharacterIndex - operation.CharacterRange.StartIndex);

			// Set the structure text which enables the internal parsing.
			matter.SetContents(newText);

			// Fire a line changed operation.
			RaiseLineChanged(new LineChangedArgs(lineIndex));

			// Return the appropriate results.
			return
				new LineBufferOperationResults(
					new BufferPosition(lineIndex, operation.CharacterRange.StartIndex));
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
			int lineIndex = operation.LineIndex;
			Matter matter = document.DocumentMatters[lineIndex];

			// Set the text of the line.
			matter.SetContents(operation.Text);

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
			int lineIndex = operation.LineIndex;

			if (lineIndex == 0)
			{
				return new LineBufferOperationResults();
			}

			// When we insert a line, we figure out what the element is at the
			// point of inserting and create an identical element.
			Matter matter = document.DocumentMatters[lineIndex];

			// Figure out the parent structure and the index of this structure
			// inside that parent.
			var parent = matter.ParentSection;
			int parentIndex = parent.IndexOf(matter);

			// Now, insert an empty version of the structure after the
			// line index.
			for (int i = 0; i < operation.Count; i++)
			{
				Matter newStructure = matter.CreateEmptyClone();
				parent.Matters.Insert(parentIndex, newStructure);
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
			Matter start = document.DocumentMatters[startIndex];
			var startParent = start.ParentSection;

			Matter end = document.DocumentMatters[endIndex];
			var endSection = end as Region;
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
					var structures = new ArrayList<Matter>();
					structures.AddAll(
						endSection.Matters.;

					endSection.Matters.RemoveAll(structures);

					startParent.Matters.InsertAll(
						end.ParentIndex + 1,
						structures);
				}

				// Remove the item.
				startParent.Matters.RemoveInterval(
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
				Matter newRoot = GetStructure(1);

				// If the new root isn't a section, we need to create a new
				// section and 
				Region newSection;

				if (!(newRoot is Region))
				{
					// Create a new section and set the section's title equal
					// to the text of the section we're getting rid of.
					newSection = new Region();
					newSection.SetText(newRoot.GetText());
				}
				else
				{
					newSection = (Region) newRoot;
				}

				// Transplant the contents of the old root into the new section.
				var rootSection = (Region) document.Structure;

				newSection.Matters.AddAll(rootSection.Matters.Slide(2));

				document.Structure = newRoot;

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
				startParent.Matters.InsertAll(
					end.ParentIndex + 1,
					endSection.Matters.;
			}

			// Remove all the items between the start and end points.
			startParent.Matters.RemoveInterval(
				start.ParentIndex,
				end.ParentIndex - start.ParentIndex + 1);
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
			Matter start = document.DocumentMatters[startIndex];
			int startDepth = start.Depth;
			int startParentIndex = start.ParentIndex;
			Region startParent = start.ParentSection;

			for (int index = startIndex + 1; index <= endIndex; index++)
			{
				// Check the depth and see if we need to flattened this.
				Matter structure = document.DocumentMatters[index];

				if (structure.Depth < startDepth)
				{
					// We need to move this down, even if it violates the normal
					// section layouts. We'll fix those later.
					structure.ParentSection.Matters.Remove(structure);
					startParent.Matters.Add(structure);

					// We have to rebalance everything.
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
			Matter start = document.DocumentMatters[startIndex];
			Matter end = document.DocumentMatters[endIndex];

			// If we are at the same level, we're done.
			if (start.Parent == end.Parent)
			{
				return;
			}

			// Since the end is lower than the start, we shift the end plus any
			// items after it from its parent up a level.
			Region endParent = end.ParentSection;

			System.Collections.Generic.IList < Matter > trailin.Matters. =
				endParent.Matters.View(
					end.ParentIndex, endParent.Matters.Count - end.ParentIndex);
			endParent.Matters.RemoveInterval(
				end.ParentIndex,
				endParent.Matters.Count - end.ParentIndex);

			Region superParent = endParent.ParentSection;

			superParent.Matters.InsertAll(
				endParent.ParentIndex + 1,
				trailin.Matters.;

			PullEndToStartLevel(startIndex, endIndex);
		}

		#endregion
	}
}