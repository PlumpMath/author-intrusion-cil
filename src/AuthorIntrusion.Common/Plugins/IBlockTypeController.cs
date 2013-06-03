// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Blocks;

namespace AuthorIntrusion.Common.Plugins
{
	/// <summary>
	/// Defines a controller that listens to changes to a block type change.
	/// </summary>
	public interface IBlockTypeController: IBlockAnalyzerController
	{
		#region Methods

		/// <summary>
		/// Indicates that a block changed its block type from oldBlockType into the
		/// currently assigned one.
		/// </summary>
		/// <param name="block">The block.</param>
		/// <param name="oldBlockType">Old type of the block.</param>
		void ChangeBlockType(
			Block block,
			BlockType oldBlockType);

		#endregion
	}
}
