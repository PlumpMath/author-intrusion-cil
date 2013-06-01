// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Blocks;
using C5;
using MfGames.Locking;

namespace AuthorIntrusion.Common.Commands
{
	/// <summary>
	/// A command intended to be used as a deferred command to mass change multiple
	/// block types in a single request.
	/// </summary>
	public class ChangeMultipleBlockTypesCommand: IBlockCommand
	{
		#region Properties

		/// <summary>
		/// Gets the list of changes to block keys and their new types.
		/// </summary>
		public HashDictionary<BlockKey, BlockType> Changes { get; private set; }

		public bool IsUndoable
		{
			get { return true; }
		}

		public BlockPosition LastPosition
		{
			get { return BlockPosition.Empty; }
		}

		private ChangeMultipleBlockTypesCommand InverseCommand { get; set; }

		#endregion

		#region Methods

		public void Do(Project project)
		{
			// Since we're making chanegs to the list, we need a write lock.
			using (new NestableWriteLock(project.Blocks.Lock))
			{
				// Clear out the inverse since we'll be rebuilding it.
				GetInverseCommand(project);
				InverseCommand.Changes.Clear();

				// Go through all the blocks in the project.
				foreach (Block block in project.Blocks)
				{
					if (Changes.Contains(block.BlockKey))
					{
						BlockType blockType = Changes[block.BlockKey];
						BlockType existingType = block.BlockType;

						InverseCommand.Changes[block.BlockKey] = existingType;
						block.BlockType = blockType;
					}
				}
			}
		}

		public IBlockCommand GetInverseCommand(Project project)
		{
			// If we don't already have an inverse, then create one. We'll populate the
			// contents of the command as part of the "do" operation.
			if (InverseCommand == null)
			{
				InverseCommand = new ChangeMultipleBlockTypesCommand();
			}

			return InverseCommand;
		}

		#endregion

		#region Constructors

		public ChangeMultipleBlockTypesCommand()
		{
			Changes = new HashDictionary<BlockKey, BlockType>();
		}

		#endregion
	}
}
