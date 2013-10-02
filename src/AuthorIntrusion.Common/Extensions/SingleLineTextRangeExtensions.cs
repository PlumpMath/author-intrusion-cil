// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Blocks;
using AuthorIntrusion.Common.Blocks.Locking;
using MfGames.Commands.TextEditing;

namespace AuthorIntrusion.Common.Extensions
{
	public static class SingleLineTextRangeExtensions
	{
		#region Methods

		public static void GetBeginAndEndCharacterIndices(
			this SingleLineTextRange range,
			ProjectBlockCollection blocks,
			out int blockIndex,
			out int sourceBegin,
			out int sourceEnd)
		{
			string text;
			GetBeginAndEndCharacterIndices(
				range, blocks, out blockIndex, out sourceBegin, out sourceEnd, out text);
		}

		public static void GetBeginAndEndCharacterIndices(
			this SingleLineTextRange range,
			ProjectBlockCollection blocks,
			out int blockIndex,
			out int sourceBegin,
			out int sourceEnd,
			out string text)
		{
			using (blocks.AcquireLock(RequestLock.Read))
			{
				// Start by getting the block based on the index.
				Block block;

				blockIndex = range.LinePosition.GetLineIndex(blocks.Count);

				using (
					blocks.AcquireBlockLock(
						RequestLock.Read, RequestLock.Read, blockIndex, out block))
				{
					// Get the text and calculate the character indicies.
					text = block.Text;

					range.GetBeginAndEndCharacterIndices(text, out sourceBegin, out sourceEnd);
				}
			}
		}

		#endregion
	}
}
