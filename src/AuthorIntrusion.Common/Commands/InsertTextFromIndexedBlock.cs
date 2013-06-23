// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System.Text;
using AuthorIntrusion.Common.Blocks;
using AuthorIntrusion.Common.Blocks.Locking;
using MfGames.Commands.TextEditing;

namespace AuthorIntrusion.Common.Commands
{
	public class InsertTextFromIndexedBlock:
		IInsertTextFromTextRangeCommand<BlockCommandContext>
	{
		#region Properties

		public bool CanUndo
		{
			get { return true; }
		}

		public Position CharacterBegin { get; private set; }
		public Position CharacterEnd { get; private set; }
		public TextPosition DestinationPosition { get; private set; }

		public bool IsTransient
		{
			get { return false; }
		}

		public bool UpdateTextPosition { get; set; }
		public bool UpdateTextSelection { get; set; }

		protected int SourceBlockIndex { get; private set; }

		#endregion

		#region Methods

		public void Do(BlockCommandContext context)
		{
			// Grab the destination block and make sure everything is locked properly.
			Block block;

			using (
				context.Blocks.AcquireBlockLock(
					RequestLock.Write,
					RequestLock.Write,
					(int) DestinationPosition.Line,
					out block))
			{
				// Grab the text from the source line.
				string sourceLine = context.Blocks[SourceBlockIndex].Text;
				int sourceBegin = CharacterBegin.Normalize(sourceLine, CharacterEnd, false);
				int sourceEnd = CharacterEnd.Normalize(sourceLine, CharacterBegin, true);
				string sourceText = sourceLine.Substring(
					sourceBegin, sourceEnd - sourceBegin);

				// Insert the text from the source line into the destination.
				string destinationLine = block.Text;
				var buffer = new StringBuilder(destinationLine);
				int characterIndex = DestinationPosition.Character.Normalize(
					destinationLine);

				buffer.Insert(characterIndex, sourceText);

				// Save the source text length so we can delete it.
				sourceLength = sourceText.Length;
				originalCharacterIndex = characterIndex;

				// Set the line in the buffer.
				destinationLine = buffer.ToString();
				block.SetText(destinationLine);
			}
		}

		public void Redo(BlockCommandContext context)
		{
			Do(context);
		}

		public void Undo(BlockCommandContext context)
		{
			// Grab the destination block and make sure everything is locked properly.
			Block block;

			using (
				context.Blocks.AcquireBlockLock(
					RequestLock.Write,
					RequestLock.Write,
					(int) DestinationPosition.Line,
					out block))
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
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="InsertTextFromBlock" /> class.
		/// </summary>
		/// <param name="destinationPosition">The position to insert the text into.</param>
		/// <param name="sourceBlockIndex">Index of the source block.</param>
		/// <param name="characterBegin">The begin character index in the source block.</param>
		/// <param name="characterEnd">The end character index in the source block.</param>
		public InsertTextFromIndexedBlock(
			TextPosition destinationPosition,
			int sourceBlockIndex,
			Position characterBegin,
			Position characterEnd)
		{
			DestinationPosition = destinationPosition;
			SourceBlockIndex = sourceBlockIndex;
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
