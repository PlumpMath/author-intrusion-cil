// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Blocks;

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

		protected override void Do(Block block)
		{
			block.SetBlockType(BlockType);
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
	}
}
