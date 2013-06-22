// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Blocks;
using C5;
using MfGames.Commands;
using MfGames.Commands.TextEditing;

namespace AuthorIntrusion.Common.Commands
{
	/// <summary>
	/// Command to delete multiple lines of text from the blocks.
	/// </summary>
	public class DeleteMultilineTextCommand: CompositeCommand<BlockCommandContext>
	{
		private LinkedList<Block> removedBlocks;

		#region Constructors

		public DeleteMultilineTextCommand(
			BlockCollection blocks,
			BlockPosition startPosition,
			BlockPosition stopPosition)
			: base(true, false)
		{
			// Start by removing the text to the right of the first line.
			var deleteTextCommand = new DeleteTextCommand(startPosition, Position.End);

			Commands.Add(deleteTextCommand);

			// Copy the final line text, from beginning to position, into the first
			// line. This will merge the top and bottom lines.
			var insertTextCommand = new InsertTextFromBlock(
				startPosition,
				stopPosition.BlockKey, Position.Begin, stopPosition.TextIndex);

			Commands.Add(insertTextCommand);

			// Once we have a merged line, then just delete the remaining lines.
			// Figure out line ranges we'll be deleting text from.
			removedBlocks = new LinkedList<Block>();

			Block startBlock = blocks[startPosition.BlockKey];
			Block stopBlock = blocks[stopPosition.BlockKey];

			int startIndex = blocks.IndexOf(startBlock);
			int stopIndex = blocks.IndexOf(stopBlock);

			// Go through the remaining lines.
			for (int i = startIndex + 1;
				i <= stopIndex;
				i++)
			{
				// Get the block we're removing and add it to the list.
				Block removeBlock = blocks[i];

				removedBlocks.Add(removeBlock);

				// Add in a command to remove the block.
				var deleteBlockCommand = new DeleteBlockCommand(removeBlock.BlockKey);

				Commands.Add(deleteBlockCommand);
			}
		}

		#endregion
	}
}
