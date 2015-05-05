// <copyright file="ProjectLayoutTests.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System;

using AuthorIntrusion.Buffers;

using MarkdownLog;

using Xunit;
using Xunit.Abstractions;

namespace AuthorIntrusion.Tests
{
	/// <summary>
	/// Tests functionality for applying layouts and the resulting regions.
	/// </summary>
	public class ProjectLayoutTests : TestsBase
	{
		#region Constructors and Destructors

		public ProjectLayoutTests(ITestOutputHelper output)
			: base(output)
		{
		}

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Tests applying a single region from the default.
		/// </summary>
		[Fact]
		public void ApplySingleInternalLayout()
		{
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

			// Assert the results.
			Assert.Equal(
				2,
				project.Regions.Count);
			Assert.True(
				project.Regions.ContainsKey("project"));
			Assert.True(
				project.Regions.ContainsKey("region-1"));

			Assert.Equal(
				1,
				project.Regions["project"].Blocks.Count);

			Assert.Equal(
				0,
				project.Regions["region-1"].Blocks.Count);

			// Write out the final state.
			var markdown = new MarkdownContainer();

			markdown.Append(new Header("Final Project State"));
			project.ToMarkdown(markdown);

			Console.WriteLine(markdown);
		}

		/// <summary>
		/// Tests applying two serial Internal regions.
		/// </summary>
		[Fact]
		public void ApplyTwoInternalLayout()
		{
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
			projectLayout.Add(
				new RegionLayout
				{
					Name = "Region 2",
					Slug = "region-2",
					HasContent = true
				});

			// Create a new project with the given layout.
			var project = new Project();
			project.ApplyLayout(projectLayout);

			// Assert the results.
			Assert.Equal(
				3,
				project.Regions.Count);
			Assert.True(
				project.Regions.ContainsKey("project"));
			Assert.True(
				project.Regions.ContainsKey("region-1"));
			Assert.True(
				project.Regions.ContainsKey("region-2"));

			Assert.Equal(
				2,
				project.Regions["project"].Blocks.Count);

			Assert.Equal(
				0,
				project.Regions["region-1"].Blocks.Count);

			Assert.Equal(
				0,
				project.Regions["region-2"].Blocks.Count);

			// Write out the final state.
			var markdown = new MarkdownContainer();

			markdown.Append(new Header("Final Project State"));
			project.ToMarkdown(markdown);

			Console.WriteLine(markdown);
		}

		/// <summary>
		/// Tests applying two nested Internal regions.
		/// </summary>
		[Fact]
		public void ApplyTwoNestedInternalLayout()
		{
			// Set up the layout.
			var projectLayout = new RegionLayout
			{
				Name = "Project",
				Slug = "project",
				HasContent = false
			};
			var regionLayout1 = new RegionLayout
			{
				Name = "Region 1",
				Slug = "region-1",
				HasContent = true
			};
			projectLayout.Add(regionLayout1);
			regionLayout1.Add(
				new RegionLayout
				{
					Name = "Region 2",
					Slug = "region-2",
					HasContent = true
				});

			// Create a new project with the given layout.
			var project = new Project();
			project.ApplyLayout(projectLayout);

			// Assert the results.
			Assert.Equal(
				3,
				project.Regions.Count);
			Assert.True(
				project.Regions.ContainsKey("project"));
			Assert.True(
				project.Regions.ContainsKey("region-1"));
			Assert.True(
				project.Regions.ContainsKey("region-2"));

			Assert.Equal(
				1,
				project.Regions["project"].Blocks.Count);

			Assert.Equal(
				1,
				project.Regions["region-1"].Blocks.Count);

			Assert.Equal(
				0,
				project.Regions["region-2"].Blocks.Count);

			// Write out the final state.
			var markdown = new MarkdownContainer();

			markdown.Append(new Header("Final Project State"));
			project.ToMarkdown(markdown);

			Console.WriteLine(markdown);
		}

		#endregion
	}
}
