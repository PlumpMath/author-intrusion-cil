// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Blocks;

namespace AuthorIntrusion.Common.Commands
{
	/// <summary>
	/// A command to change the type of block to a given type.
	/// </summary>
	public class ChangeBlockTypeCommand: BlockKeyCommand
	{
		#region Properties

		public BlockType BlockType { get; private set; }

		public override bool IsUndoable
		{
			get { return true; }
		}

		#endregion

		#region Methods

		protected override void Do(
			Project project,
			Block block)
		{
			block.BlockType = BlockType;
		}

		protected override IBlockCommand GetInverseCommand(
			Project project,
			Block block)
		{
			var inverse = new ChangeBlockTypeCommand(block.BlockKey, block.BlockType);
			return inverse;
		}

		#endregion

		#region Constructors

		public ChangeBlockTypeCommand(
			BlockKey blockKey,
			BlockType blockType)
			: base(blockKey)
		{
			BlockType = blockType;
			LastPosition = new BlockPosition(blockKey, 0);
		}

		#endregion
	}
}
