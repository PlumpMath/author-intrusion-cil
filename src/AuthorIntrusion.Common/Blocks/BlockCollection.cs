// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System;
using System.Collections.Generic;
using System.Linq;

namespace AuthorIntrusion.Common.Blocks
{
	/// <summary>
	/// A specialized collection to manage Block objects in memory.
	/// </summary>
	public class BlockCollection: List<Block>
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

				throw new IndexOutOfRangeException("Cannot find block " + blockKey);
			}
		}

		#endregion

		#region Events

		/// <summary>
		/// Occurs when the collection changes.
		/// </summary>
		public event EventHandler<EventArgs> CollectionChanged;

		#endregion

		#region Methods

		/// <summary>
		/// Adds the specified block to the collection.
		/// </summary>
		/// <param name="block">The block.</param>
		public new void Add(Block block)
		{
			base.Add(block);
			RaiseCollectionChanged();
		}

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

		public new void Insert(
			int index,
			Block block)
		{
			base.Insert(index, block);
			RaiseCollectionChanged();
		}

		public new void Remove(Block block)
		{
			base.Remove(block);
			RaiseCollectionChanged();
		}

		public new void RemoveAt(int index)
		{
			base.RemoveAt(index);
			RaiseCollectionChanged();
		}

		private void RaiseCollectionChanged()
		{
			EventHandler<EventArgs> listeners = CollectionChanged;

			if (listeners != null)
			{
				listeners(this, EventArgs.Empty);
			}
		}

		#endregion
	}
}
