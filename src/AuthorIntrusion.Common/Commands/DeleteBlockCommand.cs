// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Blocks;

namespace AuthorIntrusion.Common.Commands
{
	public class DeleteBlockCommand: BlockKeyCommand
	{
		#region Properties

		/// <summary>
		/// Gets or sets a value indicating whether the operation should ensure
		/// there is always one line in the block buffer. In most cases, this should
		/// remain false except for undo operations.
		/// </summary>
		/// <value>
		///   <c>true</c> if no minimum line processing should be done; otherwise, <c>false</c>.
		/// </value>
		public bool IgnoreMinimumLines { get; set; }

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
			// Figure out the current state at the point of deleting and populate the
			// composite command to restore that state if the operation is undone.
			int blockIndex = project.Blocks.IndexOf(block);
			IBlockCommand insertCommand = new InsertIndexedBlockCommand(
				blockIndex, block);

			inverseCommand.LastPosition = new BlockPosition(block, block.Text.Length);
			inverseCommand.Commands.Clear();
			inverseCommand.Commands.Add(insertCommand);

			// Delete the block from the list.
			project.Blocks.Remove(block);

			// If we have no more blocks, then we need to ensure we have a minimum
			// number of blocks.
			if (!IgnoreMinimumLines
				&& project.Blocks.Count == 0)
			{
				// Create a new placeholder block, which is blank.
				var blankBlock = new Block(project.Blocks)
				{
					Text = "",
				};

				project.Blocks.Add(blankBlock);

				// Set the last position to this newly created block.
				LastPosition = new BlockPosition(blankBlock, 0);

				// Because we added a block, we also have to add in the delete
				// for the reverse operation.
				var deleteBlankCommand = new DeleteBlockCommand(blankBlock.BlockKey)
				{
					IgnoreMinimumLines = true,
				};

				inverseCommand.Commands.InsertFirst(deleteBlankCommand);
			}
			else if (!IgnoreMinimumLines)
			{
				// We have to figure out where the cursor would be after this operation.
				// Ideally, this would be the block in the current position, but if this
				// is the last line, then use that.
				LastPosition = new BlockPosition(
					blockIndex < project.Blocks.Count
						? project.Blocks[blockIndex].BlockKey
						: project.Blocks[blockIndex - 1].BlockKey,
					0);
			}
		}

		protected override IBlockCommand GetInverseCommand(
			Project project,
			Block block)
		{
			return inverseCommand;
		}

		#endregion

		#region Constructors

		public DeleteBlockCommand(BlockKey blockKey)
			: base(blockKey)
		{
			inverseCommand = new CompositeCommand(true);
		}

		#endregion

		#region Fields

		private readonly CompositeCommand inverseCommand;

		#endregion
	}
}
