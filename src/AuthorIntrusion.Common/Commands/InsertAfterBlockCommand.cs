﻿// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System;
using System.Collections.Generic;
using AuthorIntrusion.Common.Blocks;
using MfGames.Commands;

namespace AuthorIntrusion.Common.Commands
{
	/// <summary>
	/// A command to insert one or more blocks after a given block. The text value
	/// of the new blocks will be a blank string.
	/// </summary>
	public class InsertAfterBlockCommand: MultipleBlockKeyCommand
	{
		#region Properties

		protected int Count { get; private set; }

		#endregion

		#region Methods

		protected override void Do(
			BlockCommandContext context,
			Block block)
		{
			// Pull out some common elements we'll need.
			ProjectBlockCollection blocks = block.Blocks;
			int blockIndex = blocks.IndexOf(block) + 1;

			// Because of how block keys work, the ID is unique very time so we have
			// to update our inverse operation.
			addedBlocks.Clear();

			// Go through and create each block at a time, adding it to the inverse
			// command as we create them.
			for (int count = 0;
				count < Count;
				count++)
			{
				// Create and insert a new block into the system.
				var newBlock = new Block(blocks);
				blocks.Insert(blockIndex, newBlock);

				// Keep track of the block so we can remove them later.
				addedBlocks.Add(newBlock);

				// Update the position.
				if (UpdateTextPosition.HasFlag(DoTypes.Do))
				{
					context.Position = new BlockPosition(newBlock.BlockKey, 0);
				}
			}
		}

		protected override void Undo(
			BlockCommandContext context,
			Block block)
		{
			foreach (Block addedBlock in addedBlocks)
			{
				context.Blocks.Remove(addedBlock);
			}

			if (UpdateTextPosition.HasFlag(DoTypes.Undo))
			{
				context.Position = new BlockPosition(BlockKey, block.Text.Length);
			}
		}

		#endregion

		#region Constructors

		public InsertAfterBlockCommand(
			BlockKey blockKey,
			int count)
			: base(blockKey)
		{
			// Make sure we have a sane state.
			if (count <= 0)
			{
				throw new ArgumentOutOfRangeException(
					"count", "Cannot insert or zero or less blocks.");
			}

			// Keep track of the counts.
			Count = count;

			// We have to keep track of the blocks we added.
			addedBlocks = new List<Block>();
		}

		#endregion

		#region Fields

		private readonly List<Block> addedBlocks;

		#endregion
	}
}
