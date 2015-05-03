// <copyright file="RegionDictionary.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

using MfGames.Text;

namespace AuthorIntrusion.Buffers
{
	/// <summary>
	/// Encapslates the functionality for managing a collection of regions. The
	/// actual sequencing of the regions is done by the Region/Block pair.
	/// </summary>
	public class RegionDictionary : Dictionary<string, Region>
	{
		#region Public Methods and Operators

		/// <summary>
		/// Adds the specified region into the dictionary.
		/// </summary>
		/// <param name="region">
		/// The region.
		/// </param>
		public void Add(Region region)
		{
			this[region.Slug] = region;
		}

		/// <summary>
		/// Creates a region based on the specified layout.
		/// </summary>
		/// <param name="parentRegion">
		/// </param>
		/// <param name="layout">
		/// The layout.
		/// </param>
		/// <returns>
		/// The created region.
		/// </returns>
		public Region Create(
			Region parentRegion,
			RegionLayout layout)
		{
			// Figure out the indexes for the region.
			int containerIndex = parentRegion.Blocks.LinkCount + 1;
			int projectIndex = GetLayoutCount(layout) + 1;

			// Format the slugs and names.
			var macros = new MacroExpansion();
			var variables = new Dictionary<string, object>
			{
				{ "ContainerIndex", containerIndex },
				{ "ProjectIndex", projectIndex }
			};

			// Add in the parent slug, if we have one.
			if (layout.ParentLayout != null)
			{
				// We have a parent regular expression, so include that.
				var parentVariables = new Dictionary<string, object>
				{
					{ "ContainerIndex", parentRegion.ContainerIndex + 1 },
					{ "ProjectIndex", parentRegion.ProjectIndex + 1 }
				};
				string parentSlug = macros.Expand(
					parentRegion.Layout.Slug,
					parentVariables);
				variables["ParentSlug"] = parentSlug;
			}

			// Create a new region.
			var region = new Region
			{
				Slug = macros.Expand(
					layout.Slug,
					variables),
				Name = macros.Expand(
					layout.Name,
					variables),
				Layout = layout
			};

			// Add the region into the collection.
			this[region.Slug] = region;

			// Return the constructed region.
			return region;
		}

		/// <summary>
		/// Tries to retrieve the region via the name.
		/// </summary>
		/// <param name="name">
		/// The name.
		/// </param>
		/// <param name="region">
		/// The region.
		/// </param>
		/// <returns>
		/// True if the item is found, false if not.
		/// </returns>
		public bool TryGetName(
			string name,
			out Region region)
		{
			region = Values.ToList()
				.FirstOrDefault(r => r.Name == name);

			return region != null;
		}

		/// <summary>
		/// Tries to get the region by slug or name.
		/// </summary>
		/// <param name="slug">
		/// The slug.
		/// </param>
		/// <param name="name">
		/// The name.
		/// </param>
		/// <param name="region">
		/// The region.
		/// </param>
		/// <returns>
		/// True if the region was found, otherwise false.
		/// </returns>
		public bool TryGetSlugOrName(
			string slug,
			string name,
			out Region region)
		{
			// Start by trying to get it via slug.
			if (!string.IsNullOrWhiteSpace(slug))
			{
				bool foundSlug = TryGetValue(
					slug,
					out region);

				if (foundSlug)
				{
					return true;
				}
			}

			// Try to get it via name.
			if (!string.IsNullOrWhiteSpace(name))
			{
				bool foundName = TryGetName(
					name,
					out region);

				if (foundName)
				{
					return true;
				}
			}

			// Otherwise, we didn't find it.
			region = null;
			return false;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Gets the number of regions that have a given count.
		/// </summary>
		/// <param name="layout">
		/// The layout.
		/// </param>
		/// <returns>
		/// The total number of items.
		/// </returns>
		private int GetLayoutCount(RegionLayout layout)
		{
			// Establish our contracts.
			Contract.Requires(layout != null);

			// Get how many regions have this contract.
			int count = Values.Count(r => r.Layout == layout);

			return count;
		}

		#endregion
	}
}
