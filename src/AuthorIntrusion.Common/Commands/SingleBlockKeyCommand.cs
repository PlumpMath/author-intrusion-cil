// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Blocks;
using AuthorIntrusion.Common.Blocks.Locking;

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
			ProjectBlockCollection blocks = context.Blocks;
			Block block;

			using (blocks.AcquireBlockLock(RequestLock.Write, BlockKey, out block))
			{
				Do(context, block);
			}
		}

		public override void Undo(BlockCommandContext context)
		{
			ProjectBlockCollection blocks = context.Blocks;
			Block block;

			using (blocks.AcquireBlockLock(RequestLock.Write, BlockKey, out block))
			{
				Undo(context, block);
			}
		}

		#endregion

		#region Constructors

		protected SingleBlockKeyCommand(BlockKey blockKey)
			: base(blockKey)
		{
		}

		#endregion
	}
}
