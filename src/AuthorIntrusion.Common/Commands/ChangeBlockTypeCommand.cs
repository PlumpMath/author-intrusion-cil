// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Blocks;
using MfGames.Commands;

namespace AuthorIntrusion.Common.Commands
{
	/// <summary>
	/// A command to change the type of block to a given type.
	/// </summary>
	public class ChangeBlockTypeCommand: MultipleBlockKeyCommand
	{
		#region Properties

		public BlockType BlockType { get; private set; }

		#endregion

		#region Methods

		protected override void Do(
			BlockCommandContext context,
			Block block)
		{
			// We need to keep track of the previous block type so we can change
			// it back with Undo.
			previousBlockType = block.BlockType;

			// Set the block type.
			block.SetBlockType(BlockType);

			// Save the position from this command.
			if (UpdateTextPosition.HasFlag(DoTypes.Do))
			{
				context.Position = new BlockPosition(BlockKey, 0);
			}
		}

		protected override void Undo(
			BlockCommandContext context,
			Block block)
		{
			// Revert the block type.
			block.SetBlockType(previousBlockType);
		}

		#endregion

		#region Constructors

		public ChangeBlockTypeCommand(
			BlockKey blockKey,
			BlockType blockType)
			: base(blockKey)
		{
			BlockType = blockType;
		}

		#endregion

		#region Fields

		private BlockType previousBlockType;

		#endregion
	}
}
