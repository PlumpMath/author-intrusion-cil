// <copyright file="BlockCollection.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System.Collections.Generic;
using System.Linq;

namespace AuthorIntrusion.Buffers
{
	/// <summary>
	/// Represents a sequence of blocks.
	/// </summary>
	public class BlockCollection : List<Block>
	{
		#region Public Properties

		/// <summary>
		/// Contains the number of link blocks in the region.
		/// </summary>
		public int LinkCount
		{
			get { return this.Count(b => b.BlockType == BlockType.Region); }
		}

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Gets the index of the region inside the linked blocks.
		/// </summary>
		/// <param name="region">
		/// The region.
		/// </param>
		/// <returns>
		/// The index of the region.
		/// </returns>
		public int GetContainerIndex(Region region)
		{
			List<Region> regions = ToArray()
				.Where(b => b.BlockType == BlockType.Region)
				.Select(b => b.LinkedRegion)
				.ToList();

			int index = regions.IndexOf(region);
			return index;
		}

		#endregion
	}
}
