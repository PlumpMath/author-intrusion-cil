// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Blocks;

namespace AuthorIntrusion.Common.Commands
{
	/// <summary>
	/// A command to delete text from a single block.
	/// </summary>
	public class DeleteTextCommand: BlockPositionCommand
	{
		#region Properties

		public override bool IsUndoable
		{
			get { return true; }
		}

		public int Length { get; private set; }

		#endregion

		#region Methods

		protected override void Do(Block block)
		{
			// Figure out what the new text string would be.
			string newText = block.Text.Remove(TextIndex, Length);

			// Set the new text into the block. This will fire various events to
			// trigger the immediate and background processing.
			block.SetText(newText);
		}

		protected override IBlockCommand GetInverseCommand(
			Project project,
			Block block)
		{
			// Create an insert text operation that is the inverse of the delete.
			string deletedText = block.Text.Substring(TextIndex, Length);
			var command = new InsertTextCommand(BlockPosition, deletedText);
			return command;
		}

		#endregion

		#region Constructors

		public DeleteTextCommand(
			BlockPosition position,
			int length)
			: base(position)
		{
			Length = length;
			LastPosition = position;
		}

		#endregion
	}
}
