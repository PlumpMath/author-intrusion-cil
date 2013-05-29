// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Blocks;

namespace AuthorIntrusion.Common.Commands
{
	public class DeleteBlockCommand: SingleBlockCommand
	{
		#region Properties

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

			inverseCommand.Commands.Clear();
			inverseCommand.Commands.Add(insertCommand);

			// Delete the block from the list.
			project.Blocks.Remove(block);
		}

		protected override IBlockCommand GetInverseCommand(
			Project project,
			Block block)
		{
			return inverseCommand;
		}

		#endregion

		#region Constructors

		public DeleteBlockCommand(BlockKey key)
			: base(key)
		{
			inverseCommand = new CompositeCommand(true);
		}

		#endregion

		#region Fields

		private readonly CompositeCommand inverseCommand;

		#endregion
	}
}
