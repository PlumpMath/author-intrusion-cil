// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Blocks;
using MfGames.Commands.TextEditing;

namespace AuthorIntrusion.Common.Commands
{
	/// <summary>
	/// A common baes for commands that function on a single block.
	/// </summary>
	public abstract class BlockKeyCommand: IBlockCommand
	{
		#region Properties

		/// <summary>
		/// Gets the key that identifies the block this command operates on.
		/// </summary>
		public BlockKey BlockKey { get; protected set; }

		public virtual bool CanUndo
		{
			get { return true; }
		}

		public virtual bool IsTransient
		{
			get { return false; }
		}

		public Position Line { get; private set; }

		public bool UpdateTextPosition { get; set; }

		public bool UpdateTextSelection { get; set; }
		protected bool UseBlockKey { get; private set; }

		#endregion

		#region Methods

		/// <summary>
		/// Acquires the locks needed for a specific operation and then performs
		/// UnlockedDo().
		/// </summary>
		public abstract void Do(BlockCommandContext context);

		public void Redo(BlockCommandContext context)
		{
			Do(context);
		}

		public abstract void Undo(BlockCommandContext context);

		/// <summary>
		/// Performs the command on the given block.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="block">The block to perform the action on.</param>
		protected abstract void Do(
			BlockCommandContext context,
			Block block);

		protected abstract void Undo(
			BlockCommandContext context,
			Block block);

		#endregion

		#region Constructors

		protected BlockKeyCommand(BlockKey blockKey)
		{
			BlockKey = blockKey;
			UseBlockKey = true;
		}

		protected BlockKeyCommand(Position line)
		{
			Line = line;
			UseBlockKey = false;
		}

		#endregion
	}
}
