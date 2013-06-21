// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Blocks;

namespace AuthorIntrusion.Common.Commands
{
	/// <summary>
	/// A command to insert one or more blocks after a given block. The text value
	/// of the new blocks will be a blank string.
	/// </summary>
	public class InsertAfterBlockCommand: MultipleBlockKeyCommand
	{
		#region Properties

		protected int Count { get; private set; }

		#endregion

		#region Methods

		protected override void Do(
			BlockCommandContext context,
			Block block)
		{
			// TODO: Fix this.
			//// Pull out some common elements we'll need.
			//ProjectBlockCollection blocks = block.Blocks;
			//int blockIndex = blocks.IndexOf(block) + 1;

			//// Because of how block keys work, the ID is unique very time so we have
			//// to update our inverse operation.
			//inverseComposite.Commands.Clear();
			//inverseComposite.LastPosition = new BlockPosition(
			//	block.BlockKey, block.Text.Length);

			//// Go through and create each block at a time, adding it to the inverse
			//// command as we create them.
			//for (int count = 0;
			//	count < Count;
			//	count++)
			//{
			//	// Create and insert a new block into the system.
			//	var newBlock = new Block(blocks);
			//	blocks.Insert(blockIndex, newBlock);

			//	// Add the corresponding delete block to the inverse command.
			//	var deleteCommand = new DeleteBlockCommand(newBlock.BlockKey);
			//	inverseComposite.Commands.Add(deleteCommand);

			//	// Set up the last position for this block.
			//	LastPosition = new BlockPosition(newBlock.BlockKey, 0);
			//}
		}

		#endregion

		#region Constructors

		public InsertAfterBlockCommand(
			BlockKey blockKey,
			int count)
			: base(blockKey)
		{
			// TODO: Need to fix this.
			//// Make sure we have a sane state.
			//Contract.Assert(count > 0);

			//// Keep track of the counts.
			//Count = count;

			//// Create the initial inverse command. This is a composite because we
			//// have to rebuild it every time we perform a do or redo operation.
			//inverseComposite = new CompositeCommand();
		}

		#endregion
	}
}
