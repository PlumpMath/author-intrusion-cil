// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Blocks;

namespace AuthorIntrusion.Common.Commands
{
	/// <summary>
	/// Operation to insert text into a single block at a given position.
	/// </summary>
	public class InsertTextCommand: SingleBlockCommand
	{
		#region Properties

		public override bool IsUndoable
		{
			get { return true; }
		}

		protected string Text { get; private set; }
		protected int TextIndex { get; private set; }

		#endregion

		#region Methods

		/// <summary>
		/// Performs the command on the given block.
		/// </summary>
		/// <param name="project">The project that contains the current state.</param>
		/// <param name="block">The block to perform the action on.</param>
		protected override void Do(
			Project project,
			Block block)
		{
			// Figure out what the new text string would be.
			string newText = block.Text.Insert(TextIndex, Text);

			// Set the new text into the block. This will fire various events to
			// trigger the immediate and background processing.
			block.SetText(newText);
		}

		protected override IBlockCommand GetInverseCommand(
			Project project,
			Block block)
		{
			// We use the full set operation instead of being more graceful with the
			// DeleteTextBlockCommand.
			var command = new SetTextCommand(BlockKey, block.Text);
			return command;
		}

		#endregion

		#region Constructors

		public InsertTextCommand(
			BlockPosition position,
			string text)
			: base(position.BlockKey)
		{
			TextIndex = position.TextIndex;
			Text = text;
		}

		#endregion
	}
}
