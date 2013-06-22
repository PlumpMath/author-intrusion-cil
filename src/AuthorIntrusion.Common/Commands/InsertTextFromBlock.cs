using System.Text;
using AuthorIntrusion.Common.Blocks;
using MfGames.Commands.TextEditing;

namespace AuthorIntrusion.Common.Commands
{
	public class InsertTextFromBlock: MultipleBlockKeyCommand, IInsertTextFromTextRangeCommand<BlockCommandContext>
	{
		private int sourceLength;
		private int originalCharacterIndex;
		public BlockPosition DestinationPosition { get; private set; }
		public BlockKey SourceBlockKey { get; private set; }
		public Position CharacterBegin { get; private set; }
		public Position CharacterEnd { get; private set; }

		public InsertTextFromBlock(BlockPosition destinationPosition,
			BlockKey sourceBlockKey,
			Position characterBegin,
			Position characterEnd)
			: base(destinationPosition.BlockKey)
		{
			DestinationPosition = destinationPosition;
			SourceBlockKey = sourceBlockKey;
			CharacterBegin = characterBegin;
			CharacterEnd = characterEnd;
		}

		protected override void Do(
			BlockCommandContext context,
			Block block)
		{
			// Grab the text from the source line.
			string sourceLine = context.Blocks[SourceBlockKey].Text;
			int sourceBegin = CharacterBegin.Normalize(
				sourceLine,CharacterEnd,false);
			int sourceEnd = CharacterEnd.Normalize(
				sourceLine,CharacterBegin,true);
			string sourceText = sourceLine.Substring(
				sourceBegin,sourceEnd - sourceBegin);

			// Insert the text from the source line into the destination.
			string destinationLine = block.Text;
			var buffer = new StringBuilder(destinationLine);
			int characterIndex = DestinationPosition.TextIndex.Normalize(destinationLine);

			buffer.Insert(characterIndex,sourceText);

			// Save the source text length so we can delete it.
			sourceLength = sourceText.Length;
			originalCharacterIndex = characterIndex;

			// Set the line in the buffer.
			destinationLine = buffer.ToString();
			block.SetText(destinationLine);
		}

		protected override void Undo(
			BlockCommandContext context,
			Block block)
		{
			// Grab the line from the line buffer.
			string lineText = block.Text;
			var buffer = new StringBuilder(lineText);

			// Normalize the character ranges.
			buffer.Remove(originalCharacterIndex,sourceLength);

			// Set the line in the buffer.
			lineText = buffer.ToString();
			block.SetText(lineText);
		}
	}
}