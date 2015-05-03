// <copyright file="LoadInternalSequenceRegionTests.cs" company="Moonfire Games">
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
	/// Tests the loading of a single buffer with an Internal region identified by
	/// a dynamic sequence.
	/// </summary>
	public class LoadInternalSequenceRegionTests
	{
		#region Public Methods and Operators

		/// <summary>
		/// Verifies initial state of the project buffer.
		/// </summary>
		[Fact]
		public void VerifyIntialProjectBuffer()
		{
			Project project = CreateProject();

			Assert.Equal(
				0,
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
				1,
				project.Regions.Count);
		}

		/// <summary>
		/// Verifies the state of the project.
		/// </summary>
		[Fact]
		public void VerifyProjectBuffer()
		{
			Project project = Setup();
			Region region1 = project.Regions["region-1"];

			Assert.Equal(
				1,
				project.Blocks.Count);
			Assert.Equal(
				BlockType.Region,
				project.Blocks[0].BlockType);
			Assert.Equal(
				region1,
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
				2,
				project.Regions.Count);

			Assert.True(
				project.Regions.ContainsKey("project"),
				"Cannot find the project region.");
			Assert.True(
				project.Regions.ContainsKey("region-1"),
				"Cannot find the region-1 region.");
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
			var regionLayout = new RegionLayout
			{
				Slug = "region-$(ContainerIndex:0)",
				HasContent = true,
				IsSequenced = true,
				SequenceBufferFormatFactory = new MarkdownBufferFormatFactory()
			};
			projectLayout.Add(regionLayout);

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
				"# Unknown Title [region-1]",
				string.Empty,
				"Text in region 1.");

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
