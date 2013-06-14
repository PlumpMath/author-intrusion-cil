// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System.IO;
using System.Runtime.CompilerServices;
using AuthorIntrusion.Common.Blocks;
using AuthorIntrusion.Common.Commands;
using AuthorIntrusion.Common.Persistence;
using AuthorIntrusion.Common.Plugins;
using MfGames.HierarchicalPaths;
using NUnit.Framework;

namespace AuthorIntrusion.Common.Tests
{
	[TestFixture]
	public class FilesystemPersistenceProjectPluginTests: CommonMultilineTests
	{
		#region Methods

		[Test]
		public void ActivatePlugin()
		{
			// Act
			ProjectBlockCollection blocks;
			BlockCommandSupervisor commands;
			PluginSupervisor plugins;
			FilesystemPersistenceProjectPlugin projectPlugin;
			SetupPlugin(out blocks, out commands, out plugins, out projectPlugin);

			// Assert
			Assert.AreEqual(2, plugins.Controllers.Count);
		}

		[Test]
		public void Write()
		{
			// Arrange: Cleanup any existing output file.
			DirectoryInfo testDirectory = PrepareTestDirectory();

			// Arrange: Set up the plugin.
			ProjectBlockCollection blocks;
			BlockCommandSupervisor commands;
			PluginSupervisor plugins;
			FilesystemPersistenceProjectPlugin projectPlugin;
			SetupPlugin(out blocks, out commands, out plugins, out projectPlugin);

			// Arrange: Create a project with some interesting data.
			SetupComplexMultilineTest(blocks.Project, 10);
			blocks.Project.BlockTypes.Add("Custom Type", false);
			blocks[0].Properties[new HierarchicalPath("/Test")] = "Custom Property";
			blocks[0].TextSpans.Add(new TextSpan(1, 3, null, null));
			blocks[0].SetText("Incor Wurd Onz");
			plugins.WaitForBlockAnalzyers();

			// Act
			projectPlugin.Settings.SetIndividualDirectoryLayout();
			projectPlugin.Save(testDirectory);

			// Assert
			string projectFilename = Path.Combine(
				testDirectory.FullName, "Project.aiproj");
			Assert.IsTrue(File.Exists(projectFilename));
		}

		[Test]
		public void WriteRead()
		{
			// Arrange: Cleanup any existing output file.
			DirectoryInfo testDirectory = PrepareTestDirectory();

			// Arrange: Set up the plugin.
			ProjectBlockCollection blocks;
			BlockCommandSupervisor commands;
			PluginSupervisor plugins;
			FilesystemPersistenceProjectPlugin projectPlugin;
			SetupPlugin(out blocks, out commands, out plugins, out projectPlugin);

			PersistenceManager persistenceManager = PersistenceManager.Instance;

			// Arrange: Create a project with some interesting data and write it out.
			SetupComplexMultilineTest(blocks.Project, 10);
			blocks.Project.BlockTypes.Add("Custom Type", false);
			Block block = blocks[0];
			block.Properties[new HierarchicalPath("/Test")] = "Custom Property";
			block.TextSpans.Add(new TextSpan(1, 3, null, null));
			block.SetText("Incor Wurd Onz");
			plugins.WaitForBlockAnalzyers();
			projectPlugin.Settings.SetIndividualDirectoryLayout();
			projectPlugin.Save(testDirectory);

			// Act
			var projectFile =
				new FileInfo(Path.Combine(testDirectory.FullName, "Project.aiproj"));
			Project project = persistenceManager.ReadProject(projectFile);

			// Assert: Block Types
			block = project.Blocks[0];

			BlockTypeSupervisor blockTypes = project.BlockTypes;
			blocks = project.Blocks;

			Assert.AreEqual(2, project.Plugins.Controllers.Count);
			Assert.NotNull(blockTypes["Custom Type"]);
			Assert.IsFalse(blockTypes["Custom Type"].IsStructural);

			// Assert: Block Structure
			Assert.AreEqual(
				blockTypes.Chapter, project.BlockStructures.RootBlockStructure.BlockType);
			Assert.AreEqual(
				blockTypes.Scene,
				project.BlockStructures.RootBlockStructure.ChildStructures[0].BlockType);
			Assert.AreEqual(
				blockTypes.Epigraph,
				project.BlockStructures.RootBlockStructure.ChildStructures[0]
					.ChildStructures[0].BlockType);
			Assert.AreEqual(
				blockTypes.EpigraphAttribution,
				project.BlockStructures.RootBlockStructure.ChildStructures[0]
					.ChildStructures[1].BlockType);
			Assert.AreEqual(
				blockTypes.Paragraph,
				project.BlockStructures.RootBlockStructure.ChildStructures[0]
					.ChildStructures[2].BlockType);

			// Assert: Blocks
			Assert.AreEqual(10, blocks.Count);

			Assert.AreEqual(blockTypes.Chapter, block.BlockType);
			Assert.AreEqual("Incor Wurd Onz", block.Text);

			Assert.AreEqual(blockTypes.Scene, blocks[1].BlockType);
			Assert.AreEqual("Line 2", blocks[1].Text);

			Assert.AreEqual(blockTypes.Epigraph, blocks[2].BlockType);
			Assert.AreEqual("Line 3", blocks[2].Text);

			Assert.AreEqual(blockTypes.EpigraphAttribution, blocks[3].BlockType);
			Assert.AreEqual("Line 4", blocks[3].Text);

			Assert.AreEqual(blockTypes.Paragraph, blocks[9].BlockType);
			Assert.AreEqual("Line 10", blocks[9].Text);

			// Assert: Verify content data.
			Assert.AreEqual(1, block.Properties.Count);
			Assert.AreEqual(
				"Custom Property", block.Properties[new HierarchicalPath("/Test")]);

			// Assert: Verify text spans.
			Assert.AreEqual(1, block.TextSpans.Count);

			Assert.AreEqual(1, block.TextSpans[0].StartTextIndex);
			Assert.AreEqual(3, block.TextSpans[0].StopTextIndex);
			Assert.IsNull(block.TextSpans[0].Controller);
			Assert.IsNull(block.TextSpans[0].Data);
		}

		/// <summary>
		/// Prepares the test directory including removing any existing data
		/// and creating the top-level.
		/// </summary>
		/// <param name="methodName">Name of the method.</param>
		/// <returns>A DirectoryInfo object of the created directory.</returns>
		private DirectoryInfo PrepareTestDirectory(
			[CallerMemberName] string methodName = "Unknown")
		{
			// Figure out the directory name.
			string pluginDirectory = Path.Combine(
				"Unit Tests", GetType().Name, methodName);

			// Clean it up if we have files already there.
			if (Directory.Exists(pluginDirectory))
			{
				Directory.Delete(pluginDirectory, true);
			}

			// Create the root directory for the plugin.
			var directory = new DirectoryInfo(pluginDirectory);
			directory.Create();

			// Return the directory so we can use it for paths.
			return directory;
		}

		/// <summary>
		/// Configures the environment to load the plugin manager and verify we
		/// have access to our plugin and projectPlugin.
		/// </summary>
		private void SetupPlugin(
			out ProjectBlockCollection blocks,
			out BlockCommandSupervisor commands,
			out PluginSupervisor plugins,
			out FilesystemPersistenceProjectPlugin projectPlugin)
		{
			// Start getting us a simple plugin manager.
			var persistencePlugin = new PersistenceFrameworkPlugin();
			var filesystemPlugin = new FilesystemPersistencePlugin();

			var pluginManager = new PluginManager(persistencePlugin, filesystemPlugin);

			PluginManager.Instance = pluginManager;
			PersistenceManager.Instance = new PersistenceManager(persistencePlugin);

			// Create a project and pull out the useful properties we'll use to
			// make changes.
			var project = new Project();

			blocks = project.Blocks;
			commands = project.Commands;
			plugins = project.Plugins;

			// Load in the plugins we'll be using in these tests.
			plugins.Add("Persistence Framework");
			plugins.Add("Filesystem Persistence");
			plugins.Add("Spelling Framework");
			plugins.Add("NHunspell");
			plugins.Add("Local Words");
			plugins.Add("Immediate Correction");

			// Pull out the projectPlugin for the correction and cast it (since we know
			// what type it is).
			ProjectPluginController pluginController = plugins.Controllers[1];
			projectPlugin =
				(FilesystemPersistenceProjectPlugin) pluginController.ProjectPlugin;
		}

		#endregion
	}
}
