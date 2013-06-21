// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Blocks;

namespace AuthorIntrusion.Common.Commands
{
	public class InsertIndexedBlockCommand: IBlockCommand
	{
		#region Properties

		public Block Block { get; private set; }
		public int BlockIndex { get; private set; }

		public bool IsUndoable
		{
			get { return true; }
		}

		public BlockPosition LastPosition { get; private set; }

		#endregion

		#region Methods

		public void Do(BlockCommandContext context)
		{
			// TODO: Need to fix this.
			//// We need a write lock since we are making changes to the collection itself.
			//using (project.Blocks.AcquireLock(RequestLock.Write))
			//{
			//	project.Blocks.Insert(BlockIndex, Block);
			//}
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
