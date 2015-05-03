// <copyright file="Region.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using AuthorIntrusion.IO;
using AuthorIntrusion.Metadata;

using MfGames.HierarchicalPaths;

namespace AuthorIntrusion.Buffers
{
	/// <summary>
	/// Encapsulates the logic of a token buffer for a single file in the project. This
	/// contains logic for loading and unloading of data, reordering, and translating
	/// requests from the ProjectSequenceBuffer into individual buffer operations.
	/// </summary>
	public class Region : IProjectBuffer
	{
		#region Constructors and Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Region"/> class.
		/// </summary>
		public Region()
		{
			Metadata = new MetadataDictionary();
			Blocks = new BlockCollection();
			Authors = new NameDictionary();
			Titles = new TitleInfo();
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets the authors of the project.
		/// </summary>
		/// <value>
		/// The names.
		/// </value>
		public NameDictionary Authors { get; private set; }

		/// <summary>
		/// Gets the blocks associated directly with the project buffer.
		/// </summary>
		/// <value>
		/// The blocks.
		/// </value>
		public BlockCollection Blocks { get; private set; }

		/// <summary>
		/// Gets the index of this region within it's container container.
		/// </summary>
		/// <value>
		/// The index of the container.
		/// </value>
		public int ContainerIndex
		{
			get
			{
				if (ParentRegion == null)
				{
					return 0;
				}

				return ParentRegion.Blocks.GetContainerIndex(this);
			}
		}

		/// <summary>
		/// Gets or sets the layout.
		/// </summary>
		/// <value>
		/// The layout.
		/// </value>
		public RegionLayout Layout { get; set; }

		/// <summary>
		/// Gets the metadata associated with the project.
		/// </summary>
		/// <value>
		/// The metadata.
		/// </value>
		public MetadataDictionary Metadata { get; private set; }

		/// <summary>
		/// Gets the name of the region.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the parent region for this region.
		/// </summary>
		/// <value>
		/// The parent region.
		/// </value>
		public Region ParentRegion { get; set; }

		/// <summary>
		/// Gets the path associated with this region.
		/// </summary>
		/// <value>
		/// The path.
		/// </value>
		public HierarchicalPath Path
		{
			get { return new HierarchicalPath("/" + Slug); }
		}

		/// <summary>
		/// Gets the index of the region within the entire project.
		/// </summary>
		/// <value>
		/// The index of the project.
		/// </value>
		public int ProjectIndex { get { return ContainerIndex; } }

		/// <summary>
		/// Gets the slug associated with the project.
		/// </summary>
		/// <value>
		/// The slug.
		/// </value>
		public string Slug { get; set; }

		/// <summary>
		/// Gets the titles of the project.
		/// </summary>
		/// <value>
		/// The titles.
		/// </value>
		public TitleInfo Titles { get; private set; }

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return string.Format(
				"Region({0})",
				Slug);
		}

		#endregion
	}
}
