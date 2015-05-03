// <copyright file="StoreExternalRegionWithInternalSequenceRegionsTests.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System.Collections.Generic;

using AuthorIntrusion.Buffers;
using AuthorIntrusion.IO;

using MfGames.HierarchicalPaths;

using Xunit;

namespace AuthorIntrusion.Tests.IO.MarkdownBufferFormatTests
{
	/// <summary>
	/// Tests various aspects of storing an external sequence region in Markdown.
	/// </summary>
	public class StoreExternalRegionWithInternalSequenceRegionsTests :
		MemoryPersistenceTestsBase
	{
		#region Fields

		/// <summary>
		/// Contains the persistence used to read in the file.
		/// </summary>
		private MemoryPersistence inputPersistence;

		/// <summary>
		/// Contains the context from the load process.
		/// </summary>
		private BufferStoreContext outputContext;

		/// <summary>
		/// Contains the persistence used to write out the results.
		/// </summary>
		private MemoryPersistence outputPersistence;

		/// <summary>
		/// Contains the loaded project for verification purposes.
		/// </summary>
		private Project project;

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Verifies the resulting output files.
		/// </summary>
		[Fact]
		public void VerifyOutputFiles()
		{
			Setup();

			Assert.Equal(
				2,
				outputPersistence.DataCount);
		}

		/// <summary>
		/// Verifies the contents of the project file.
		/// </summary>
		[Fact]
		public void VerifyProjectContents()
		{
			Setup();

			List<string> lines = outputPersistence.GetDataLines("/");

			AssertLines(
				lines,
				"---",
				"title: Testing",
				"---",
				string.Empty,
				"* [Regions](regions)");
		}

		/// <summary>
		/// Verifies the contents of the regions file.
		/// </summary>
		[Fact]
		public void VerifyRegions()
		{
			Setup();

			List<string> lines = outputPersistence.GetDataLines("/regions");

			AssertLines(
				lines,
				"---",
				"title: Regions",
				"---",
				string.Empty,
				"# Test 1 [regions/region-1]",
				string.Empty,
				"One Two Three.",
				string.Empty,
				"# Test 2 [regions/region-2]",
				string.Empty,
				"Four Five Six.");
		}

		#endregion

		#region Methods

		/// <summary>
		/// Sets up this instance.
		/// </summary>
		private void Setup()
		{
			// Create the test input.
			inputPersistence = new MemoryPersistence();
			inputPersistence.SetData(
				new HierarchicalPath("/"),
				"---",
				"title: Testing",
				"---",
				"* [Regions](regions)");
			inputPersistence.SetData(
				new HierarchicalPath("/regions"),
				"---",
				"title: Regions",
				"---",
				"# Test 1 [regions/region-1]",
				"One Two Three.",
				"# Test 2 [regions/region-2]",
				"Four Five Six.");

			// Set up the layout.
			var projectLayout = new RegionLayout
			{
				Name = "Project",
				Slug = "project",
				HasContent = false
			};
			var regionsLayout = new RegionLayout
			{
				Name = "Region",
				Slug = "regions",
				HasContent = false,
				IsExternal = true
			};
			var sequencedRegion = new RegionLayout
			{
				Name = "Sequenced Region",
				Slug = "regions/region-$(ContainerIndex:0)",
				HasContent = true,
				IsSequenced = true,
				IsExternal = false
			};

			projectLayout.Add(regionsLayout);
			regionsLayout.Add(sequencedRegion);

			// Create a new project with the given layout.
			project = new Project();
			project.ApplyLayout(projectLayout);

			// Create the format.
			var format = new MarkdownBufferFormat();

			// Parse the buffer lines.
			var inputContext = new BufferLoadContext(
				project,
				inputPersistence);

			format.LoadProject(inputContext);

			// Using the same project layout, we create a new persistence and
			// write out the results.
			outputPersistence = new MemoryPersistence();
			outputContext = new BufferStoreContext(
				project,
				outputPersistence);

			format.StoreProject(outputContext);
		}

		#endregion
	}
}
