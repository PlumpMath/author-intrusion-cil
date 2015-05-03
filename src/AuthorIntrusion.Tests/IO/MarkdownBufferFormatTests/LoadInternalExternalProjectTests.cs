// <copyright file="LoadInternalExternalProjectTests.cs" company="Moonfire Games">
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
	/// Tests loading a single file that has an Internal region with external regions.
	/// </summary>
	[TestFixture]
	public class LoadInternalExternalProjectTests
	{
		#region Public Methods and Operators

		/// <summary>
		/// Verifies the state of the project's region.
		/// </summary>
		[Test]
		public void VerifyNestedRegion()
		{
			Project project = Setup();
			Region nestedRegion = project.Regions["nested"];
			Region region1 = project.Regions["region-1"];

			Assert.AreEqual(
				1,
				nestedRegion.Blocks.Count,
				"Number of lines in the project was unexpected.");
			Assert.AreEqual(
				BlockType.Region,
				nestedRegion.Blocks[0].BlockType,
				"The block type of project's link block is unexpected.");
			Assert.AreEqual(
				region1,
				nestedRegion.Blocks[0].LinkedRegion,
				"The linked region of the link type is unexpected.");
		}

		/// <summary>
		/// Verifies the state of the project's region.
		/// </summary>
		[Test]
		public void VerifyProject()
		{
			Project project = Setup();
			Region nestedRegion = project.Regions["nested"];

			Assert.AreEqual(
				1,
				project.Blocks.Count,
				"Number of lines in the project was unexpected.");
			Assert.AreEqual(
				BlockType.Region,
				project.Blocks[0].BlockType,
				"The block type of project's link block is unexpected.");
			Assert.AreEqual(
				nestedRegion,
				project.Blocks[0].LinkedRegion,
				"The linked region of the link type is unexpected.");
		}

		/// <summary>
		/// Verifies the state of the project's region.
		/// </summary>
		[Test]
		public void VerifyRegion()
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
		/// Tests reading a single nested Internal region.
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
				"# Nested",
				"* [Region 1](region-1)");
			persistence.SetData(
				new HierarchicalPath("/region-1"),
				"Text in region 1.");

			// Set up the layout.
			var projectLayout = new RegionLayout
			{
				Name = "Project",
				Slug = "project",
				HasContent = false
			};
			var nestedLayout = new RegionLayout
			{
				Name = "Nested",
				Slug = "nested",
				HasContent = false
			};
			var regionLayout = new RegionLayout
			{
				Name = "Region 1",
				Slug = "region-1",
				HasContent = true,
				IsExternal = true
			};
			projectLayout.Add(nestedLayout);
			nestedLayout.Add(regionLayout);

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

			// Return the resulting project.
			return project;
		}

		#endregion
	}
}
