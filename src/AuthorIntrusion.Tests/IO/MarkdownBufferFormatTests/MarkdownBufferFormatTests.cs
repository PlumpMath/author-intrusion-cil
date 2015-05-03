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

using NUnit.Framework;

namespace AuthorIntrusion.Tests.IO.MarkdownBufferFormatTests
{
	/// <summary>
	/// Tests various parsing various input into the MarkdownBufferFormat.
	/// </summary>
	[TestFixture]
	public class MarkdownBufferFormatTests
	{
		#region Public Methods and Operators

		/// <summary>
		/// Tests reading a blank-line separated content.
		/// </summary>
		[Test]
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

			Assert.AreEqual(
				2,
				contents.Count,
				"Number of output lines was unexpected.");
			Assert.AreEqual(
				"One Two Three.",
				contents[0].Text,
				"1st output line was unexpected.");
			Assert.AreEqual(
				"Four Five Six.",
				contents[1].Text,
				"2nd output line was unexpected.");
		}

		/// <summary>
		/// Tests reading contents that have a leading blank line before the text.
		/// </summary>
		[Test]
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

			Assert.AreEqual(
				1,
				metadata.Count,
				"Number of metadata keys is unexpected.");
			Assert.IsTrue(
				metadata.ContainsKey(titleKey),
				"Could not find Scalar key in metadata.");
			Assert.AreEqual(
				"Unit Test",
				metadata[titleKey].Value,
				"Value of Scalar was unexpected.");

			// Verify the contents.
			BlockCollection contents = project.Blocks;

			Assert.AreEqual(
				1,
				contents.Count,
				"Number of output lines was unexpected.");
			Assert.AreEqual(
				"One Two Three.",
				contents[0].Text,
				"1st output line was unexpected.");
		}

		/// <summary>
		/// Tests reading a single external region.
		/// </summary>
		[Test]
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

			// Get the second region.
			Assert.AreEqual(
				1,
				region1.Blocks.Count,
				"Number of lines in region 1 was unexpected.");
			Assert.AreEqual(
				"Text in region 1.",
				region1.Blocks[0].Text,
				"The text in region 1 was unexpected.");
		}

		/// <summary>
		/// Tests reading input that has no metadata.
		/// </summary>
		[Test]
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
			Assert.AreEqual(
				0,
				metadata.Count,
				"Number of metadata keys is unexpected.");

			// Verify the contents.
			BlockCollection contents = project.Blocks;

			Assert.AreEqual(
				1,
				contents.Count,
				"Number of output lines was unexpected.");
			Assert.AreEqual(
				"One Two Three.",
				contents[0].Text,
				"1st output line was unexpected.");
		}

		/// <summary>
		/// Tests reading in a single line Markdown with a single metadata.
		/// </summary>
		[Test]
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

			Assert.AreEqual(
				1,
				metadata.Count,
				"Number of metadata keys is unexpected.");
			Assert.IsTrue(
				metadata.ContainsKey(titleKey),
				"Could not find Scalar key in metadata.");
			Assert.AreEqual(
				"Unit Test",
				metadata[titleKey].Value,
				"Value of Scalar was unexpected.");

			// Verify the contents.
			BlockCollection contents = project.Blocks;

			Assert.AreEqual(
				1,
				contents.Count,
				"Number of output lines was unexpected.");
			Assert.AreEqual(
				"One Two Three.",
				contents[0].Text,
				"1st output line was unexpected.");
		}

		/// <summary>
		/// Tests reading input with only an author.
		/// </summary>
		[Test]
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
			Assert.AreEqual(
				0,
				metadata.Count,
				"Number of metadata keys is unexpected.");

			// Verify the title.
			Assert.AreEqual(
				"Unit Test",
				project.Authors.PreferredName,
				"Primary name was unexpected.");
		}

		/// <summary>
		/// Tests reading input with only metadata.
		/// </summary>
		[Test]
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

			Assert.AreEqual(
				1,
				metadata.Count,
				"Number of metadata keys is unexpected.");
			Assert.IsTrue(
				metadata.ContainsKey(titleKey),
				"Could not find Scalar key in metadata.");
			Assert.AreEqual(
				"Unit Test",
				metadata[titleKey].Value,
				"Value of Scalar was unexpected.");

			// Verify the contents.
			BlockCollection contents = project.Blocks;

			Assert.AreEqual(
				0,
				contents.Count,
				"Number of output lines was unexpected.");
		}

		/// <summary>
		/// Tests reading input with only title.
		/// </summary>
		[Test]
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
			Assert.AreEqual(
				0,
				metadata.Count,
				"Number of metadata keys is unexpected.");

			// Verify the title.
			Assert.AreEqual(
				"Unit Test",
				project.Titles.Title,
				"Title was unexpected.");
		}

		#endregion
	}
}
