// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Blocks;
using AuthorIntrusion.Common.Blocks.Locking;
using C5;
using MfGames.Commands;

namespace AuthorIntrusion.Common.Commands
{
	/// <summary>
	/// A command intended to be used as a deferred command to mass change multiple
	/// block types in a single request.
	/// </summary>
	public class ChangeMultipleBlockTypesCommand: IBlockCommand
	{
		public DoTypes UpdateTextPosition { get; set; }

		#region Properties

		public bool CanUndo
		{
			get { return true; }
		}

		/// <summary>
		/// Gets the list of changes to block keys and their new types.
		/// </summary>
		public HashDictionary<BlockKey, BlockType> Changes { get; private set; }

		public bool IsTransient
		{
			get { return false; }
		}

		#endregion

		#region Methods

		public void Do(BlockCommandContext context)
		{
			// Since we're making chanegs to the list, we need a write lock.
			ProjectBlockCollection blocks = context.Blocks;

			using (blocks.AcquireLock(RequestLock.Write))
			{
				// Clear out the undo list since we'll be rebuilding it.
				previousBlockTypes.Clear();

				// Go through all the blocks in the project.
				foreach (Block block in blocks)
				{
					if (Changes.Contains(block.BlockKey))
					{
						BlockType blockType = Changes[block.BlockKey];
						BlockType existingType = block.BlockType;

						previousBlockTypes[block.BlockKey] = existingType;
						block.SetBlockType(blockType);
					}
				}
			}
		}

		public void Redo(BlockCommandContext context)
		{
			Do(context);
		}

		public void Undo(BlockCommandContext context)
		{
			// Since we're making chanegs to the list, we need a write lock.
			ProjectBlockCollection blocks = context.Blocks;

			using (blocks.AcquireLock(RequestLock.Write))
			{
				// Go through all the blocks in the project.
				foreach (Block block in blocks)
				{
					if (Changes.Contains(block.BlockKey))
					{
						// Revert the type of this block.
						BlockType blockType = previousBlockTypes[block.BlockKey];

						block.SetBlockType(blockType);
					}
				}
			}
		}

		#endregion

		#region Constructors

		public ChangeMultipleBlockTypesCommand()
		{
			Changes = new HashDictionary<BlockKey, BlockType>();
			previousBlockTypes = new HashDictionary<BlockKey, BlockType>();
			UpdateTextPosition = DoTypes.All;
		}

		#endregion

		#region Fields

		private readonly HashDictionary<BlockKey, BlockType> previousBlockTypes;

		#endregion
	}
}
