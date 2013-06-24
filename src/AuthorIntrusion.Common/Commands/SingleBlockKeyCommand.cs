// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Blocks;
using AuthorIntrusion.Common.Blocks.Locking;
using MfGames.Commands.TextEditing;

namespace AuthorIntrusion.Common.Commands
{
	/// <summary>
	/// Encapsulates a command that works on a single block identified by a key.
	/// </summary>
	public abstract class SingleBlockKeyCommand: BlockKeyCommand
	{
		#region Methods

		public override void Do(BlockCommandContext context)
		{
			// If we have a block key, we use that first.
			ProjectBlockCollection blocks = context.Blocks;

			if (UseBlockKey)
			{
				Block block;

				using (
					blocks.AcquireBlockLock(
						RequestLock.Write, RequestLock.Write, BlockKey, out block))
				{
					Do(context, block);
				}
			}
			else
			{
				Block block;

				using (
					blocks.AcquireBlockLock(
						RequestLock.Write, RequestLock.Write, (int) Line, out block))
				{
					BlockKey = block.BlockKey;
					Do(context, block);
				}
			}
		}

		public override void Undo(BlockCommandContext context)
		{
			// If we have a block key, we use that first.
			ProjectBlockCollection blocks = context.Blocks;

			if (UseBlockKey)
			{
				Block block;

				using (
					blocks.AcquireBlockLock(
						RequestLock.Read, RequestLock.Write, BlockKey, out block))
				{
					Undo(context, block);
				}
			}
			else
			{
				Block block;

				using (
					blocks.AcquireBlockLock(
						RequestLock.Read, RequestLock.Write, (int) Line, out block))
				{
					Undo(context, block);
				}
			}
		}

		#endregion

		#region Constructors

		protected SingleBlockKeyCommand(BlockKey blockKey)
			: base(blockKey)
		{
		}

		protected SingleBlockKeyCommand(Position line)
			: base(line)
		{
		}

		#endregion
	}
}
