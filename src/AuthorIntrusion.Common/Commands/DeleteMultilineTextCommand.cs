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
		#region Constructors

		public DeleteMultilineTextCommand(
			BlockCollection blocks,
			BlockPosition startPosition,
			BlockPosition stopPosition)
			: base(true, false)
		{
			// Save the variables so we can set the position.
			this.startPosition = startPosition;
			this.stopPosition = stopPosition;

			// If we are in the same line, we have a modified command.
			if (startPosition.BlockKey == stopPosition.BlockKey)
			{
				// We have a single-line delete.
				var singleDeleteCommand = new DeleteTextCommand(
					startPosition, stopPosition.TextIndex);

				Commands.Add(singleDeleteCommand);
				return;
			}

			// Start by removing the text to the right of the first line.
			var deleteTextCommand = new DeleteTextCommand(
				startPosition, CharacterPosition.End);

			Commands.Add(deleteTextCommand);

			// Copy the final line text, from beginning to position, into the first
			// line. This will merge the top and bottom lines.
			var insertTextCommand = new InsertTextFromBlock(
				startPosition,
				stopPosition.BlockKey,
				stopPosition.TextIndex,
				CharacterPosition.End);

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

		#region Fields

		private readonly LinkedList<Block> removedBlocks;
		private readonly BlockPosition startPosition;
		private readonly BlockPosition stopPosition;

		#endregion
	}
}
