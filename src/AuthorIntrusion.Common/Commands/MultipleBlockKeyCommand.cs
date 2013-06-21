// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Blocks;
using AuthorIntrusion.Common.Blocks.Locking;

namespace AuthorIntrusion.Common.Commands
{
	/// <summary>
	/// Encapsulates a command that is identified by a single block but affects the
	/// entire structure of the document.
	/// </summary>
	public abstract class MultipleBlockKeyCommand: BlockKeyCommand
	{
		#region Methods

		public override void Do(BlockCommandContext context)
		{
			Block block;

			using (
				context.Blocks.AcquireBlockLock(
					RequestLock.Write, RequestLock.Write, BlockKey, out block))
			{
				Do(context, block);
			}
		}

		public override void Undo(BlockCommandContext context)
		{
			Block block;

			using (
				context.Blocks.AcquireBlockLock(
					RequestLock.Write, RequestLock.Write, BlockKey, out block))
			{
				Undo(context, block);
			}
		}

		#endregion

		#region Constructors

		protected MultipleBlockKeyCommand(BlockKey blockKey)
			: base(blockKey)
		{
		}

		#endregion
	}
}
