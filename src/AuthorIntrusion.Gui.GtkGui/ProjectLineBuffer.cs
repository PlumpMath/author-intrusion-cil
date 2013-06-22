// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using AuthorIntrusion.Common;
using AuthorIntrusion.Common.Actions;
using AuthorIntrusion.Common.Blocks;
using AuthorIntrusion.Common.Blocks.Locking;
using AuthorIntrusion.Common.Commands;
using C5;
using Gtk;
using MfGames.Commands;
using MfGames.GtkExt;
using MfGames.GtkExt.TextEditor;
using MfGames.GtkExt.TextEditor.Events;
using MfGames.GtkExt.TextEditor.Models;
using MfGames.GtkExt.TextEditor.Models.Buffers;

namespace AuthorIntrusion.Gui.GtkGui
{
	/// <summary>
	/// Encapsulates an adapter class between the text editor's line buffer and
	/// the project information.
	/// </summary>
	public class ProjectLineBuffer: MultiplexedOperationLineBuffer
	{
		#region Properties

		public override int LineCount
		{
			get
			{
				using (blocks.AcquireLock(RequestLock.Read))
				{
					int results = blocks.Count;
					return results;
				}
			}
		}

		public override bool ReadOnly
		{
			get { return false; }
		}

		#endregion

		#region Methods

		public override LineBufferOperationResults DeleteLines(
			int lineIndex,
			int count)
		{
			using (project.Blocks.AcquireLock(RequestLock.Write))
			{
				Block block = project.Blocks[lineIndex];
				var command = new DeleteBlockCommand(block.BlockKey);
				var context = new BlockCommandContext(project);
				project.Commands.Do(command, context);

				return GetOperationResults();
			}
		}

		public override IEnumerable<ILineIndicator> GetLineIndicators(
			int lineIndex,
			int startCharacterIndex,
			int endCharacterIndex)
		{
			// Create a list of indicators.
			var indicators = new ArrayList<ILineIndicator>();

			// Grab the line and use the block type to figure out the indicator.
			Block block;

			using (blocks.AcquireBlockLock(RequestLock.Read, lineIndex, out block))
			{
				string blockTypeName = block.BlockType.Name;

				switch (blockTypeName)
				{
					case "Chapter":
						ILineIndicator indicator = new ProjectLineIndicator(blockTypeName);
						indicators.Add(indicator);
						break;
				}
			}

			// Return the resulting indicators.
			return indicators;
		}

		public override int GetLineLength(
			int lineIndex,
			LineContexts lineContexts)
		{
			string line = GetLineText(lineIndex, lineContexts);
			return line.Length;
		}

		public override string GetLineMarkup(
			int lineIndex,
			LineContexts lineContexts)
		{
			// We need to get a read-only lock on the block.
			Block block;

			using (blocks.AcquireBlockLock(RequestLock.Read, lineIndex, out block))
			{
				// Now that we have a block, grab the text and text spans and
				// format them. If we don't have any text spans, we can return
				// a simple formatted string.
				string text = block.Text;
				TextSpanCollection spans = block.TextSpans;
				string markup = spans.IsEmpty
					? PangoUtility.Escape(text)
					: FormatText(text, spans);

				// Return the resulting markup.
				return markup;
			}
		}

		public override string GetLineNumber(int lineIndex)
		{
			return lineIndex.ToString(CultureInfo.InvariantCulture);
		}

		public override string GetLineStyleName(
			int lineIndex,
			LineContexts lineContexts)
		{
			// We only need a read-lock on the blocks just to make sure nothing moves
			// underneath us while we get the block key.
			using (blocks.AcquireLock(RequestLock.Read))
			{
				// Create the command and submit it to the project's command manager.
				Block block = blocks[lineIndex];
				string blockTypeName = block.BlockType.Name;
				return blockTypeName;
			}
		}

		public override string GetLineText(
			int lineIndex,
			LineContexts lineContexts)
		{
			using (blocks.AcquireLock(RequestLock.Read))
			{
				Block block = blocks[lineIndex];
				string line = block.Text;
				return line;
			}
		}

		public override LineBufferOperationResults InsertLines(
			int lineIndex,
			int count)
		{
			var composite = new CompositeCommand<BlockCommandContext>(true, false);

			using (project.Blocks.AcquireLock(RequestLock.Write))
			{
				for (int i = 0;
					i < count;
					i++)
				{
					var block = new Block(project.Blocks);
					var command = new InsertIndexedBlockCommand(lineIndex, block);
					composite.Commands.Add(command);
				}

				var context = new BlockCommandContext(project);
				project.Commands.Do(composite, context);

				return GetOperationResults();
			}
		}

		public override LineBufferOperationResults InsertText(
			int lineIndex,
			int characterIndex,
			string text)
		{
			using (project.Blocks.AcquireLock(RequestLock.Write))
			{
				Block block = project.Blocks[lineIndex];
				var position = new BlockPosition(block.BlockKey, characterIndex);
				var command = new InsertTextCommand(position, text);
				var context = new BlockCommandContext(project);
				project.Commands.Do(command, context);

				return GetOperationResults();
			}
		}

		protected override LineBufferOperationResults Do(
			DeleteTextOperation operation)
		{
			//// We only need a read-lock on the blocks just to make sure nothing moves
			//// underneath us while we get the block key.
			//using (blocks.AcquireLock(RequestLock.Read))
			//{
			//	// Create the command and submit it to the project's command manager.
			//	Block block = blocks[operation.LineIndex];
			//	var command =
			//		new DeleteTextCommand(
			//			new BlockPosition(block.BlockKey, operation.CharacterRange.StartIndex),
			//			operation.CharacterRange.Length);
			//	commands.Do(command);

			//	// Fire a line changed operation.
			//	RaiseLineChanged(new LineChangedArgs(operation.LineIndex));

			//	// Construct the operation results for the delete from information in the
			//	// command manager.
			//	var results =
			//		new LineBufferOperationResults(
			//			new BufferPosition(blocks.IndexOf(block), commands.LastPosition.TextIndex));
			//	return results;
			//}
			throw new NotImplementedException();
		}

		protected override LineBufferOperationResults Do(
			InsertTextOperation operation)
		{
			//// We need a write lock on the block and a read lock on the blocks.
			//Block block = blocks[operation.BufferPosition.LineIndex];

			//using (block.AcquireBlockLock(RequestLock.Write))
			//{
			//	// Create the command and submit it to the project's command manager.
			//	var command =
			//		new InsertTextCommand(
			//			new BlockPosition(block.BlockKey, operation.BufferPosition.CharacterIndex),
			//			operation.Text);
			//	commands.Do(command);

			//	// Fire a line changed operation.
			//	RaiseLineChanged(new LineChangedArgs(operation.BufferPosition.LineIndex));

			//	// Construct the operation results for the delete from information in the
			//	// command manager.
			//	var results =
			//		new LineBufferOperationResults(
			//			new BufferPosition(blocks.IndexOf(block), commands.LastPosition.TextIndex));
			//	return results;
			//}
			throw new NotImplementedException();
		}

		protected override LineBufferOperationResults Do(SetTextOperation operation)
		{
			//// We only need a read-lock on the blocks just to make sure nothing moves
			//// underneath us while we get the block key.
			//using (blocks.AcquireLock(RequestLock.Read))
			//{
			//	// Create the command and submit it to the project's command manager.
			//	Block block = blocks[operation.LineIndex];
			//	var command = new SetTextCommand(block.BlockKey, operation.Text);
			//	commands.Do(command);

			//	// Fire a line changed operation.
			//	RaiseLineChanged(new LineChangedArgs(operation.LineIndex));

			//	// Construct the operation results for the delete from information in the
			//	// command manager.
			//	var results =
			//		new LineBufferOperationResults(
			//			new BufferPosition(blocks.IndexOf(block), commands.LastPosition.TextIndex));
			//	return results;
			//}
			throw new NotImplementedException();
		}

		protected override LineBufferOperationResults Do(
			InsertLinesOperation operation)
		{
			//// We need a write lock on the blocks since this will be making changes
			//// to the structure of the document.
			//using (blocks.AcquireLock(RequestLock.Write))
			//{
			//	// Create the command and submit it to the project's command manager.
			//	Block block = blocks[operation.LineIndex];
			//	var command = new InsertAfterBlockCommand(block.BlockKey, operation.Count);
			//	commands.Do(command);

			//	// Raise the events to indicate the line changed.
			//	RaiseLinesInserted(
			//		new LineRangeEventArgs(
			//			operation.LineIndex, operation.LineIndex + operation.Count));

			//	// Construct the operation results for the delete from information in the
			//	// command manager.
			//	var results =
			//		new LineBufferOperationResults(
			//			new BufferPosition(blocks.IndexOf(block), commands.LastPosition.TextIndex));
			//	return results;
			//}
			throw new NotImplementedException();
		}

		protected override LineBufferOperationResults Do(
			DeleteLinesOperation operation)
		{
			//// We only need a read-lock on the blocks just to make sure nothing moves
			//// underneath us while we get the block key.
			//using (blocks.AcquireLock(RequestLock.Read))
			//{
			//	// Add each delete line into a composite command.
			//	var deleteCommand = new CompositeCommand();

			//	for (int lineIndex = operation.LineIndex;
			//		lineIndex < operation.LineIndex + operation.Count;
			//		lineIndex++)
			//	{
			//		Block block = blocks[lineIndex];
			//		var command = new DeleteBlockCommand(block.BlockKey);
			//		deleteCommand.Commands.Add(command);
			//	}

			//	// Submit the delete line.
			//	commands.Do(deleteCommand);

			//	// Raise the deleted line events.
			//	RaiseLinesDeleted(
			//		new LineRangeEventArgs(
			//			operation.LineIndex, operation.LineIndex + operation.Count));

			//	// Construct the operation results for the delete from information in the
			//	// command manager.
			//	var results =
			//		new LineBufferOperationResults(
			//			new BufferPosition(operation.LineIndex, commands.LastPosition.TextIndex));
			//	return results;
			//}
			throw new NotImplementedException();
		}

		/// <summary>
		/// Formats the text using the spans, adding in error formatting if there
		/// is a text span.
		/// </summary>
		/// <param name="text">The text to format.</param>
		/// <param name="spans">The spans we need to use to format.</param>
		/// <returns>A Pango-formatted string.</returns>
		private string FormatText(
			string text,
			TextSpanCollection spans)
		{
			// Create a string builder and go through the text, one character at a time.
			var buffer = new StringBuilder();
			bool inSpan = false;

			for (int index = 0;
				index < text.Length;
				index++)
			{
				// Grab the character at this position in the text.
				char c = text[index];
				bool hasSpan = spans.Contains(index);

				// If the inSpan and hasSpan is different, we need to either
				// open or close the span.
				if (hasSpan != inSpan)
				{
					// Add in the tag depending on if we are opening or close the tag.
					string tag = inSpan
						? "</span>"
						: "<span underline='error' underline_color='red' color='red'>";

					buffer.Append(tag);

					// Update the current inSpan state.
					inSpan = hasSpan;
				}

				// Add in the character we've been processing.
				buffer.Append(c);
			}

			// Check to see if we were in a tag, if we are, we need to close it.
			if (inSpan)
			{
				buffer.Append("</span>");
			}

			// Return the resulting buffer.
			string markup = buffer.ToString();
			Debug.WriteLine("M: " + markup);
			return markup;
		}

		private LineBufferOperationResults GetOperationResults()
		{
			int blockIndex =
				project.Blocks.IndexOf(project.Commands.LastPosition.BlockKey);
			var results =
				new LineBufferOperationResults(
					new BufferPosition(blockIndex, project.Commands.LastPosition.TextIndex));
			return results;
		}

		/// <summary>
		/// Called when the context menu is being populated.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="args">The event arguments.</param>
		private void OnPopulateContextMenu(
			object sender,
			PopulateContextMenuArgs args)
		{
			// We need a read lock on both the collection and the specific block
			// for the given line index.
			BufferPosition position = editorView.Caret.Position;
			int blockIndex = position.LineIndex;
			Block block;

			var context = new BlockCommandContext(project);

			using (blocks.AcquireBlockLock(RequestLock.Read, blockIndex, out block))
			{
				// Figure out if we have any spans for this position.
				if (!block.TextSpans.Contains(position.CharacterIndex))
				{
					// Nothing to add, so we can stop processing.
					return;
				}

				// Gather up all the text spans for the current position in the line.
				var textSpans = new ArrayList<TextSpan>();

				textSpans.AddAll(block.TextSpans.GetAll(position.CharacterIndex));

				// Gather up the menu items for this point.
				bool firstItem = true;

				foreach (TextSpan textSpan in textSpans)
				{
					C5.IList<IEditorAction> actions =
						textSpan.Controller.GetEditorActions(block, textSpan);

					foreach (IEditorAction action in actions)
					{
						// Add the separator, if we need it.
						if (firstItem)
						{
							args.Menu.Add(new SeparatorMenuItem());
							firstItem = false;
						}

						// Create a menu item and add it.
						IEditorAction doAction = action;

						var menuItem = new MenuItem(action.DisplayName);
						menuItem.Activated += delegate
						{
							doAction.Do(context);
							RaiseLineChanged(new LineChangedArgs(blockIndex));
						};

						args.Menu.Add(menuItem);
					}
				}
			}
		}

		#endregion

		#region Constructors

		public ProjectLineBuffer(
			Project project,
			EditorView editorView)
		{
			// Save the parameters as member fields for later.
			this.project = project;
			this.editorView = editorView;

			// Pull out some common elements.
			blocks = this.project.Blocks;
			commands = project.Commands;

			// Hook up the events.
			editorView.Controller.PopulateContextMenu += OnPopulateContextMenu;
		}

		#endregion

		#region Fields

		private readonly ProjectBlockCollection blocks;
		private readonly BlockCommandSupervisor commands;
		private readonly EditorView editorView;
		private readonly Project project;

		#endregion
	}
}
