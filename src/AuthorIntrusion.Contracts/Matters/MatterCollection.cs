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

using System;

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

		private readonly IMattersContainer container;
		private int flattenedCount;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="MatterCollection"/> class.
		/// </summary>
		/// <param name="container">The container.</param>
		public MatterCollection(IMattersContainer container)
		{
			// Save the container so we can wire up parents.
			if (container == null)
			{
				throw new ArgumentNullException("container");
			}

			this.container = container;

			// Attach to events to observe collection changes.
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
		/// Occurs when the flattened count is changed.
		/// </summary>
		public event EventHandler<FlattenedCountChangedEventArgs>
			FlattenedCountChanged;

		/// <summary>
		/// Called when a child's flatted count is changed.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="AuthorIntrusion.Contracts.Matters.FlattenedCountChangedEventArgs"/> instance containing the event data.</param>
		private void OnChildFlattedCountChanged(
			object sender,
			FlattenedCountChangedEventArgs e)
		{
			flattenedCount += e.Amount;
			RaiseFlattenedCountChanged(e.Amount);
		}

		/// <summary>
		/// Called for every item added into a region contains in this list.
		/// </summary>
		private void OnChildItemsAdded(
			object sender,
			ItemCountEventArgs<Matter> eventargs)
		{
			flattenedCount++;
			RaiseFlattenedCountChanged(1);
		}

		/// <summary>
		/// Called for every item removed from a region contained in this list.
		/// </summary>
		private void OnChildItemsRemoved(
			object sender,
			ItemCountEventArgs<Matter> eventargs)
		{
			flattenedCount--;
			RaiseFlattenedCountChanged(-1);
		}

		/// <summary>
		/// Called for every item added.
		/// </summary>
		private void OnItemsAdded(
			object sender,
			ItemCountEventArgs<Matter> e)
		{
			// All matters have a parent container. If this non-null, we throw
			// an exception to help ensure integrity of the relationships 
			// between all of the items.
			Matter matter = e.Item;

			if (matter.ParentContainer != null)
			{
				throw new InvalidOperationException(
					"Cannot add the item (" + matter +
					") to the collection because it already has a parent container. Remove it from the previous list before adding it to this one.");
			}

			matter.ParentContainer = container;
			matter.ParagraphChanged += OnParagraphChanged;

			// If the item is a Region, then we subscribe to the region's
			// matter list so we can keep our flattened counts updated.
			if (matter.MatterType == MatterType.Region)
			{
				// Add up the events for listening to size changes. Also add
				// the current flattened count to catch the current size of our
				// new child.
				var region = (Region) matter;

				flattenedCount += region.Matters.FlattenedCount;
				RaiseFlattenedCountChanged(region.Matters.FlattenedCount);

				region.Matters.ItemsAdded += OnChildItemsAdded;
				region.Matters.ItemsRemoved += OnChildItemsRemoved;
				region.Matters.FlattenedCountChanged += OnChildFlattedCountChanged;
			}
		}

		/// <summary>
		/// Called for every item removed.
		/// </summary>
		private void OnItemsRemoved(
			object sender,
			ItemCountEventArgs<Matter> e)
		{
			// Clear the parent item out so we don't have to worry about integrity.
			Matter matter = e.Item;

			matter.ParentContainer = null;
			matter.ParagraphChanged -= OnParagraphChanged;

			// If the item is a Region, then we subscribe to the region's
			// matter list so we can keep our flattened counts updated.
			if (matter.MatterType == MatterType.Region)
			{
				var region = (Region) matter;

				flattenedCount -= region.Matters.FlattenedCount;
				RaiseFlattenedCountChanged(-region.Matters.FlattenedCount);

				region.Matters.ItemsAdded -= OnChildItemsAdded;
				region.Matters.ItemsRemoved -= OnChildItemsRemoved;
				region.Matters.FlattenedCountChanged -= OnChildFlattedCountChanged;
			}
		}

		/// <summary>
		/// Called when a contained paragraph changes.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="AuthorIntrusion.Contracts.Matters.ParagraphChangedEventArgs"/> instance containing the event data.</param>
		private void OnParagraphChanged(
			object sender,
			ParagraphChangedEventArgs e)
		{
			RaiseParagraphChanged(e);
		}

		/// <summary>
		/// Occurs when a contained paragraph changes.
		/// </summary>
		public event EventHandler<ParagraphChangedEventArgs> ParagraphChanged;

		/// <summary>
		/// Raises the flattened count changed event.
		/// </summary>
		/// <param name="amount">The amount.</param>
		protected void RaiseFlattenedCountChanged(int amount)
		{
			if (FlattenedCountChanged != null)
			{
				FlattenedCountChanged(this, new FlattenedCountChangedEventArgs(amount));
			}
		}

		/// <summary>
		/// Raises the paragraph changed event.
		/// </summary>
		/// <param name="e">The <see cref="AuthorIntrusion.Contracts.Matters.ParagraphChangedEventArgs"/> instance containing the event data.</param>
		protected void RaiseParagraphChanged(ParagraphChangedEventArgs e)
		{
			if (ParagraphChanged != null)
			{
				ParagraphChanged(this, e);
			}
		}

		#endregion
	}
}