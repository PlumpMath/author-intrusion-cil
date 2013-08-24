// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System;
using System.Collections.Generic;
using System.Linq;
using AuthorIntrusion.Common;
using AuthorIntrusion.Common.Blocks;
using AuthorIntrusion.Common.Blocks.Locking;
using AuthorIntrusion.Common.Plugins;
using MfGames.HierarchicalPaths;

namespace AuthorIntrusion.Plugins.Counter
{
	/// <summary>
	/// A controller to handle word counting.
	/// </summary>
	public class WordCounterProjectPlugin: IBlockTypeProjectPlugin
	{
		#region Properties

		public string Key
		{
			get { return "Word Counter"; }
		}

		#endregion

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
			// Grab counts from the current block text.
			int newCount = 1;
			int newWordCount;
			int newCharacterCount;
			int newNonWhitespaceCount;
			string text = block.Text;

			WordCounter.CountWords(
				text, out newWordCount, out newCharacterCount, out newNonWhitespaceCount);

			// Grab the existing counts from the current block, if we have one.
			int oldCount;
			int oldWordCount;
			int oldCharacterCount;
			int oldNonWhitespaceCount;

			WordCounterPathUtility.GetCounts(
				this,
				block,
				out oldCount,
				out oldWordCount,
				out oldCharacterCount,
				out oldNonWhitespaceCount);

			// Calculate the deltas between the values.
			int delta = newCount - oldCount;
			int wordDelta = newWordCount - oldWordCount;
			int characterDelta = newCharacterCount - oldCharacterCount;
			int nonWhitespaceDelta = newNonWhitespaceCount - oldNonWhitespaceCount;

			// Build up a dictionary of changes so we can have a simple loop to
			// set them in the various elements.
			Dictionary<HierarchicalPath, int> deltas = WordCounterPathUtility.GetDeltas(
				this, block, delta, wordDelta, characterDelta, nonWhitespaceDelta);

			// Get a write lock on the blocks list and update that block and all
			// parent blocks in the document.
			using (block.AcquireBlockLock(RequestLock.Write))
			{
				// Log that we are analyzing this block.
				Log("BEGIN AnalyzeBlock: {0}: Words {1:N0}", block, newWordCount);

				// First check to see if we've gotten stale.
				if (block.IsStale(blockVersion))
				{
					return;
				}

				// Update the block and the document.
				UpdateDeltas(block, deltas);
				UpdateDeltas(block.Project, deltas);

				// Log that we finished processing this block.
				Log("END   AnalyzeBlock: {0}: Words {1:N0}", block, newWordCount);
			}
		}

		public void ChangeBlockType(
			Block block,
			BlockType oldBlockType)
		{
			// We need a write lock on the blocks while we make this change.
			using (block.AcquireBlockLock(RequestLock.Write))
			{
				// Report what we're doing if we have logging on.
				Log("ChangeBlockType: {0}: Old Type {1}", block, oldBlockType);

				//// Figure out the deltas for this block.
				//var deltas = new Dictionary<HierarchicalPath, int>();
				//deltas[WordCounterPathUtility.GetPath(oldBlockType)] = -1;
				//deltas[WordCounterPathUtility.GetPath(block.BlockType)] = 1;

				//// Update the parent types.
				//UpdateDeltas(block, deltas);
				//UpdateDeltas(block.Project, deltas);
			}
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
		/// <param name="propertiesContainer">The block.</param>
		/// <param name="deltas">The deltas.</param>
		/// <param name="multiplier">The multiplier.</param>
		private void UpdateDeltas(
			IPropertiesContainer propertiesContainer,
			IDictionary<HierarchicalPath, int> deltas,
			int multiplier = 1)
		{
			foreach (HierarchicalPath path in deltas.Keys)
			{
				int delta = deltas[path] * multiplier;
				Log("  Update Delta: {0}: {1} += {2}", propertiesContainer, path, delta);
				propertiesContainer.Properties.AdditionOrAdd(path, delta);
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
