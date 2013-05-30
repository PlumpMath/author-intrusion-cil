// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Blocks;
using MfGames.Locking;

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

		#endregion

		#region Methods

		public void Do(Project project)
		{
			// Because this is a block command, we need to get a writer lock on the
			// block subsystem.
			using (new WriteLock(project.Blocks.Lock))
			{
				UnlockedDo(project);
			}
		}

		public IBlockCommand GetInverseCommand(Project project)
		{
			// Since this command is a non-manipulating, we only need a read lock on
			// the system to get the current state.
			using (new ReadLock(project.Blocks.Lock))
			{
				// Retrieve the block that is referenced by the key so we can keep
				// the extending classes relatively small.
				Block block = project.Blocks[BlockKey];

				// Perform the action on the block.
				return GetInverseCommand(project, block);
			}
		}

		/// <summary>
		/// Performs the Do() method without locking.
		/// </summary>
		/// <param name="project">The project.</param>
		public void UnlockedDo(Project project)
		{
			// Retrieve the block that is referenced by the key so we can keep
			// the extending classes relatively small.
			Block block = project.Blocks[BlockKey];

			// Perform the action on the block.
			Do(project, block);
		}

		/// <summary>
		/// Performs the command on the given block.
		/// </summary>
		/// <param name="project">The project that contains the current state.</param>
		/// <param name="block">The block to perform the action on.</param>
		protected abstract void Do(
			Project project,
			Block block);

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

		protected BlockKeyCommand(BlockKey key)
		{
			BlockKey = key;
		}

		#endregion
	}
}
