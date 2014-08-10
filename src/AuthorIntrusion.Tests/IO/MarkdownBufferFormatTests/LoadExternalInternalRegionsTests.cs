﻿// <copyright file="LoadExternalInternalRegionsTests.cs" company="Moonfire Games">
//     Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// MIT Licensed (http://opensource.org/licenses/MIT)
namespace AuthorIntrusion.Tests.IO.MarkdownBufferFormatTests
{
    using System.Collections.Generic;
    using System.Linq;

    using AuthorIntrusion.Buffers;
    using AuthorIntrusion.IO;

    using MfGames.HierarchicalPaths;

    using NUnit.Framework;

    /// <summary>
    /// Tests the loading of a single buffer with with two External regions in a sequence.
    /// </summary>
    [TestFixture]
    public class LoadExternalInternalRegionsTests
    {
        #region Public Methods and Operators

        /// <summary>
        /// Verifies initial state of the nested buffer.
        /// </summary>
        [Test]
        public void VerifyIntialNestedRegion()
        {
            Project project = this.CreateProject();
            Region region = project.Regions["nested"];

            Assert.AreEqual(
                0, 
                region.Blocks.Count, 
                "The number of blocks is unexpected.");
        }

        /// <summary>
        /// Verifies initial state of the project buffer.
        /// </summary>
        [Test]
        public void VerifyIntialProjectBuffer()
        {
            Project project = this.CreateProject();

            Assert.AreEqual(
                1, 
                project.Blocks.Count, 
                "The number of blocks is unexpected.");
        }

        /// <summary>
        /// Verifies the identification of sequenced containers within the layout.
        /// </summary>
        [Test]
        public void VerifyLayout()
        {
            Project project = this.CreateProject();

            List<RegionLayout> sequencedContainers =
                project.Layout.GetSequencedRegions().ToList();

            Assert.AreEqual(
                1, 
                sequencedContainers.Count, 
                "The number of sequenced containers is unexpected.");
            Assert.AreEqual(
                "region-$(ContainerIndex:0)", 
                sequencedContainers[0].Slug, 
                "The container slug is unexpected.");
            Assert.AreEqual(
                2, 
                project.Regions.Count, 
                "The number of regions was unexpected.");
        }

        /// <summary>
        /// Verifies the state of the nested region.
        /// </summary>
        [Test]
        public void VerifyNestedRegion()
        {
            Project project = this.Setup();
            Region nested = project.Regions["nested"];
            Region region1 = project.Regions["region-1"];
            Region region2 = project.Regions["region-2"];

            Assert.AreEqual(
                2, 
                nested.Blocks.Count, 
                "Number of lines in the project was unexpected.");

            Assert.AreEqual(
                BlockType.Region, 
                nested.Blocks[0].BlockType, 
                "The block type of project's 1st link block is unexpected.");
            Assert.AreEqual(
                region1, 
                nested.Blocks[0].LinkedRegion, 
                "The linked region of the 1st link type is unexpected.");

            Assert.AreEqual(
                BlockType.Region, 
                nested.Blocks[1].BlockType, 
                "The block type of project's 1st link block is unexpected.");
            Assert.AreEqual(
                region2, 
                nested.Blocks[1].LinkedRegion, 
                "The linked region of the 1st link type is unexpected.");
        }

        /// <summary>
        /// Verifies the state of the project.
        /// </summary>
        [Test]
        public void VerifyProjectBuffer()
        {
            Project project = this.Setup();
            Region nested = project.Regions["nested"];

            Assert.AreEqual(
                1, 
                project.Blocks.Count, 
                "Number of lines in the project was unexpected.");

            Assert.AreEqual(
                BlockType.Region, 
                project.Blocks[0].BlockType, 
                "The block type of project's 1st link block is unexpected.");
            Assert.AreEqual(
                nested, 
                project.Blocks[0].LinkedRegion, 
                "The linked region of the 1st link type is unexpected.");
        }

        /// <summary>
        /// Verifies the proper regions exist.
        /// </summary>
        [Test]
        public void VerifyProjectRegions()
        {
            Project project = this.Setup();

            Assert.AreEqual(
                4, 
                project.Regions.Count, 
                "The number of regions is unexpected.");

            Assert.IsTrue(
                project.Regions.ContainsKey("project"), 
                "Cannot find the project region.");
            Assert.IsTrue(
                project.Regions.ContainsKey("region-1"), 
                "Cannot find the region-1 region.");
            Assert.IsTrue(
                project.Regions.ContainsKey("region-2"), 
                "Cannot find the region-2 region.");
        }

        /// <summary>
        /// Verifies the state of region-1.
        /// </summary>
        [Test]
        public void VerifyRegion1()
        {
            Project project = this.Setup();
            Region region1 = project.Regions["region-1"];

            Assert.AreEqual(
                1, 
                region1.Blocks.Count, 
                "Number of lines in region 1 was unexpected.");
            Assert.AreEqual(
                "Text in region 1.", 
                region1.Blocks[0].Text, 
                "The text in 1st block was unexpected.");
        }

        /// <summary>
        /// Verifies the state of region-2.
        /// </summary>
        [Test]
        public void VerifyRegion2()
        {
            Project project = this.Setup();
            Region region2 = project.Regions["region-2"];

            Assert.AreEqual(
                2, 
                region2.Blocks.Count, 
                "Number of lines in region 1 was unexpected.");
            Assert.AreEqual(
                "Text in region 2.", 
                region2.Blocks[0].Text, 
                "The text in 1st block was unexpected.");
            Assert.AreEqual(
                "2nd text in region 2.", 
                region2.Blocks[1].Text, 
                "The text in 2nd block was unexpected.");
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
                    HasContent = false, 
                };
            var nestedLayout = new RegionLayout
                {
                    Name = "Nested", 
                    Slug = "nested", 
                    HasContent = false, 
                };
            var regionLayout = new RegionLayout
                {
                    Slug = "region-$(ContainerIndex:0)", 
                    HasContent = true, 
                    IsSequenced = true, 
                    SequenceBufferFormatFactory = new MarkdownBufferFormatFactory(), 
                };
            projectLayout.InnerLayouts.Add(nestedLayout);
            nestedLayout.InnerLayouts.Add(regionLayout);

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
            Project project = this.CreateProject();
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