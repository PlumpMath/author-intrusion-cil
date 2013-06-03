// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System;
using System.Threading;
using AuthorIntrusion.Common.Blocks;
using C5;
using MfGames.Locking;

namespace AuthorIntrusion.Common.Plugins
{
	public class BlockAnalyzer
	{
		#region Properties

		public Block Block { get; private set; }
		public IList<IBlockAnalyzerController> BlockAnalyzers { get; set; }
		public int BlockVersion { get; private set; }

		#endregion

		#region Methods

		public void Run()
		{
			// Loop through all the analyzers in the list and perform each one in turn.
			BlockOwnerCollection blocks = Block.Project.Blocks;

			foreach (IBlockAnalyzerController blockAnalyzer in BlockAnalyzers)
			{
				// Check to see if the block had gone stale.
				if (Block.IsStale(BlockVersion))
				{
					// The block is stale, so we just dump out since there will be
					// another task to reanalyze this block.
					return;
				}

				// Perform the analysis on the given block.
				blockAnalyzer.AnalyzeBlock(Block, BlockVersion);
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
			IList<IBlockAnalyzerController> blockAnalyzers)
		{
			Block = block;
			BlockVersion = blockVersion;
			BlockAnalyzers = blockAnalyzers;
		}

		#endregion
	}
}
