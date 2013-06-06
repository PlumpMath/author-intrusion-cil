// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Blocks;

namespace AuthorIntrusion.Common.Plugins
{
	/// <summary>
	/// Defines a controller that manages block relationships.
	/// </summary>
	public interface IBlockRelationshipProjectPlugin: IBlockAnalyzerProjectPlugin
	{
		#region Methods

		/// <summary>
		/// Indicates that a block changed its parent from oldParentBlock to the
		/// current block.Parent.
		/// </summary>
		/// <param name="block">The block.</param>
		/// <param name="oldParentBlock">The old parent block.</param>
		void ChangeBlockParent(
			Block block,
			Block oldParentBlock);

		#endregion
	}
}
