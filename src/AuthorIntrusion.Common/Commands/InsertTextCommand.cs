// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Blocks;
using MfGames.Commands.TextEditing;

namespace AuthorIntrusion.Common.Commands
{
	/// <summary>
	/// Operation to insert text into a single block at a given position.
	/// </summary>
	public class InsertTextCommand: BlockPositionCommand
	{
		#region Properties

		protected string Text { get; private set; }

		#endregion

		#region Methods

		/// <summary>
		/// Performs the command on the given block.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="block">The block to perform the action on.</param>
		/// <param name="project">The project that contains the current state.</param>
		protected override void Do(
			BlockCommandContext context,
			Block block)
		{
			// Save the previous text so we can undo it.
			previousText = block.Text;

			// Figure out what the new text string would be.
			int textIndex = new Position(TextIndex).Normalize(block.Text);
			string newText = block.Text.Insert(textIndex, Text);

			// Set the new text into the block. This will fire various events to
			// trigger the immediate and background processing.
			block.SetText(newText);

			// After we insert text, we need to give the immediate editor plugins a
			// chance to made any alterations to the output.
			block.Project.Plugins.ProcessImmediateEdits(
				context, block, textIndex + Text.Length);

			// Set the new position in the buffer.
			context.Position = new BlockPosition(BlockKey, textIndex + Text.Length);
		}

		protected override void Undo(
			BlockCommandContext context,
			Block block)
		{
			block.SetText(previousText);

			// Set the new cursor position.
			int textIndex = BlockPosition.TextIndex.Normalize(previousText);
			context.Position = new BlockPosition(BlockPosition.BlockKey, textIndex);
		}

		#endregion

		#region Constructors

		public InsertTextCommand(
			BlockPosition position,
			string text)
			: base(position)
		{
			Text = text;
		}

		public InsertTextCommand(
			TextPosition textPosition,
			string text)
			: base(textPosition)
		{
			Text = text;
		}

		#endregion

		#region Fields

		private string previousText;

		#endregion
	}
}
