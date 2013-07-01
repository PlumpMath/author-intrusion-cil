// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Blocks;

namespace AuthorIntrusion.Common.Plugins
{
	/// <summary>
	/// Defines a controller process that analyzes a block in a background thread.
	/// </summary>
	public interface IBlockAnalyzerProjectPlugin: IProjectPlugin
	{
		#region Methods

		/// <summary>
		/// Analyzes the block and makes changes as needed.
		/// </summary>
		/// <param name="block">The block to analyze.</param>
		/// <param name="blockVersion">The block version of the initial request.</param>
		void AnalyzeBlock(
			Block block,
			int blockVersion);

		#endregion
	}
}
