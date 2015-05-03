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

using NUnit.Framework;

namespace AuthorIntrusion.Tests.IO.MarkdownBufferFormatTests
{
	/// <summary>
	/// Tests the loading of a single buffer with an Internal region identified by
	/// a dynamic sequence.
	/// </summary>
	[TestFixture]
	public class LoadInternalSequenceRegionTests
	{
		#region Public Methods and Operators

		/// <summary>
		/// Verifies initial state of the project buffer.
		/// </summary>
		[Test]
		public void VerifyIntialProjectBuffer()
		{
			Project project = CreateProject();

			Assert.AreEqual(
				0,
				project.Blocks.Count,
				"The number of blocks is unexpected.");
		}

		/// <summary>
		/// Verifies the identification of sequenced containers within the layout.
		/// </summary>
		[Test]
		public void VerifyLayout()
		{
			Project project = CreateProject();

			List<RegionLayout> sequencedContainers =
				project.Layout.GetSequencedRegions()
					.ToList();

			Assert.AreEqual(
				1,
				sequencedContainers.Count,
				"The number of sequenced containers is unexpected.");
			Assert.AreEqual(
				"region-$(ContainerIndex:0)",
				sequencedContainers[0].Slug,
				"The container slug is unexpected.");
			Assert.AreEqual(
				1,
				project.Regions.Count,
				"The number of regions was unexpected.");
		}

		/// <summary>
		/// Verifies the state of the project.
		/// </summary>
		[Test]
		public void VerifyProjectBuffer()
		{
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
		/// Verifies the proper regions exist.
		/// </summary>
		[Test]
		public void VerifyProjectRegions()
		{
			Project project = Setup();

			Assert.AreEqual(
				2,
				project.Regions.Count,
				"The number of regions is unexpected.");

			Assert.IsTrue(
				project.Regions.ContainsKey("project"),
				"Cannot find the project region.");
			Assert.IsTrue(
				project.Regions.ContainsKey("region-1"),
				"Cannot find the region-1 region.");
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
