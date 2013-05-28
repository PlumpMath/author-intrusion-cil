// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using C5;

namespace AuthorIntrusion.Common
{
	/// <summary>
	/// A manager class that manages block types for a given project.
	/// </summary>
	public class BlockTypeManager
	{
		#region Properties

		/// <summary>
		/// Gets or sets the project associated with this block type manager.
		/// </summary>
		public Project Project { get; private set; }

		/// <summary>
		/// Gets the block types associated with this manager.
		/// </summary>
		protected HashDictionary<string, BlockType> BlockTypes { get; private set; }

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="BlockTypeManager"/> class.
		/// </summary>
		/// <param name="project">The project.</param>
		public BlockTypeManager(Project project)
		{
			// Save the project so we can associated the manager with its project.
			Project = project;

			// Create the standard project block types.
			var paragraph = new BlockType(this)
			{
				Name = "Paragraph",
				IsSystem = true
			};

			// Initialize the collection of block types.
			BlockTypes = new HashDictionary<string, BlockType>();
			BlockTypes[paragraph.Name] = paragraph;
		}

		#endregion
	}
}
