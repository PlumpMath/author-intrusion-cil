// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Blocks;
using MfGames.Commands.TextEditing;

namespace AuthorIntrusion.Common.Commands
{
	/// <summary>
	/// Encapsulates the functionality for a command that takes a single block and
	/// index (e.g., BlockPosition).
	/// </summary>
	public abstract class BlockPositionCommand: SingleBlockKeyCommand
	{
		#region Properties

		/// <summary>
		/// Gets the block position for this command.
		/// </summary>
		protected BlockPosition BlockPosition
		{
			get
			{
				var position = new BlockPosition(BlockKey, (CharacterPosition) TextIndex);
				return position;
			}
		}

		/// <summary>
		/// Gets the index of the text operation.
		/// </summary>
		protected int TextIndex { get; private set; }

		#endregion

		#region Constructors

		protected BlockPositionCommand(BlockPosition position)
			: base(position.BlockKey)
		{
			TextIndex = (int) position.TextIndex;
		}

		protected BlockPositionCommand(TextPosition position)
			: base(position.Line)
		{
			TextIndex = (int) position.Character;
		}

		#endregion
	}
}
