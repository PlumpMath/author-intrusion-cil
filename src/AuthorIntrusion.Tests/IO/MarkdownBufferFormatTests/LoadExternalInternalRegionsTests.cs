// <copyright file="LoadExternalInternalRegionsTests.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System.Collections.Generic;
using System.Linq;

using AuthorIntrusion.Buffers;
using AuthorIntrusion.IO;

using MfGames.HierarchicalPaths;

using Xunit;

namespace AuthorIntrusion.Tests.IO.MarkdownBufferFormatTests
{
	/// <summary>
	/// Tests the loading of a single buffer with with two External regions in a sequence.
	/// </summary>
	public class LoadExternalInternalRegionsTests
	{
		#region Public Methods and Operators

		/// <summary>
		/// Verifies initial state of the nested buffer.
		/// </summary>
		[Fact]
		public void VerifyIntialNestedRegion()
		{
			Project project = CreateProject();
			Region region = project.Regions["nested"];

			Assert.Equal(
				0,
				region.Blocks.Count);
		}

		/// <summary>
		/// Verifies initial state of the project buffer.
		/// </summary>
		[Fact]
		public void VerifyIntialProjectBuffer()
		{
			Project project = CreateProject();

			Assert.Equal(
				1,
				project.Blocks.Count);
		}

		/// <summary>
		/// Verifies the identification of sequenced containers within the layout.
		/// </summary>
		[Fact]
		public void VerifyLayout()
		{
			Project project = CreateProject();

			List<RegionLayout> sequencedContainers =
				project.Layout.GetSequencedRegions()
					.ToList();

			Assert.Equal(
				1,
				sequencedContainers.Count);
			Assert.Equal(
				"region-$(ContainerIndex:0)",
				sequencedContainers[0].Slug);
			Assert.Equal(
				2,
				project.Regions.Count);
		}

		/// <summary>
		/// Verifies the state of the nested region.
		/// </summary>
		[Fact]
		public void VerifyNestedRegion()
		{
			Project project = Setup();
			Region nested = project.Regions["nested"];
			Region region1 = project.Regions["region-1"];
			Region region2 = project.Regions["region-2"];

			Assert.Equal(
				2,
				nested.Blocks.Count);

			Assert.Equal(
				BlockType.Region,
				nested.Blocks[0].BlockType);
			Assert.Equal(
				region1,
				nested.Blocks[0].LinkedRegion);

			Assert.Equal(
				BlockType.Region,
				nested.Blocks[1].BlockType);
			Assert.Equal(
				region2,
				nested.Blocks[1].LinkedRegion);
		}

		/// <summary>
		/// Verifies the state of the project.
		/// </summary>
		[Fact]
		public void VerifyProjectBuffer()
		{
			Project project = Setup();
			Region nested = project.Regions["nested"];

			Assert.Equal(
				1,
				project.Blocks.Count);

			Assert.Equal(
				BlockType.Region,
				project.Blocks[0].BlockType);
			Assert.Equal(
				nested,
				project.Blocks[0].LinkedRegion);
		}

		/// <summary>
		/// Verifies the proper regions exist.
		/// </summary>
		[Fact]
		public void VerifyProjectRegions()
		{
			Project project = Setup();

			Assert.Equal(
				4,
				project.Regions.Count);

			Assert.True(
				project.Regions.ContainsKey("project"));
			Assert.True(
				project.Regions.ContainsKey("region-1"));
			Assert.True(
				project.Regions.ContainsKey("region-2"));
		}

		/// <summary>
		/// Verifies the state of region-1.
		/// </summary>
		[Fact]
		public void VerifyRegion1()
		{
			Project project = Setup();
			Region region1 = project.Regions["region-1"];

			Assert.Equal(
				1,
				region1.Blocks.Count);
			Assert.Equal(
				"Text in region 1.",
				region1.Blocks[0].Text);
		}

		/// <summary>
		/// Verifies the state of region-2.
		/// </summary>
		[Fact]
		public void VerifyRegion2()
		{
			Project project = Setup();
			Region region2 = project.Regions["region-2"];

			Assert.Equal(
				2,
				region2.Blocks.Count);
			Assert.Equal(
				"Text in region 2.",
				region2.Blocks[0].Text);
			Assert.Equal(
				"2nd text in region 2.",
				region2.Blocks[1].Text);
		}

		#endregion

		#region Methods

		/// <summary>
		/// Creates the project with the appropriate layout.
		/// </summary>
		/// <returns>The created project.</returns>
		private Project CreateProject()
		{
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
				Slug = "region-$(ContainerIndex:0)",
				HasContent = true,
				IsSequenced = true,
				SequenceBufferFormatFactory = new MarkdownBufferFormatFactory()
			};
			projectLayout.Add(nestedLayout);
			nestedLayout.Add(regionLayout);

			// Create a new project with the given layout.
			var project = new Project();
			project.ApplyLayout(projectLayout);
			return project;
		}

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
				"* [Nested](nested)");
			persistence.SetData(
				new HierarchicalPath("/nested"),
				"# Region 1 [region-1]",
				"Text in region 1.",
				"# Region 2 [region-2]",
				"Text in region 2.",
				"2nd text in region 2.");

			// Create the format.
			var format = new MarkdownBufferFormat();

			// Parse the buffer lines.
			Project project = CreateProject();
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
