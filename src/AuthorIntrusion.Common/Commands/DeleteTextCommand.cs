// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Blocks;
using MfGames.Commands;
using MfGames.Commands.TextEditing;

namespace AuthorIntrusion.Common.Commands
{
	/// <summary>
	/// A command to delete text from a single block.
	/// </summary>
	public class DeleteTextCommand: BlockPositionCommand
	{
		#region Properties

		public Position CharacterEnd { get; private set; }
		public Position End { get; set; }

		#endregion

		#region Methods

		protected override void Do(
			BlockCommandContext context,
			Block block)
		{
			// Save the previous text so we can restore it.
			previousText = block.Text;

			// Figure out what the new text string would be.
			int startIndex = BlockPosition.TextIndex.Normalize(block.Text, End, false);
			int endIndex = End.Normalize(block.Text, TextIndex, true);
			string newText = block.Text.Remove(startIndex, endIndex - startIndex);

			// Set the new text into the block. This will fire various events to
			// trigger the immediate and background processing.
			block.SetText(newText);

			// Set the position after the next text.
			if(UpdateTextPosition.HasFlag(DoTypes.Do))
				context.Position = new BlockPosition(BlockKey,startIndex);
		}

		protected override void Undo(
			BlockCommandContext context,
			Block block)
		{
			block.SetText(previousText);

			// Set the position after the next text.
			int startIndex = BlockPosition.TextIndex.Normalize(block.Text, End, false);
			int endIndex = End.Normalize(block.Text, TextIndex, true);

			if (End.Index < 0)
			{
				endIndex = startIndex;
			}

			if(UpdateTextPosition.HasFlag(DoTypes.Undo))
				context.Position = new BlockPosition(BlockKey,endIndex);
		}

		#endregion

		#region Constructors

		public DeleteTextCommand(
			BlockPosition begin,
			Position end)
			: base(begin)
		{
			End = end;
		}

		public DeleteTextCommand(SingleLineTextRange range)
			: base(new TextPosition(range.Line, range.CharacterBegin))
		{
			// DREM ToTextPosition
			End = range.CharacterEnd;
		}

		#endregion

		#region Fields

		private string previousText;

		#endregion
	}
}
