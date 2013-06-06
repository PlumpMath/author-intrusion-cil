// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System;
using AuthorIntrusion.Common.Blocks;
using C5;

namespace AuthorIntrusion.Common.Tests
{
	public class BlockOperationReporter
	{
		#region Methods

		public void Register(ProjectBlockCollection ownerCollection)
		{
			ownerCollection.ItemsAdded += OnItemsAdded;
			ownerCollection.ItemsRemoved += OnItemsRemoved;
			ownerCollection.ItemRemovedAt += OnItemRemovedAt;
			ownerCollection.ItemInserted += OnItemInserted;
			ownerCollection.CollectionCleared += OnCollectionCleared;
			ownerCollection.CollectionChanged += OnCollectionChanged;
		}

		private void OnCollectionChanged(object sender)
		{
			Console.WriteLine("Blocks.CollectionChanged");
		}

		private void OnCollectionCleared(
			object sender,
			ClearedEventArgs eventargs)
		{
			Console.WriteLine("Blocks.CollectionCleared: " + eventargs.Count + " items");
		}

		private void OnItemInserted(
			object sender,
			ItemAtEventArgs<Block> eventargs)
		{
			Console.WriteLine(
				"Blocks.ItemInserted: {0} @{1}", eventargs.Index, eventargs.Item);
		}

		private void OnItemRemovedAt(
			object sender,
			ItemAtEventArgs<Block> eventargs)
		{
			Console.WriteLine(
				"Blocks.ItemRemoved: {0} @ {1}", eventargs.Index, eventargs.Item);
		}

		private void OnItemsAdded(
			object sender,
			ItemCountEventArgs<Block> args)
		{
			Console.WriteLine("Blocks.ItemAdded: " + args.Item);
		}

		private void OnItemsRemoved(
			object sender,
			ItemCountEventArgs<Block> eventargs)
		{
			Console.WriteLine("Blocks.ItemsRemoved: {0}", eventargs.Count);
		}

		#endregion
	}
}
