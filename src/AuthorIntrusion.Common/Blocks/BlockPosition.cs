// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

namespace AuthorIntrusion.Common.Blocks
{
	public struct BlockPosition
	{
		#region Properties

		public BlockKey BlockKey { get; private set; }
		public int TextIndex { get; private set; }

		#endregion

		#region Constructors

		public BlockPosition(
			BlockKey blockKey,
			int textIndex)
			: this()
		{
			BlockKey = blockKey;
			TextIndex = textIndex;
		}

		#endregion
	}
}
