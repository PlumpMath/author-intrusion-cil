// <copyright file="StoreSimpleProjectTests.cs" company="Moonfire Games">
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
	/// Tests various aspects of storing a single Markdown file.
	/// </summary>
	public class StoreSimpleProjectTests : MemoryPersistenceTestsBase
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
				1,
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
				"One Two Three.");
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
				"One Two Three.");

			// Set up the layout.
			var projectLayout = new RegionLayout
			{
				Name = "Project",
				Slug = "project",
				HasContent = true
			};

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
