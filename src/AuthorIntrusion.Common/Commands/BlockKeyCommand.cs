// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Blocks;
using AuthorIntrusion.Common.Blocks.Locking;

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
		public BlockKey BlockKey { get; private set; }

		public abstract bool IsUndoable { get; }
		public BlockPosition LastPosition { get; protected set; }

		#endregion

		#region Methods

		/// <summary>
		/// Acquires the locks needed for a specific operation and then performs
		/// UnlockedDo().
		/// </summary>
		/// <param name="project">The project.</param>
		public abstract void Do(Project project);

		public IBlockCommand GetInverseCommand(Project project)
		{
			// We need a read access to the project.
			Block block;

			using (project.Blocks.AcquireBlockLock(RequestLock.Read, BlockKey, out block)
				)
			{
				// Perform the action on the block.
				return GetInverseCommand(project, block);
			}
		}

		/// <summary>
		/// Performs the command on the given block.
		/// </summary>
		/// <param name="block">The block to perform the action on.</param>
		protected abstract void Do(Block block);

		/// <summary>
		/// Gets the inverse command for a given block.
		/// </summary>
		/// <param name="project">The project that contains the current state.</param>
		/// <param name="block">The block to perform the action on.</param>
		protected abstract IBlockCommand GetInverseCommand(
			Project project,
			Block block);

		#endregion

		#region Constructors

		protected BlockKeyCommand(BlockKey blockKey)
		{
			BlockKey = blockKey;
		}

		#endregion
	}
}
