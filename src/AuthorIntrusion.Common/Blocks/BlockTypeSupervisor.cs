// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using C5;

namespace AuthorIntrusion.Common.Blocks
{
	/// <summary>
	/// A supervisor class that manages block types for a given project.
	/// </summary>
	public class BlockTypeSupervisor
	{
		#region Properties

		/// <summary>
		/// Gets the <see cref="BlockType"/> with the specified block type name.
		/// </summary>
		/// <value>
		/// The <see cref="BlockType"/>.
		/// </value>
		/// <param name="blockTypeName">Name of the block type.</param>
		/// <returns>The associated block type.</returns>
		public BlockType this[string blockTypeName]
		{
			get { return BlockTypes[blockTypeName]; }
		}

		public BlockType Chapter
		{
			get { return this[ChapterName]; }
		}

		public BlockType Paragraph
		{
			get { return this[ParagraphName]; }
		}

		/// <summary>
		/// Gets or sets the project associated with this block type Supervisor.
		/// </summary>
		public Project Project { get; private set; }

		public BlockType Scene
		{
			get { return this[SceneName]; }
		}

		/// <summary>
		/// Gets the block types associated with this Supervisor.
		/// </summary>
		protected HashDictionary<string, BlockType> BlockTypes { get; private set; }

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="BlockTypeSupervisor"/> class.
		/// </summary>
		/// <param name="project">The project.</param>
		public BlockTypeSupervisor(Project project)
		{
			// Save the project so we can associated the Supervisor with its project.
			Project = project;

			// Create the standard project block types.
			var paragraph = new BlockType(this)
			{
				Name = ParagraphName,
				IsSystem = true
			};
			var chapter = new BlockType(this)
			{
				Name = ChapterName,
				IsSystem = true
			};
			var scene = new BlockType(this)
			{
				Name = SceneName,
				IsSystem = true
			};

			// Initialize the collection of block types.
			BlockTypes = new HashDictionary<string, BlockType>();
			BlockTypes[ParagraphName] = paragraph;
			BlockTypes[SceneName] = scene;
			BlockTypes[ChapterName] = chapter;
		}

		#endregion

		#region Fields

		public const string ChapterName = "Chapter";

		public const string ParagraphName = "Paragraph";
		public const string SceneName = "Scene";

		#endregion
	}
}
