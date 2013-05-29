// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

namespace AuthorIntrusion.Common
{
	/// <summary>
	/// BlockType uniquely identifies the various types of blocks inside a project.
	/// There are two categories of block types. System blocks are ones critical to
	/// the system and can be deleted. User blocks are customizable to organize
	/// a document.
	/// 
	/// In both cases, the bulk of the formatting and display is based on block types.
	/// </summary>
	public class BlockType
	{
		#region Properties

		/// <summary>
		/// Gets or sets a value indicating whether this instance is a system block
		/// type.
		/// </summary>
		public bool IsSystem { get; set; }

		/// <summary>
		/// Gets the Supervisor that handles the block types.
		/// </summary>
		public BlockTypeSupervisor Supervisor { get; private set; }

		/// <summary>
		/// Gets or sets the unique name for the block.
		/// </summary>
		public string Name { get; set; }

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="BlockType"/> class.
		/// </summary>
		/// <param name="supervisor">The Supervisor.</param>
		public BlockType(BlockTypeSupervisor supervisor)
		{
			Supervisor = supervisor;
		}

		#endregion
	}
}
