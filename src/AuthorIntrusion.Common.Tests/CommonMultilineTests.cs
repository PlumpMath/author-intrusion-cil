// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Blocks;
using AuthorIntrusion.Common.Commands;

namespace AuthorIntrusion.Common.Tests
{
	/// <summary>
	/// Common functionality when running multiline unit tests.
	/// </summary>
	public abstract class CommonMultilineTests
	{
		#region Methods

		/// <summary>
		/// Creates a project with a set number of lines and gives them a state that
		/// can easily be tested for.
		/// </summary>
		/// <param name="blocks">The block collection in the project.</param>
		/// <param name="blockTypes">The block types supervisor for the project.</param>
		/// <param name="commands">The commands supervisor for the project.</param>
		/// <param name="lineCount">The number of blocks to insert into the projects.</param>
		protected void SetupMultilineTest(
			out BlockOwnerCollection blocks,
			out BlockTypeSupervisor blockTypes,
			out BlockCommandSupervisor commands,
			int lineCount = 4)
		{
			// Everything is based on the project.
			var project = new Project();

			blocks = project.Blocks;
			commands = project.Commands;
			blockTypes = project.BlockTypes;

			// Set up the block structure.
			var chapterStructure = new BlockStructure
			{
				BlockType = blockTypes.Chapter
			};

			var sceneStructure = new BlockStructure()
			{
				BlockType = blockTypes.Scene,
				MinimumOccurances = 1,
			};

			chapterStructure.ChildStructures.Add(sceneStructure);
			project.BlockStructures.RootBlockStructure = chapterStructure;

			// Modify the first line, which is always there.
			blocks[0].Text = "Line 1";

			// Add in the additional lines after the first one.
			for (int i = 1;
				i < lineCount;
				i++)
			{
				blocks.Add(
					new Block(blocks)
					{
						BlockType = project.BlockTypes.Scene,
						Text = "Line " + (i + 1)
					});
			}
		}

		#endregion
	}
}
