// <copyright file="LoadNestedMetadataTests.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using AuthorIntrusion.Buffers;
using AuthorIntrusion.IO;

using MfGames.HierarchicalPaths;

using Xunit;

namespace AuthorIntrusion.Tests.IO.MarkdownBufferFormatTests
{
	/// <summary>
	/// Tests loading nested metdata from the file.
	/// </summary>
	public class LoadNestedMetadataTests
	{
		#region Public Methods and Operators

		/// <summary>
		/// Verifies the state of chapter 1.
		/// </summary>
		[Fact]
		public void VerifyChapter1Region()
		{
			Project project = Setup();
			Region chapterRegion = project.Regions["chapter-01"];
			Region sceneRegion1 = project.Regions["chapter-01/scene-001"];
			Region sceneRegion2 = project.Regions["chapter-01/scene-002"];

			Assert.Equal(
				2,
				chapterRegion.Blocks.Count);

			Assert.Equal(
				BlockType.Region,
				chapterRegion.Blocks[0].BlockType);
			Assert.Equal(
				sceneRegion1,
				chapterRegion.Blocks[0].LinkedRegion);

			Assert.Equal(
				BlockType.Region,
				chapterRegion.Blocks[1].BlockType);
			Assert.Equal(
				sceneRegion2,
				chapterRegion.Blocks[1].LinkedRegion);
		}

		/// <summary>
		/// Verifies the state of chapter 1, scene 1.
		/// </summary>
		[Fact]
		public void VerifyChapter1Scene1()
		{
			Project project = Setup();
			Region region1 = project.Regions["chapter-01/scene-001"];

			Assert.Equal(
				1,
				region1.Blocks.Count);
			Assert.Equal(
				"Text in chapter 1, scene 1.",
				region1.Blocks[0].Text);
		}

		/// <summary>
		/// Verifies the state of chapter 1, scene 2.
		/// </summary>
		[Fact]
		public void VerifyChapter1Scene2()
		{
			Project project = Setup();
			Region region1 = project.Regions["chapter-01/scene-002"];

			Assert.Equal(
				1,
				region1.Blocks.Count);
			Assert.Equal(
				"Text in chapter 1, scene 2.",
				region1.Blocks[0].Text);
		}

		/// <summary>
		/// Verifies the state of chapter 2.
		/// </summary>
		[Fact]
		public void VerifyChapter2Region()
		{
			Project project = Setup();
			Region chapterRegion = project.Regions["chapter-02"];
			Region sceneRegion1 = project.Regions["chapter-02/scene-003"];
			Region sceneRegion2 = project.Regions["chapter-02/scene-004"];

			Assert.Equal(
				2,
				chapterRegion.Blocks.Count);

			Assert.Equal(
				BlockType.Region,
				chapterRegion.Blocks[0].BlockType);
			Assert.Equal(
				sceneRegion1,
				chapterRegion.Blocks[0].LinkedRegion);

			Assert.Equal(
				BlockType.Region,
				chapterRegion.Blocks[1].BlockType);
			Assert.Equal(
				sceneRegion2,
				chapterRegion.Blocks[1].LinkedRegion);
		}

		/// <summary>
		/// Verifies the state of chapter 2, scene 1.
		/// </summary>
		[Fact]
		public void VerifyChapter2Scene1()
		{
			Project project = Setup();
			Region region1 = project.Regions["chapter-02/scene-003"];

			Assert.Equal(
				1,
				region1.Blocks.Count);
			Assert.Equal(
				"Text in chapter 2, scene 1.",
				region1.Blocks[0].Text);
		}

		/// <summary>
		/// Verifies the state of chapter 2, scene 2.
		/// </summary>
		[Fact]
		public void VerifyChapter2Scene2()
		{
			Project project = Setup();
			Region region1 = project.Regions["chapter-02/scene-004"];

			Assert.Equal(
				1,
				region1.Blocks.Count);
			Assert.Equal(
				"Text in chapter 2, scene 2.",
				region1.Blocks[0].Text);
		}

		/// <summary>
		/// Verifies the state of the project's region.
		/// </summary>
		[Fact]
		public void VerifyProject()
		{
			Project project = Setup();
			Region chapter1 = project.Regions["chapter-01"];
			Region chapter2 = project.Regions["chapter-02"];

			Assert.Equal(
				2,
				project.Blocks.Count);

			Assert.Equal(
				BlockType.Region,
				project.Blocks[0].BlockType);
			Assert.Equal(
				chapter1,
				project.Blocks[0].LinkedRegion);

			Assert.Equal(
				BlockType.Region,
				project.Blocks[1].BlockType);
			Assert.Equal(
				chapter2,
				project.Blocks[1].LinkedRegion);
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
				"# Chapter 1 [chapter-01]",
				"## Scene 1 [chapter-01/scene-001]",
				"Text in chapter 1, scene 1.",
				"## Scene 2 [chapter-01/scene-002]",
				"Text in chapter 1, scene 2.",
				"# Chapter 2 [chapter-02]",
				"## Scene 1 [chapter-02/scene-003]",
				"Text in chapter 2, scene 1.",
				"## Scene 2 [chapter-02/scene-004]",
				"Text in chapter 2, scene 2.");

			// Set up the layout.
			var projectLayout = new RegionLayout
			{
				Name = "Project",
				Slug = "project",
				HasContent = false
			};
			var chapterLayout = new RegionLayout
			{
				Name = "Chapters",
				Slug = "chapter-$(ContainerIndex:00)",
				HasContent = false,
				IsSequenced = true
			};
			var sceneLayout = new RegionLayout
			{
				Name = "Scenes",
				Slug = "$(ParentSlug)/scene-$(ProjectIndex:000)",
				HasContent = true,
				IsSequenced = true
			};
			projectLayout.Add(chapterLayout);
			chapterLayout.Add(sceneLayout);

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
