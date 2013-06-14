// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System.Globalization;
using AuthorIntrusion.Common;
using AuthorIntrusion.Common.Blocks;
using AuthorIntrusion.Common.Commands;
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
				using (blocks.AcquireReadLock())
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

		public override int GetLineLength(
			int lineIndex,
			LineContexts lineContexts)
		{
			string line = GetLineText(lineIndex, lineContexts);
			return line.Length;
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
			using (blocks.AcquireReadLock())
			{
				// Create the command and submit it to the project's command manager.
				//Block block = blocks[lineIndex];
				//string blockType = block.BlockType.Name;
				//TODO return blockType
				return "Paragraph";
			}
		}

		public override string GetLineText(
			int lineIndex,
			LineContexts lineContexts)
		{
			using (blocks.AcquireReadLock())
			{
				Block block = blocks[lineIndex];
				string line = block.Text;
				return line;
			}
		}

		protected override LineBufferOperationResults Do(
			DeleteTextOperation operation)
		{
			// We only need a read-lock on the blocks just to make sure nothing moves
			// underneath us while we get the block key.
			using (blocks.AcquireReadLock())
			{
				// Create the command and submit it to the project's command manager.
				Block block = blocks[operation.LineIndex];
				var command =
					new DeleteTextCommand(
						new BlockPosition(block.BlockKey, operation.CharacterRange.StartIndex),
						operation.CharacterRange.Length);
				commands.Do(command);

				// Fire a line changed operation.
				RaiseLineChanged(new LineChangedArgs(operation.LineIndex));

				// Construct the operation results for the delete from information in the
				// command manager.
				var results =
					new LineBufferOperationResults(
						new BufferPosition(blocks.IndexOf(block), commands.LastPosition.TextIndex));
				return results;
			}
		}

		protected override LineBufferOperationResults Do(
			InsertTextOperation operation)
		{
			// We need a write lock on the block and a read lock on the blocks.
			Block block = blocks[operation.BufferPosition.LineIndex];

			using (block.AcquireWriteLock())
			{
				// Create the command and submit it to the project's command manager.
				var command =
					new InsertTextCommand(
						new BlockPosition(block.BlockKey, operation.BufferPosition.CharacterIndex),
						operation.Text);
				commands.Do(command);

				// Fire a line changed operation.
				RaiseLineChanged(new LineChangedArgs(operation.BufferPosition.LineIndex));

				// Construct the operation results for the delete from information in the
				// command manager.
				var results =
					new LineBufferOperationResults(
						new BufferPosition(blocks.IndexOf(block), commands.LastPosition.TextIndex));
				return results;
			}
		}

		protected override LineBufferOperationResults Do(SetTextOperation operation)
		{
			// We only need a read-lock on the blocks just to make sure nothing moves
			// underneath us while we get the block key.
			using (blocks.AcquireReadLock())
			{
				// Create the command and submit it to the project's command manager.
				Block block = blocks[operation.LineIndex];
				var command = new SetTextCommand(block.BlockKey, operation.Text);
				commands.Do(command);

				// Fire a line changed operation.
				RaiseLineChanged(new LineChangedArgs(operation.LineIndex));

				// Construct the operation results for the delete from information in the
				// command manager.
				var results =
					new LineBufferOperationResults(
						new BufferPosition(blocks.IndexOf(block), commands.LastPosition.TextIndex));
				return results;
			}
		}

		protected override LineBufferOperationResults Do(
			InsertLinesOperation operation)
		{
			// We need a write lock on the blocks since this will be making changes
			// to the structure of the document.
			using (blocks.AcquireWriteLock())
			{
				// Create the command and submit it to the project's command manager.
				Block block = blocks[operation.LineIndex];
				var command = new InsertAfterBlockCommand(block.BlockKey, operation.Count);
				commands.Do(command);

				// Raise the events to indicate the line changed.
				RaiseLinesInserted(
					new LineRangeEventArgs(
						operation.LineIndex, operation.LineIndex + operation.Count));

				// Construct the operation results for the delete from information in the
				// command manager.
				var results =
					new LineBufferOperationResults(
						new BufferPosition(blocks.IndexOf(block), commands.LastPosition.TextIndex));
				return results;
			}
		}

		protected override LineBufferOperationResults Do(
			DeleteLinesOperation operation)
		{
			// We only need a read-lock on the blocks just to make sure nothing moves
			// underneath us while we get the block key.
			using (blocks.AcquireReadLock())
			{
				// Add each delete line into a composite command.
				var deleteCommand = new CompositeCommand();

				for (int lineIndex = operation.LineIndex;
					lineIndex < operation.LineIndex + operation.Count;
					lineIndex++)
				{
					Block block = blocks[lineIndex];
					var command = new DeleteBlockCommand(block.BlockKey);
					deleteCommand.Commands.Add(command);
				}

				// Submit the delete line.
				commands.Do(deleteCommand);

				// Raise the deleted line events.
				RaiseLinesDeleted(
					new LineRangeEventArgs(
						operation.LineIndex, operation.LineIndex + operation.Count));

				// Construct the operation results for the delete from information in the
				// command manager.
				var results =
					new LineBufferOperationResults(
						new BufferPosition(operation.LineIndex, commands.LastPosition.TextIndex));
				return results;
			}
		}

		#endregion

		#region Constructors

		public ProjectLineBuffer(Project project)
		{
			this.project = project;
			blocks = this.project.Blocks;
			commands = project.Commands;
		}

		#endregion

		#region Fields

		private readonly ProjectBlockCollection blocks;
		private readonly BlockCommandSupervisor commands;
		private readonly Project project;

		#endregion
	}
}
