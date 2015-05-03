// <copyright file="LoadInternalRegionWithWrongTitleTests.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using AuthorIntrusion.Buffers;
using AuthorIntrusion.IO;

using MfGames.HierarchicalPaths;

using NUnit.Framework;

namespace AuthorIntrusion.Tests.IO.MarkdownBufferFormatTests
{
	/// <summary>
	/// Tests the loading of a single buffer with a single Internal region that has
	/// an identifier but an unknown title.
	/// </summary>
	[TestFixture]
	public class LoadInternalRegionWithWrongTitleTests
	{
		#region Public Methods and Operators

		/// <summary>
		/// Verifies the state of the project.
		/// </summary>
		[Test]
		public void VerifyProjectBuffer()
		{
			// Prepare the test.
			Project project = Setup();
			Region region1 = project.Regions["region-1"];

			Assert.AreEqual(
				1,
				project.Blocks.Count,
				"Number of lines in the project was unexpected.");
			Assert.AreEqual(
				BlockType.Region,
				project.Blocks[0].BlockType,
				"The block type of project's link block is unexpected.");
			Assert.AreEqual(
				region1,
				project.Blocks[0].LinkedRegion,
				"The linked region of the link type is unexpected.");
		}

		/// <summary>
		/// Verifies the state of region-1.
		/// </summary>
		[Test]
		public void VerifyRegion1()
		{
			Project project = Setup();
			Region region1 = project.Regions["region-1"];

			Assert.AreEqual(
				1,
				region1.Blocks.Count,
				"Number of lines in region 1 was unexpected.");
			Assert.AreEqual(
				"Text in region 1.",
				region1.Blocks[0].Text,
				"The text in region 1 was unexpected.");
		}

		#endregion

		#region Methods

		/// <summary>
		/// Sets up the unit test.
		/// </summary>
		/// <returns>
		/// The loaded project.
		/// </returns>
		private Project Setup()
		{
			// Create the test input.
			var persistence = new MemoryPersistence();
			persistence.SetData(
				new HierarchicalPath("/"),
				"# Unknown Title [region-1]",
				string.Empty,
				"Text in region 1.");

			// Set up the layout.
			var projectLayout = new RegionLayout
			{
				Name = "Project",
				Slug = "project",
				HasContent = false
			};
			projectLayout.Add(
				new RegionLayout
				{
					Name = "Region 1",
					Slug = "region-1",
					HasContent = true
				});

			// Create a new project with the given layout.
			var project = new Project();
			project.ApplyLayout(projectLayout);

			// Create the format.
			var format = new MarkdownBufferFormat();

			// Parse the buffer lines.
			var context = new BufferLoadContext(
				project,
				persistence);

			format.LoadProject(context);

			// Return the project.
			return project;
		}

		#endregion
	}
}
