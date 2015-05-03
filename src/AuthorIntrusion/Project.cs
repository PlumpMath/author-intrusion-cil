// <copyright file="Project.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System.Collections.Generic;

using AuthorIntrusion.Buffers;
using AuthorIntrusion.Metadata;

using MarkdownLog;

namespace AuthorIntrusion
{
	/// <summary>
	/// Primary organizational unit for a writing project. This manages all of
	/// the internals of the project including access to the buffer for editing.
	/// </summary>
	public class Project : Region
	{
		#region Constructors and Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Project"/> class.
		/// </summary>
		public Project()
		{
			// Create the collections.
			Singletons = new SingletonManager();
			MetadataManager = new MetadataManager();
			Regions = new RegionDictionary();

			// Hardcode the project layout to be a story.
			Layout = new RegionLayout
			{
				Name = "Project",
				Slug = "project",
				HasContent = true,
				IsSequenced = false
			};
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets the metadata manager for the project.
		/// </summary>
		/// <value>
		/// The metadata manager.
		/// </value>
		public MetadataManager MetadataManager { get; private set; }

		/// <summary>
		/// Gets the regions associated with the project.
		/// </summary>
		/// <value>
		/// The regions.
		/// </value>
		public RegionDictionary Regions { get; private set; }

		/// <summary>
		/// Gets the singleton manager used for managing keys such as
		/// class names and metadata.
		/// </summary>
		public SingletonManager Singletons { get; private set; }

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Applies the layout to the project, moving as much as possible (based on
		/// slugs), removing unused regions, and creating new regions.
		/// </summary>
		/// <param name="rootLayout">
		/// The root layout.
		/// </param>
		public void ApplyLayout(RegionLayout rootLayout)
		{
			// Get a clone of the existing regions in the project so we can
			// reference it.
			var oldRegions = new Dictionary<string, Region>(Regions);

			// Clear out the old regions and rebuild all of the entries that we actually need.
			Regions.Clear();
			CreateRegion(
				rootLayout,
				oldRegions);

			// Save the layout.
			Layout = rootLayout;
		}

		/// <summary>
		/// Converts the region information into a bulleted list.
		/// </summary>
		/// <param name="markdown">
		/// The markdown.
		/// </param>
		public void ToMarkdown(MarkdownContainer markdown)
		{
			markdown.Append(new BulletedList("Title: " + Titles));
		}

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return Titles.ToString();
		}

		#endregion

		#region Methods

		/// <summary>
		/// Creates a new region based on the current layout.
		/// </summary>
		/// <param name="layout">
		/// The layout.
		/// </param>
		/// <param name="oldRegions">
		/// The old regions.
		/// </param>
		/// <returns>
		/// The created region or null if one cannot be created.
		/// </returns>
		private Region CreateRegion(
			RegionLayout layout,
			Dictionary<string, Region> oldRegions)
		{
			// We don't do anything with sequenced regions.
			if (layout.IsSequenced)
			{
				return null;
			}

			// See if we have an existing region for this name.
			string regionSlug = layout.Slug;
			Region region;

			if (regionSlug == "project")
			{
				region = this;
			}
			else if (!oldRegions.TryGetValue(
				regionSlug,
				out region))
			{
				// Create a new region for this slug.
				region = new Region
				{
					Slug = regionSlug
				};
			}

			// If the region doesn't have content, then just remove everything.
			if (!layout.HasContent)
			{
				region.Blocks.Clear();
			}

			// We already had this region, so just copy it over but remove the
			// existing links because we'll be rebuilding it.
			region.Blocks.RemoveAll(r => r.BlockType == BlockType.Region);

			// Assign the new flags to the region.
			region.Name = layout.Name;
			region.Slug = layout.Slug;
			region.Layout = layout;

			// Add the region into the new region collection.
			Regions.Add(region);

			// Loop through and create all the child regions.
			foreach (RegionLayout childLayout in layout.InnerLayouts)
			{
				// Create the child region recursively.
				Region childRegion = CreateRegion(
					childLayout,
					oldRegions);

				// If we got a null, then this is a sequenced child and we don't
				// want to add the links.
				if (childRegion == null)
				{
					continue;
				}

				// Add the block to the end of the list.
				var block = new Block
				{
					BlockType = BlockType.Region,
					LinkedRegion = childRegion
				};
				childRegion.ParentRegion = region;

				region.Blocks.Add(block);
			}

			// Return the resulting region.
			return region;
		}

		#endregion
	}
}
