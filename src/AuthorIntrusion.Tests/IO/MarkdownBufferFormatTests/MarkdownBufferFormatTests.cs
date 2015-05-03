// <copyright file="MarkdownBufferFormatTests.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using AuthorIntrusion.Buffers;
using AuthorIntrusion.IO;
using AuthorIntrusion.Metadata;

using MfGames.HierarchicalPaths;

using Xunit;

namespace AuthorIntrusion.Tests.IO.MarkdownBufferFormatTests
{
	/// <summary>
	/// Tests various parsing various input into the MarkdownBufferFormat.
	/// </summary>
	public class MarkdownBufferFormatTests
	{
		#region Public Methods and Operators

		/// <summary>
		/// Tests reading a blank-line separated content.
		/// </summary>
		[Fact]
		public void BlankSeparatedParagraphs()
		{
			// Create the test input.
			var persistence = new MemoryPersistence();
			persistence.SetData(
				new HierarchicalPath("/"),
				"One Two Three.",
				string.Empty,
				"Four Five Six.");

			// Create the format.
			var format = new MarkdownBufferFormat();

			// Parse the buffer lines.
			var project = new Project();
			var context = new BufferLoadContext(
				project,
				persistence);

			format.LoadProject(context);

			// Verify the contents.
			BlockCollection contents = project.Blocks;

			Assert.Equal(
				2,
				contents.Count);
			Assert.Equal(
				"One Two Three.",
				contents[0].Text);
			Assert.Equal(
				"Four Five Six.",
				contents[1].Text);
		}

		/// <summary>
		/// Tests reading contents that have a leading blank line before the text.
		/// </summary>
		[Fact]
		public void LeadingBlankLineYamlMetadata()
		{
			// Create the test input.
			var persistence = new MemoryPersistence();
			persistence.SetData(
				new HierarchicalPath("/"),
				"---",
				"Scalar: Unit Test",
				"---",
				string.Empty,
				"One Two Three.");

			// Create the format.
			var format = new MarkdownBufferFormat();

			// Parse the buffer lines.
			var project = new Project();
			MetadataDictionary metadata = project.Metadata;
			var context = new BufferLoadContext(
				project,
				persistence);

			format.LoadProject(context);

			// Verify the metadata.
			MetadataKey titleKey = project.MetadataManager["Scalar"];

			Assert.Equal(
				1,
				metadata.Count);
			Assert.True(
				metadata.ContainsKey(titleKey),
				"Could not find Scalar key in metadata.");
			Assert.Equal(
				"Unit Test",
				metadata[titleKey].Value);

			// Verify the contents.
			BlockCollection contents = project.Blocks;

			Assert.Equal(
				1,
				contents.Count);
			Assert.Equal(
				"One Two Three.",
				contents[0].Text);
		}

		/// <summary>
		/// Tests reading a single external region.
		/// </summary>
		[Fact]
		public void LoadExternalSingleRegion()
		{
			// Create the test input.
			var persistence = new MemoryPersistence();
			persistence.SetData(
				new HierarchicalPath("/"),
				"* [Region 1](region-1)");
			persistence.SetData(
				new HierarchicalPath("/region-1"),
				"---",
				"title: Region 1",
				"---",
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
					HasContent = true,
					IsExternal = true
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

			// Verify the contents of the project.
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

			// Get the second region.
			Assert.Equal(
				1,
				region1.Blocks.Count);
			Assert.Equal(
				"Text in region 1.",
				region1.Blocks[0].Text);
		}

		/// <summary>
		/// Tests reading input that has no metadata.
		/// </summary>
		[Fact]
		public void NoYamlMetadata()
		{
			// Create the test input.
			var persistence = new MemoryPersistence();
			persistence.SetData(
				new HierarchicalPath("/"),
				"One Two Three.");

			// Create the format.
			var format = new MarkdownBufferFormat();

			// Parse the buffer lines.
			var project = new Project();
			MetadataDictionary metadata = project.Metadata;
			var context = new BufferLoadContext(
				project,
				persistence);

			format.LoadProject(context);

			// Verify the metadata.
			Assert.Equal(
				0,
				metadata.Count);

			// Verify the contents.
			BlockCollection contents = project.Blocks;

			Assert.Equal(
				1,
				contents.Count);
			Assert.Equal(
				"One Two Three.",
				contents[0].Text);
		}

		/// <summary>
		/// Tests reading in a single line Markdown with a single metadata.
		/// </summary>
		[Fact]
		public void SimpleYamlMetadata()
		{
			// Create the test input.
			var persistence = new MemoryPersistence();
			persistence.SetData(
				new HierarchicalPath("/"),
				"---",
				"Scalar: Unit Test",
				"---",
				"One Two Three.");

			// Create the format.
			var format = new MarkdownBufferFormat();

			// Parse the buffer lines.
			var project = new Project();
			MetadataDictionary metadata = project.Metadata;
			var context = new BufferLoadContext(
				project,
				persistence);

			format.LoadProject(context);

			// Verify the metadata.
			MetadataKey titleKey = project.MetadataManager["Scalar"];

			Assert.Equal(
				1,
				metadata.Count);
			Assert.True(
				metadata.ContainsKey(titleKey));
			Assert.Equal(
				"Unit Test",
				metadata[titleKey].Value);

			// Verify the contents.
			BlockCollection contents = project.Blocks;

			Assert.Equal(
				1,
				contents.Count);
			Assert.Equal(
				"One Two Three.",
				contents[0].Text);
		}

		/// <summary>
		/// Tests reading input with only an author.
		/// </summary>
		[Fact]
		public void YamlAuthorOnly()
		{
			// Create the test input.
			var persistence = new MemoryPersistence();
			persistence.SetData(
				new HierarchicalPath("/"),
				"---",
				"Author: Unit Test",
				"---");

			// Create the format.
			var format = new MarkdownBufferFormat();

			// Parse the buffer lines.
			var project = new Project();
			MetadataDictionary metadata = project.Metadata;
			var context = new BufferLoadContext(
				project,
				persistence);

			format.LoadProject(context);

			// Verify the metadata.
			Assert.Equal(
				0,
				metadata.Count);

			// Verify the title.
			Assert.Equal(
				"Unit Test",
				project.Authors.PreferredName);
		}

		/// <summary>
		/// Tests reading input with only metadata.
		/// </summary>
		[Fact]
		public void YamlMetadataOnly()
		{
			// Create the test input.
			var persistence = new MemoryPersistence();
			persistence.SetData(
				new HierarchicalPath("/"),
				"---",
				"Scalar: Unit Test",
				"---");

			// Create the format.
			var format = new MarkdownBufferFormat();

			// Parse the buffer lines.
			var project = new Project();
			MetadataDictionary metadata = project.Metadata;
			var context = new BufferLoadContext(
				project,
				persistence);

			format.LoadProject(context);

			// Verify the metadata.
			MetadataKey titleKey = project.MetadataManager["Scalar"];

			Assert.Equal(
				1,
				metadata.Count);
			Assert.True(
				metadata.ContainsKey(titleKey));
			Assert.Equal(
				"Unit Test",
				metadata[titleKey].Value);

			// Verify the contents.
			BlockCollection contents = project.Blocks;

			Assert.Equal(
				0,
				contents.Count);
		}

		/// <summary>
		/// Tests reading input with only title.
		/// </summary>
		[Fact]
		public void YamlTitleOnly()
		{
			// Create the test input.
			var persistence = new MemoryPersistence();
			persistence.SetData(
				new HierarchicalPath("/"),
				"---",
				"Title: Unit Test",
				"---");

			// Create the format.
			var format = new MarkdownBufferFormat();

			// Parse the buffer lines.
			var project = new Project();
			MetadataDictionary metadata = project.Metadata;
			var context = new BufferLoadContext(
				project,
				persistence);

			format.LoadProject(context);

			// Verify the metadata.
			Assert.Equal(
				0,
				metadata.Count);

			// Verify the title.
			Assert.Equal(
				"Unit Test",
				project.Titles.Title);
		}

		#endregion
	}
}
