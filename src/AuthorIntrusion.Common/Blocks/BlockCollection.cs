// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using C5;

namespace AuthorIntrusion.Common.Blocks
{
	/// <summary>
	/// A specialized collection to manage Block objects in memory.
	/// </summary>
	public class BlockCollection: LinkedList<Block>
	{
		#region Properties

		public Block this[BlockKey blockKey]
		{
			get
			{
				foreach (Block block in this)
				{
					if (block.BlockKey == blockKey)
					{
						return block;
					}
				}

				throw new NoSuchItemException("Cannot find block " + blockKey);
			}
		}

		#endregion
	}
}
