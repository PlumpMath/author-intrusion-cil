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

using AuthorIntrusion.Contracts.Structures;

using C5;

#endregion

namespace AuthorIntrusion.Contracts.Collections
{
	/// <summary>
	/// Implements a list that manages structure elements.
	/// </summary>
	public class StructureList : ArrayList<Structure>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="StructureList"/> class.
		/// </summary>
		/// <param name="parent">The parent.</param>
		public StructureList(Structure parent)
		{
			// Save the structure so we can maintain the relationships.
			if (parent == null)
			{
				throw new ArgumentNullException("parent");
			}

			this.parent = parent;

			// Attach to our internal events for collections.
			ItemsAdded += OnItemsAdded;
			ItemsRemoved += OnItemsRemoved;
		}

		#endregion

		#region Structures

		private readonly Structure parent;

		/// <summary>
		/// Gets the parent structural element.
		/// </summary>
		/// <value>The parent.</value>
		public Structure Parent
		{
			get { return parent; }
		}

		#endregion

		#region Events

		/// <summary>
		/// Called for every item added to the collection.
		/// </summary>
		private void OnItemsAdded(
			object sender,
			ItemCountEventArgs<Structure> e)
		{
			// Make sure the item does not already have a parent. This is to
			// maintain structure but also to ensure continuity of elements.
			if (e.Item.Parent != null)
			{
				throw new InvalidOperationException(
					"Cannot add multiple parents to " + e.Item +
					". Remove it from the previous collection before adding it to this one.");
			}

			// Set the parent to this collection's parent.
			e.Item.Parent = parent;
		}

		/// <summary>
		/// Called for every item removed.
		/// </summary>
		private void OnItemsRemoved(
			object sender,
			ItemCountEventArgs<Structure> e)
		{
			// Make sure the item's parent is our own.
			if (e.Item.Parent != parent)
			{
				throw new InvalidOperationException(
					"Cannot clear a parent from item " + e.Item +
					" because it does not match this collection.");
			}

			// Clear the parent from the item.
			e.Item.Parent = null;
		}

		#endregion
	}
}