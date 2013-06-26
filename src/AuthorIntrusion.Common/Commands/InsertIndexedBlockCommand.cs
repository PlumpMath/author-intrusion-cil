// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Blocks;
using AuthorIntrusion.Common.Blocks.Locking;
using MfGames.Commands;
using MfGames.Commands.TextEditing;

namespace AuthorIntrusion.Common.Commands
{
	public class InsertIndexedBlockCommand: IBlockCommand
	{
		public DoTypes UpdateTextPosition { get; set; }

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
				if(UpdateTextPosition.HasFlag(DoTypes.Undo))
				{
					previousPosition = context.Position;
				}

				context.Blocks.Insert(BlockIndex,Block);

				// Set the position after the command.
				if(UpdateTextPosition.HasFlag(DoTypes.Do))
				{
					context.Position = new BlockPosition(Block.BlockKey,CharacterPosition.Begin);
				}
			}
		}

		private BlockPosition? previousPosition;

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

				// Set the position after the command.
				if(UpdateTextPosition.HasFlag(DoTypes.Undo) &&
					previousPosition.HasValue)
				{
					context.Position = previousPosition.Value;
				}
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
			UpdateTextPosition = DoTypes.All;
		}

		#endregion
	}
}
