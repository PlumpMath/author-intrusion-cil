// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System.Linq;
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
				foreach (Block block in this.Where(block => block.BlockKey == blockKey))
				{
					return block;
				}

				throw new NoSuchItemException("Cannot find block " + blockKey);
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Finds the index of a given block key.
		/// </summary>
		/// <param name="blockKey">The block key to look it up.</param>
		/// <returns>The index of the position.</returns>
		public int IndexOf(BlockKey blockKey)
		{
			Block block = this[blockKey];
			int index = IndexOf(block);
			return index;
		}

		#endregion
	}
}
