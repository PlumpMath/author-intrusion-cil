// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Blocks;
using MfGames.Commands;

namespace AuthorIntrusion.Common.Commands
{
	public abstract class CompositeBlockPositionCommand: CompositeCommand<BlockCommandContext>
	{
		#region Properties

		public BlockKey BlockKey
		{
			get { return BlockPosition.BlockKey; }
		}

		public BlockPosition BlockPosition { get; set; }

		#endregion

		#region Constructors

		protected CompositeBlockPositionCommand(
			BlockPosition position,
			bool isUndoable = true)
			: base(isUndoable, false)
		{
			BlockPosition = position;
		}

		#endregion
	}
}
