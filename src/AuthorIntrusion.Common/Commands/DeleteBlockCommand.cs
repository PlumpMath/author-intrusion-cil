// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Blocks;
using AuthorIntrusion.Common.Blocks.Locking;

namespace AuthorIntrusion.Common.Commands
{
	public class DeleteBlockCommand: IBlockCommand
	{
		#region Properties

		public bool CanUndo
		{
			get { return true; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether the operation should ensure
		/// there is always one line in the block buffer. In most cases, this should
		/// remain false except for undo operations.
		/// </summary>
		/// <value>
		///   <c>true</c> if no minimum line processing should be done; otherwise, <c>false</c>.
		/// </value>
		public bool IgnoreMinimumLines { get; set; }

		public bool IsTransient
		{
			get { return false; }
		}

		#endregion

		#region Methods

		public void Do(BlockCommandContext context)
		{
			using (context.Blocks.AcquireLock(RequestLock.Write))
			{
				// We need the index of the block so we can restore it back into
				// its place.
				Block block = context.Blocks[blockKey];
				removedBlockIndex = context.Blocks.IndexOf(blockKey);
				removedBlock = block;

				// Delete the block from the list.
				context.Blocks.Remove(block);

				// If we have no more blocks, then we need to ensure we have a minimum
				// number of blocks.
				addedBlankBlock = null;

				if (!IgnoreMinimumLines
					&& context.Blocks.Count == 0)
				{
					// Create a new placeholder block, which is blank.
					addedBlankBlock = new Block(
						context.Blocks, block.Project.BlockTypes.Paragraph);

					context.Blocks.Add(addedBlankBlock);

					context.Position = new BlockPosition(addedBlankBlock.BlockKey, 0);
				}
				else if (!IgnoreMinimumLines)
				{
					// We have to figure out where the cursor would be after this operation.
					// Ideally, this would be the block in the current position, but if this
					// is the last line, then use that.
					context.Position =
						new BlockPosition(
							removedBlockIndex < context.Blocks.Count
								? context.Blocks[removedBlockIndex].BlockKey
								: context.Blocks[removedBlockIndex - 1].BlockKey,
							0);
				}
			}
		}

		public void Redo(BlockCommandContext state)
		{
			Do(state);
		}

		public void Undo(BlockCommandContext context)
		{
			using (context.Blocks.AcquireLock(RequestLock.Write))
			{
				// Insert in the old block.
				context.Blocks.Insert(removedBlockIndex, removedBlock);

				// Set the last text position.
				context.Position = new BlockPosition(blockKey, removedBlock.Text.Length);

				// Remove the blank block, if we added one.
				if (addedBlankBlock != null)
				{
					context.Blocks.Remove(addedBlankBlock);
					addedBlankBlock = null;
				}
			}
		}

		#endregion

		#region Constructors

		public DeleteBlockCommand(BlockKey blockKey)
		{
			this.blockKey = blockKey;
		}

		#endregion

		#region Fields

		private Block addedBlankBlock;
		private readonly BlockKey blockKey;
		private Block removedBlock;
		private int removedBlockIndex;

		#endregion
	}
}
