// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System.Text;
using AuthorIntrusion.Common.Blocks;
using MfGames.Commands.TextEditing;

namespace AuthorIntrusion.Common.Commands
{
	public class InsertTextFromBlock: MultipleBlockKeyCommand,
		IInsertTextFromTextRangeCommand<BlockCommandContext>
	{
		#region Properties

		public CharacterPosition CharacterBegin { get; private set; }
		public CharacterPosition CharacterEnd { get; private set; }
		public BlockPosition DestinationPosition { get; private set; }
		public BlockKey SourceBlockKey { get; private set; }

		#endregion

		#region Methods

		protected override void Do(
			BlockCommandContext context,
			Block block)
		{
			// Grab the text from the source line.
			string sourceLine = context.Blocks[SourceBlockKey].Text;
			int sourceBegin = CharacterBegin.NormalizeIndex(
				sourceLine, CharacterEnd, WordSearchDirection.Left);
			int sourceEnd = CharacterEnd.NormalizeIndex(
				sourceLine, CharacterBegin, WordSearchDirection.Right);
			string sourceText = sourceLine.Substring(
				sourceBegin, sourceEnd - sourceBegin);

			// Insert the text from the source line into the destination.
			string destinationLine = block.Text;
			var buffer = new StringBuilder(destinationLine);
			int characterIndex =
				DestinationPosition.TextIndex.NormalizeIndex(destinationLine);

			buffer.Insert(characterIndex, sourceText);

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
			buffer.Remove(originalCharacterIndex, sourceLength);

			// Set the line in the buffer.
			lineText = buffer.ToString();
			block.SetText(lineText);
		}

		#endregion

		#region Constructors

		public InsertTextFromBlock(
			BlockPosition destinationPosition,
			BlockKey sourceBlockKey,
			CharacterPosition characterBegin,
			CharacterPosition characterEnd)
			: base(destinationPosition.BlockKey)
		{
			DestinationPosition = destinationPosition;
			SourceBlockKey = sourceBlockKey;
			CharacterBegin = characterBegin;
			CharacterEnd = characterEnd;
		}

		#endregion

		#region Fields

		private int originalCharacterIndex;
		private int sourceLength;

		#endregion
	}
}
