#region Copyright and License

// Copyright (c) 2011, Moonfire Games
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

#endregion

#region Namespaces

using C5;

#endregion

namespace AuthorIntrusion.Contracts.Matters
{
	/// <summary>
	/// Represents a list of Matter object.
	/// </summary>
	public class MatterCollection : LinkedList<Matter>
	{
		#region Fields

		private int flattenedCount;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="MatterCollection"/> class.
		/// </summary>
		public MatterCollection()
		{
			ItemsAdded += OnItemsAdded;
			ItemsRemoved += OnItemsRemoved;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the number of matters in all recursive structures.
		/// </summary>
		public int FlattenedCount
		{
			get { return flattenedCount + Count; }
		}

		#endregion

		#region Events

		/// <summary>
		/// Called for every item added into a region contains in this list.
		/// </summary>
		private void OnChildItemsAdded(
			object sender,
			ItemCountEventArgs<Matter> eventargs)
		{
			flattenedCount++;
		}

		/// <summary>
		/// Called for every item removed from a region contained in this list.
		/// </summary>
		private void OnChildItemsRemoved(
			object sender,
			ItemCountEventArgs<Matter> eventargs)
		{
			flattenedCount--;
		}

		/// <summary>
		/// Called for every item added.
		/// </summary>
		private void OnItemsAdded(
			object sender,
			ItemCountEventArgs<Matter> e)
		{
			// If the item is a Region, then we subscribe to the region's
			// matter list so we can keep our flattened counts updated.
			if (e.Item.MatterType == MatterType.Region)
			{
				var region = (Region) e.Item;

				flattenedCount += region.Matters.FlattenedCount;
				region.Matters.ItemsAdded += OnChildItemsAdded;
				region.Matters.ItemsRemoved += OnChildItemsRemoved;
			}
		}

		/// <summary>
		/// Called for every item removed.
		/// </summary>
		private void OnItemsRemoved(
			object sender,
			ItemCountEventArgs<Matter> e)
		{
			// If the item is a Region, then we subscribe to the region's
			// matter list so we can keep our flattened counts updated.
			if (e.Item.MatterType == MatterType.Region)
			{
				var region = (Region) e.Item;

				flattenedCount -= region.Matters.FlattenedCount;
				region.Matters.ItemsAdded -= OnChildItemsAdded;
				region.Matters.ItemsRemoved -= OnChildItemsRemoved;
			}
		}

		#endregion
	}
}