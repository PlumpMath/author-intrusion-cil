// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Blocks;
using AuthorIntrusion.Common.Blocks.Locking;

namespace AuthorIntrusion.Common.Commands
{
	public class InsertIndexedBlockCommand: IBlockCommand
	{
		#region Properties

		public Block Block { get; private set; }
		public int BlockIndex { get; private set; }

		public bool CanUndo
		{
			get { return true; }
		}

		public bool IsTransient
		{
			get { return false; }
		}

		public bool IsUndoable
		{
			get { return true; }
		}

		public BlockPosition LastPosition { get; private set; }

		#endregion

		#region Methods

		public void Do(BlockCommandContext context)
		{
			// We need a write lock since we are making changes to the collection itself.
			using (context.Blocks.AcquireLock(RequestLock.Write))
			{
				context.Blocks.Insert(BlockIndex, Block);
			}
		}

		public void Redo(BlockCommandContext context)
		{
			Do(context);
		}

		public void Undo(BlockCommandContext context)
		{
			// We need a write lock since we are making changes to the collection itself.
			using (context.Blocks.AcquireLock(RequestLock.Write))
			{
				context.Blocks.Remove(Block);
			}
		}

		#endregion

		#region Constructors

		public InsertIndexedBlockCommand(
			int blockIndex,
			Block block)
		{
			BlockIndex = blockIndex;
			Block = block;
		}

		#endregion
	}
}
