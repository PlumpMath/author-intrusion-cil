﻿// <copyright file="StoreProjectIndexedequencedRegionsTests.cs" company="Moonfire Games">
//     Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// MIT Licensed (http://opensource.org/licenses/MIT)
namespace AuthorIntrusion.Tests.IO.MarkdownBufferFormatTests
{
    using System.Collections.Generic;

    using AuthorIntrusion.Buffers;
    using AuthorIntrusion.IO;

    using MfGames.HierarchicalPaths;

    using NUnit.Framework;

    /// <summary>
    /// Tests writing out a series of files with nested sequences and project indexes.
    /// </summary>
    [TestFixture]
    public class StoreProjectIndexedequencedRegionsTests : MemoryPersistenceTestsBase
    {
        #region Fields

        /// <summary>
        /// Contains the output context from storing.
        /// </summary>
        private BufferStoreContext outputContext;

        /// <summary>
        /// Contains the resulting persistence results after the store.
        /// </summary>
        private MemoryPersistence outputPersistence;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Verifies the contents of the chapter-01.
        /// </summary>
        [Test]
        public void VerifyChapter1()
        {
            this.Setup();

            List<string> lines =
                this.outputPersistence.GetDataLines("/chapter-01");

            this.AssertLines(
                lines, 
                "---", 
                "title: Chapter 1", 
                "---", 
                string.Empty, 
                "1. [Scenes](chapter-01/scene-001)", 
                "2. [Scenes](chapter-01/scene-002)");
        }

        /// <summary>
        /// Verifies the contents of the chapter-01/scene-001.
        /// </summary>
        [Test]
        public void VerifyChapter1Scene1()
        {
            this.Setup();

            List<string> lines =
                this.outputPersistence.GetDataLines("/chapter-01/scene-001");

            this.AssertLines(
                lines, 
                "Text in chapter 1, scene 1.");
        }

        /// <summary>
        /// Verifies the contents of the chapter-01/scene-002.
        /// </summary>
        [Test]
        public void VerifyChapter1Scene2()
        {
            this.Setup();

            List<string> lines =
                this.outputPersistence.GetDataLines("/chapter-01/scene-002");

            this.AssertLines(
                lines, 
                "Text in chapter 1, scene 2.");
        }

        /// <summary>
        /// Verifies the contents of the chapter-02.
        /// </summary>
        [Test]
        public void VerifyChapter2()
        {
            this.Setup();

            List<string> lines =
                this.outputPersistence.GetDataLines("/chapter-02");

            this.AssertLines(
                lines, 
                "---", 
                "title: Chapter 2", 
                "---", 
                string.Empty, 
                "1. [Scenes](chapter-02/scene-003)", 
                "2. [Scenes](chapter-02/scene-004)");
        }

        /// <summary>
        /// Verifies the contents of the chapter-02/scene-001.
        /// </summary>
        [Test]
        public void VerifyChapter2Scene1()
        {
            this.Setup();

            List<string> lines =
                this.outputPersistence.GetDataLines("/chapter-02/scene-003");

            this.AssertLines(
                lines, 
                "Text in chapter 2, scene 1.");
        }

        /// <summary>
        /// Verifies the contents of the chapter-02/scene-002.
        /// </summary>
        [Test]
        public void VerifyChapter2Scene2()
        {
            this.Setup();

            List<string> lines =
                this.outputPersistence.GetDataLines("/chapter-02/scene-004");

            this.AssertLines(
                lines, 
                "Text in chapter 2, scene 2.");
        }

        /// <summary>
        /// Verifies the resulting output files.
        /// </summary>
        [Test]
        public void VerifyOutputFiles()
        {
            this.Setup();

            Assert.AreEqual(
                7, 
                this.outputPersistence.DataCount, 
                "The number of output files was unexpected.");
        }

        /// <summary>
        /// Verifies the contents of the project file.
        /// </summary>
        [Test]
        public void VerifyProjectContents()
        {
            this.Setup();

            List<string> lines = this.outputPersistence.GetDataLines("/");

            this.AssertLines(
                lines, 
                "1. [Chapter 1](chapter-01)", 
                "2. [Chapter 2](chapter-02)");
        }

        #endregion

        #region Methods

        /// <summary>
        /// Tests reading a single nested Internal region.
        /// </summary>
        private void Setup()
        {
            // Create the test input.
            var persistence = new MemoryPersistence();
            persistence.SetData(
                new HierarchicalPath("/"),
                "* [Chapter 1](chapter-01)",
                "* [Chapter 2](chapter-02)");
            persistence.SetData(
                new HierarchicalPath("/chapter-01"),
                "---",
                "title: Chapter 1",
                "---",
                "* [Scene 1](chapter-01/scene-001)",
                "* [Scene 2](chapter-01/scene-002)");
            persistence.SetData(
                new HierarchicalPath("/chapter-02"),
                "---",
                "title: Chapter 2",
                "---",
                "* [Scene 1](chapter-02/scene-003)",
                "* [Scene 2](chapter-02/scene-004)");
            persistence.SetData(
                new HierarchicalPath("/chapter-01/scene-001"),
                "Text in chapter 1, scene 1.");
            persistence.SetData(
                new HierarchicalPath("/chapter-01/scene-002"),
                "Text in chapter 1, scene 2.");
            persistence.SetData(
                new HierarchicalPath("/chapter-02/scene-003"),
                "Text in chapter 2, scene 1.");
            persistence.SetData(
                new HierarchicalPath("/chapter-02/scene-004"),
                "Text in chapter 2, scene 2.");

            // Set up the layout.
            var projectLayout = new RegionLayout
            {
                Name = "Project",
                Slug = "project",
                HasContent = false,
            };
            var chapterLayout = new RegionLayout
            {
                Name = "Chapters",
                Slug = "chapter-$(ContainerIndex:00)",
                HasContent = false,
                IsExternal = true,
                IsSequenced = true,
            };
            var sceneLayout = new RegionLayout
            {
                Name = "Scenes",
                Slug = "$(ParentSlug)/scene-$(ProjectIndex:000)",
                HasContent = true,
                IsExternal = true,
                IsSequenced = true,
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

            // Using the same project layout, we create a new persistence and
            // write out the results.
            this.outputPersistence = new MemoryPersistence();
            this.outputContext = new BufferStoreContext(
                project, 
                this.outputPersistence);

            format.StoreProject(this.outputContext);
        }

        #endregion
    }
}