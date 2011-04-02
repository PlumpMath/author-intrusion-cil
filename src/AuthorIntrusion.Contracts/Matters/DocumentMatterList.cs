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
	/// Manages the underlying list of <see cref="Matter"/> objects. There
	/// is typically only one list and all <see cref="Region"/> objects represent
	/// views into the list.
	/// </summary>
	public class DocumentMatterList : ArrayList<Matter>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="DocumentMatterList"/> class.
		/// </summary>
		public DocumentMatterList()
		{
			ItemsAdded += OnItemsAdded;
			ItemsRemoved += OnItemsRemoved;
		}

		#endregion

		#region Events

		/// <summary>
		/// Called when a matter is added to the list.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The ItemCountEventArgs instance containing the event data.</param>
		private void OnItemsAdded(
			object sender,
			ItemCountEventArgs<Matter> e)
		{
			// Check to see what type of item we have.
			if (e.Item.MatterType != MatterType.Region)
			{
				return;
			}

			// Cast this to a region and see if it already has an underlying
			// list. If it does, then we have a potentially invalid state.
			var region = (Region) e.Item;

			if (region.IsConnected)
			{
				throw new InvalidOperationException("Cannot add a Region to the document list if it is already connected.");
			}

			// Create a zero-item list right after the item being added.
			int index = IndexOf(e.Item);
			IList<Matter> regionView = View(index + 1, 0);

			region.ConnectToDocument(regionView);
		}

		/// <summary>
		/// Called when an item is removed from the list.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The ItemCountEventArgs instance containing the event data.</param>
		private static void OnItemsRemoved(
			object sender,
			ItemCountEventArgs<Matter> e)
		{
			// Check to see what type of item we have.
			if (e.Item.MatterType != MatterType.Region)
			{
				return;
			}

			// Cast this to a region and see if we have matters associated with
			// the list. If we don't, then don't do anything.
			var region = (Region)e.Item;

			region.DisconnectFromDocument();
		}

		#endregion
	}
}