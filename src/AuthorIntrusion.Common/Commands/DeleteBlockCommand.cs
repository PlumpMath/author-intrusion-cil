// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Blocks;

namespace AuthorIntrusion.Common.Commands
{
	public class DeleteBlockCommand: MultipleBlockKeyCommand
	{
		#region Properties

		/// <summary>
		/// Gets or sets a value indicating whether the operation should ensure
		/// there is always one line in the block buffer. In most cases, this should
		/// remain false except for undo operations.
		/// </summary>
		/// <value>
		///   <c>true</c> if no minimum line processing should be done; otherwise, <c>false</c>.
		/// </value>
		public bool IgnoreMinimumLines { get; set; }

		#endregion

		#region Methods

		protected override void Do(
			BlockCommandContext context,
			Block block)
		{
			// We need the index of the block so we can restore it back into
			// its place.
			removedBlockIndex = block.Blocks.IndexOf(block);
			removedBlock = block;

			// Delete the block from the list.
			block.Blocks.Remove(block);

			// If we have no more blocks, then we need to ensure we have a minimum
			// number of blocks.
			addedBlankBlock = null;

			if (!IgnoreMinimumLines
				&& block.Blocks.Count == 0)
			{
				// Create a new placeholder block, which is blank.
				addedBlankBlock = new Block(
					block.Blocks, block.Project.BlockTypes.Paragraph);

				block.Blocks.Add(addedBlankBlock);
			}
			else if (!IgnoreMinimumLines)
			{
				// TODO: Need to fix this.
				//// We have to figure out where the cursor would be after this operation.
				//// Ideally, this would be the block in the current position, but if this
				//// is the last line, then use that.
				//LastPosition = new BlockPosition(
				//	blockIndex < block.Blocks.Count
				//		? block.Blocks[blockIndex].BlockKey
				//		: block.Blocks[blockIndex - 1].BlockKey,
				//	0);
			}
		}

		protected override void Undo(
			BlockCommandContext context,
			Block block)
		{
			// Insert in the old block.
			context.Blocks.Insert(removedBlockIndex, removedBlock);

			// Remove the blank block, if we added one.
			if (addedBlankBlock != null)
			{
				context.Blocks.Remove(addedBlankBlock);
				addedBlankBlock = null;
			}
		}

		#endregion

		#region Constructors

		public DeleteBlockCommand(BlockKey blockKey)
			: base(blockKey)
		{
		}

		#endregion

		#region Fields

		private Block addedBlankBlock;
		private Block removedBlock;
		private int removedBlockIndex;

		#endregion
	}
}
