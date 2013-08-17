// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System.Collections.Generic;
using AuthorIntrusion.Common.Blocks;
using AuthorIntrusion.Common.Blocks.Locking;

namespace AuthorIntrusion.Common.Plugins
{
	public class BlockAnalyzer
	{
		#region Properties

		public HashSet<IBlockAnalyzerProjectPlugin> Analysis { get; private set; }
		public Block Block { get; private set; }
		public IList<IBlockAnalyzerProjectPlugin> BlockAnalyzers { get; private set; }
		public int BlockVersion { get; private set; }

		#endregion

		#region Methods

		public void Run()
		{
			// Figure out which analyzers we need to actually run on the block.
			var neededAnalyzers = new List<IBlockAnalyzerProjectPlugin>();

			foreach (IBlockAnalyzerProjectPlugin blockAnalyzer in BlockAnalyzers)
			{
				if (!Analysis.Contains(blockAnalyzer))
				{
					neededAnalyzers.Add(blockAnalyzer);
				}
			}

			// Loop through all the analyzers in the list and perform each one in turn.
			ProjectBlockCollection blocks = Block.Project.Blocks;

			foreach (IBlockAnalyzerProjectPlugin blockAnalyzer in neededAnalyzers)
			{
				// Check to see if the block had gone stale.
				using (blocks.AcquireBlockLock(RequestLock.Read, Block))
				{
					if (Block.IsStale(BlockVersion))
					{
						// The block is stale, so we just dump out since there will be
						// another task to reanalyze this block.
						return;
					}
				}

				// Perform the analysis on the given block.
				blockAnalyzer.AnalyzeBlock(Block, BlockVersion);

				// Once we're done analyzing the block, we need to add this
				// analyzer to the list so we don't attempt to run it again.
				Block.AddAnalysis(blockAnalyzer);
			}
		}

		public override string ToString()
		{
			return string.Format(
				"BlockAnalyzer (BlockKey {0}, Version {1})",
				Block.BlockKey.Id.ToString("X8"),
				BlockVersion);
		}

		#endregion

		#region Constructors

		public BlockAnalyzer(
			Block block,
			int blockVersion,
			IList<IBlockAnalyzerProjectPlugin> blockAnalyzers,
			HashSet<IBlockAnalyzerProjectPlugin> analysis)
		{
			Block = block;
			BlockVersion = blockVersion;
			BlockAnalyzers = blockAnalyzers;
			Analysis = analysis;
		}

		#endregion
	}
}
