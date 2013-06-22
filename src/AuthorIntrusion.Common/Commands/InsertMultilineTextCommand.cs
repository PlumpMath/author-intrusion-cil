// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System.Diagnostics.Contracts;
using AuthorIntrusion.Common.Blocks;
using AuthorIntrusion.Common.Blocks.Locking;
using C5;
using MfGames.Commands.TextEditing;

namespace AuthorIntrusion.Common.Commands
{
	/// <summary>
	/// Command to insert multiple lines of text into the blocks.
	/// </summary>
	public class InsertMultilineTextCommand: IBlockCommand
	{
		#region Properties

		public BlockPosition BlockPosition { get; set; }

		public bool IsUndoable
		{
			get { return true; }
		}

		public BlockPosition LastPosition { get; private set; }

		public string Text { get; private set; }

		#endregion

		#region Methods

		private LinkedList<Block> addedBlocks;
		private DeleteTextCommand deleteFirstCommand;
		private InsertTextCommand insertFirstCommand;

		public void Do(BlockCommandContext context)
		{
			// We have to clear the undo buffer every time because we'll be creating
			// new blocks.
			addedBlocks.Clear();

			// Start by breaking apart the lines on the newline.
			string[] lines = Text.Split('\n');

			// Make changes to the first line by creating a command, adding it to the
			// list of commands we need an inverse for, and then performing it.
			Block block = context.Blocks[BlockPosition.BlockKey];
			string remainingText = block.Text.Substring((int)BlockPosition.TextIndex);
			deleteFirstCommand = new DeleteTextCommand(
				BlockPosition, block.Text.Length);
			insertFirstCommand = new InsertTextCommand(BlockPosition, lines[0]);

			deleteFirstCommand.Do(context);
			insertFirstCommand.Do(context);

			// Update the final lines text with the remains of the first line.
			int lastLineLength = lines[lines.Length - 1].Length;
			lines[lines.Length - 1] += remainingText;

			// For the remaining lines, we need to insert each one in turn.
			LastPosition = BlockPosition.Empty;

			if (lines.Length > 1)
			{
				// Go through all the lines in reverse order to insert them.
				int firstBlockIndex = context.Blocks.IndexOf(block);

				for (int i = lines.Length - 1;
					i > 0;
					i--)
				{
					// Insert the line and set its text value.
					var newBlock = new Block(context.Blocks);

					addedBlocks.Add(newBlock);

					using (newBlock.AcquireBlockLock(RequestLock.Write))
					{
						newBlock.SetText(lines[i]);
					}

					context.Blocks.Insert(firstBlockIndex + 1, newBlock);

					// Update the last position as we go.
					if (LastPosition == BlockPosition.Empty)
					{
						LastPosition = new BlockPosition(newBlock.BlockKey, (Position)lastLineLength);
					}
				}
			}
		}

		public void Redo(BlockCommandContext context)
		{
			Do(context);
		}

		public void Undo(BlockCommandContext context)
		{
			// Delete all the added blocks first.
			foreach (Block block in addedBlocks)
			{
				context.Blocks.Remove(block);
			}

			// Restore the text from the first line.
			deleteFirstCommand.Undo(context);
			insertFirstCommand.Undo(context);
		}

		public bool CanUndo
		{
			get { return true; }
		}

		public bool IsTransient {
			get { return false; }
		}

		#endregion

		#region Constructors

		public InsertMultilineTextCommand(
			BlockPosition position,
			string text)
		{
			// Make sure we have a sane state.
			Contract.Assert(!text.Contains("\r"));

			// Save the text for the changes.
			BlockPosition = position;
			Text = text;

			// Set up our collection.
			addedBlocks = new LinkedList<Block>();
		}

		#endregion
	}
}
