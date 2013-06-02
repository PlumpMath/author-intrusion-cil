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

		protected void SetupComplexMultilineTest(
			out BlockOwnerCollection blocks,
			out BlockTypeSupervisor blockTypes,
			out BlockCommandSupervisor commands,
			int lineCount = 10)
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

			var sceneStructure = new BlockStructure
			{
				BlockType = blockTypes.Scene,
				MinimumOccurances = 1,
			};

			var epigraphStructure = new BlockStructure
			{
				BlockType = blockTypes.Epigraph,
				MinimumOccurances = 1,
				MaximumOccurances = 1,
			};

			var epigraphAttributationStructure = new BlockStructure
			{
				BlockType = blockTypes.EpigraphAttribution,
				MinimumOccurances = 1,
				MaximumOccurances = 1,
			};

			var paragraphStructure = new BlockStructure
			{
				BlockType = blockTypes.Paragraph,
				MinimumOccurances = 1,
			};

			sceneStructure.ChildStructures.Add(epigraphStructure);
			sceneStructure.ChildStructures.Add(epigraphAttributationStructure);
			sceneStructure.ChildStructures.Add(paragraphStructure);
			chapterStructure.ChildStructures.Add(sceneStructure);
			project.BlockStructures.RootBlockStructure = chapterStructure;

			// Insert the bulk of the lines.
			InsertLines(project, lineCount);
		}

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

			var sceneStructure = new BlockStructure
			{
				BlockType = blockTypes.Scene,
				MinimumOccurances = 1,
			};

			chapterStructure.ChildStructures.Add(sceneStructure);
			project.BlockStructures.RootBlockStructure = chapterStructure;

			// Insert the bulk of the lines.
			InsertLines(project, lineCount);

			// Go through and set up the block types for these elements.
			project.Blocks[0].BlockType = blockTypes.Chapter;

			for (int index = 1;
				index < project.Blocks.Count;
				index++)
			{
				project.Blocks[index].BlockType = blockTypes.Scene;
			}
		}

		/// <summary>
		/// Inserts the lines.
		/// </summary>
		/// <param name="project">The project.</param>
		/// <param name="lineCount">The line count.</param>
		private void InsertLines(
			Project project,
			int lineCount)
		{
			// Pull out some useful variables.
			BlockOwnerCollection blocks = project.Blocks;

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
						Text = "Line " + (i + 1)
					});
			}
		}

		#endregion
	}
}
