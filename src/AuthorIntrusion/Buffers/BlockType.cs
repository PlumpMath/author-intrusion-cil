// <copyright file="BlockType.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

namespace AuthorIntrusion.Buffers
{
	/// <summary>
	/// Identifies the types of blocks used within the system.
	/// </summary>
	public enum BlockType
	{
		/// <summary>
		/// Indicates that the block is a content or text-only block. The Text
		/// property will have the text of the block.
		/// </summary>
		Text,

		/// <summary>
		/// The block has a link to another region which may or may not be in a
		/// separate file.
		/// </summary>
		Region
	}
}
