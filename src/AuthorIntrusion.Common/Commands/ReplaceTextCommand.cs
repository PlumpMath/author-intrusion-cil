// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Blocks;

namespace AuthorIntrusion.Common.Commands
{
	public class ReplaceTextCommand: CompositeBlockPositionCommand
	{
		#region Properties

		public int Length { get; set; }
		public string Text { get; private set; }

		#endregion

		#region Constructors

		public ReplaceTextCommand(
			BlockPosition position,
			int length,
			string text)
			: base(position, true)
		{
			// Save the text for the changes.
			Length = length;
			Text = text;

			// Create the commands in this command.
			var deleteCommand = new DeleteTextCommand(position, Length);
			var insertCommand = new InsertTextCommand(position, Text);

			Commands.Add(deleteCommand);
			Commands.Add(insertCommand);

			// Set up the last position equal to the insert.
			LastPositionFunc = () => insertCommand.LastPosition;

			// Calculate the inverse of these operations.
			InverseLastPositionFunc =
				() => new BlockPosition(position.BlockKey, position.TextIndex + length);
		}

		#endregion
	}
}
