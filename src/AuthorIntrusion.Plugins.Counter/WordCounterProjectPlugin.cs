// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System;
using System.Linq;
using AuthorIntrusion.Common.Blocks;
using AuthorIntrusion.Common.Plugins;
using C5;
using MfGames.HierarchicalPaths;

namespace AuthorIntrusion.Plugins.Counter
{
	/// <summary>
	/// A controller to handle word counting.
	/// </summary>
	public class WordCounterProjectPlugin: IBlockRelationshipProjectPlugin,
		IBlockTypeProjectPlugin
	{
		#region Methods

		/// <summary>
		/// Analyzes the block and counts the word. Once counted, this updates
		/// the block and all parent blocks with the altered change.
		/// </summary>
		/// <param name="block">The block.</param>
		/// <param name="blockVersion">The block version of the initial request.</param>
		public void AnalyzeBlock(
			Block block,
			int blockVersion)
		{
			// Grab the text and get the word counts.
			int wordCount;
			int characterCount;
			int nonWhitespaceCount;
			WordCounterHelper.CountWords(
				block.Text, out wordCount, out characterCount, out nonWhitespaceCount);

			// See if we have an existing key for these values, which we need for
			// the deltas. If the keys don't exist, then just use the full amount.
			int wordDelta = wordCount
				- block.Properties.GetOrDefault(CounterPaths.WordCountPath, 0);
			int characterDelta = characterCount
				- block.Properties.GetOrDefault(CounterPaths.CharacterCountPath, 0);
			int nonWhitespaceDelta = nonWhitespaceCount
				- block.Properties.GetOrDefault(CounterPaths.NonWhitespaceCountPath, 0);

			// Build up a dictionary of changes so we can simply setting them.
			var deltas = new HashDictionary<HierarchicalPath, int>();

			deltas[CounterPaths.WordCountPath] = wordDelta;
			deltas[CounterPaths.CharacterCountPath] = characterDelta;
			deltas[CounterPaths.NonWhitespaceCountPath] = nonWhitespaceDelta;

			// Get a write lock on the blocks list and update that block and all
			// parent blocks in the document.
			using (block.AcquireWriteLock())
			{
				// Log that we are analyzing this block.
				Log("BEGIN AnalyzeBlock: {0}: Words {1:N0}", block, wordCount);

				// First check to see if we've gotten stale.
				if (block.IsStale(blockVersion))
				{
					return;
				}

				// Grab the type deltas in the lock.
				HierarchicalPath typePath = CounterPaths.GetPath(block.BlockType);
				int typeDelta = 1 - block.Properties.GetOrDefault(typePath, 0);
				deltas[typePath] = typeDelta;

				// Get a list of the block and its parents.
				IList<Block> relatedBlocks = block.GetBlockAndParents();

				// Check one last time that the block hasn't gone stale.
				if (block.IsStale(blockVersion))
				{
					return;
				}

				// Update each block in order.
				foreach (Block relatedBlock in relatedBlocks)
				{
					// Update each block with the various deltas.
					UpdateDeltas(relatedBlock, deltas);
				}

				// Log that we finished processing this block.
				Log("END   AnalyzeBlock: {0}: Words {1:N0}", block, wordCount);
			}
		}

		public void ChangeBlockParent(
			Block block,
			Block oldParentBlock)
		{
			// We need a write lock on the blocks while we make this change.
			using (block.AcquireWriteLock())
			{
				Log("ChangeBlockParent: {0}: Old Parent {1}", block, oldParentBlock);

				// Figure out the deltas for this block.
				IDictionary<HierarchicalPath, int> deltas = GetCounts(block);

				// Figure out the lists for the old and new parent.
				IList<Block> oldParentBlocks = oldParentBlock == null
					? new ArrayList<Block>()
					: oldParentBlock.GetBlockAndParents();
				IList<Block> newParentBlocks = block.ParentBlock == null
					? new ArrayList<Block>()
					: block.ParentBlock.GetBlockAndParents();

				// Get rid of blocks common in both lists.
				var common = new HashSet<Block>();

				foreach (Block parentBlock in
					oldParentBlocks.Where(parentBlock => newParentBlocks.Contains(parentBlock))
					)
				{
					common.Add(parentBlock);
				}

				foreach (Block parentBlock in common)
				{
					oldParentBlocks.Remove(parentBlock);
					newParentBlocks.Remove(parentBlock);
				}

				// Update the entire relationship tree from the old parent block
				// by removing our counts from those blocks.
				if (oldParentBlock != null)
				{
					foreach (Block parentBlock in oldParentBlocks)
					{
						UpdateDeltas(parentBlock, deltas, -1);
					}
				}

				// Update the new relationship tree with the new parents
				// by adding our counts to the new relationship tree.
				if (block.ParentBlock != null)
				{
					foreach (Block parentBlock in newParentBlocks)
					{
						UpdateDeltas(parentBlock, deltas);
					}
				}
			}
		}

		public void ChangeBlockType(
			Block block,
			BlockType oldBlockType)
		{
			// We need a write lock on the blocks while we make this change.
			using (block.AcquireWriteLock())
			{
				// Report what we're doing if we have logging on.
				Log("ChangeBlockType: {0}: Old Type {1}", block, oldBlockType);

				// Figure out the deltas for this block.
				var deltas = new HashDictionary<HierarchicalPath, int>();
				deltas[CounterPaths.GetPath(oldBlockType)] = -1;
				deltas[CounterPaths.GetPath(block.BlockType)] = 1;

				// Update the parent types.
				IList<Block> relatedBlocks = block.GetBlockAndParents();

				foreach (Block relatedBlock in relatedBlocks)
				{
					UpdateDeltas(relatedBlock, deltas);
				}
			}
		}

		/// <summary>
		/// Gets the counts from the block properties.
		/// </summary>
		/// <param name="block">The block.</param>
		private IDictionary<HierarchicalPath, int> GetCounts(Block block)
		{
			// Build up a dictionary of counter paths and counts.
			var deltas = new HashDictionary<HierarchicalPath, int>();

			// Go through the standard counter paths.
			foreach (HierarchicalPath path in CounterPaths.StandardCounterPaths)
			{
				deltas[path] = block.Properties.GetOrDefault(path, 0);
			}

			// Return the resulting deltas.
			return deltas;
		}

		/// <summary>
		/// Logs a message to the Logger property, if set.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <param name="arguments">The arguments.</param>
		private void Log(
			string format,
			params object[] arguments)
		{
			if (Logger != null)
			{
				Logger(format, arguments);
			}
		}

		/// <summary>
		/// Updates the properties inside the block with the given deltas.
		/// </summary>
		/// <param name="block">The block.</param>
		/// <param name="deltas">The deltas.</param>
		/// <param name="multiplier">The multiplier.</param>
		private void UpdateDeltas(
			Block block,
			IDictionary<HierarchicalPath, int> deltas,
			int multiplier = 1)
		{
			foreach (HierarchicalPath path in deltas.Keys)
			{
				int delta = deltas[path] * multiplier;
				Log("  Update Delta: {0}: {1} += {2}", block, path, delta);
				block.Properties.AdditionOrAdd(path, delta);
			}
		}

		#endregion

		#region Fields

		/// <summary>
		/// If set, the given function will be called at key points in the
		/// class.
		/// </summary>
		public static Action<string, object[]> Logger;

		#endregion
	}
}
