// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System;
using AuthorIntrusion.Common;
using AuthorIntrusion.Common.Blocks;
using AuthorIntrusion.Common.Commands;
using AuthorIntrusion.Common.Plugins;
using AuthorIntrusion.Common.Tests;
using MfGames.HierarchicalPaths;
using NUnit.Framework;

namespace AuthorIntrusion.Plugins.Counter.Tests
{
	[TestFixture]
	public class WordCounterControllerTests: CommonMultilineTests
	{
		#region Methods

		[Test]
		public void ActivatePlugin()
		{
			// Act
			ProjectBlockCollection blocks;
			BlockCommandSupervisor commands;
			WordCounterProjectPlugin projectPlugin;
			SetupPlugin(out blocks, out commands, out projectPlugin);

			// Assert
			Project project = blocks.Project;

			Assert.AreEqual(1, project.Plugins.Controllers.Count);
		}

		[Test]
		public void CountComplexSetup()
		{
			// Arrange
			ProjectBlockCollection blocks;
			BlockCommandSupervisor commands;
			WordCounterProjectPlugin projectPlugin;
			SetupPlugin(out blocks, out commands, out projectPlugin);
			SetupComplexMultilineTest(blocks.Project, 6);

			BlockTypeSupervisor blockTypes = blocks.Project.BlockTypes;

			// Act
			blocks.Project.Plugins.WaitForBlockAnalzyers();

			// Assert
			Project project = blocks.Project;

			Assert.AreEqual(1, project.Plugins.Controllers.Count);

			//int index = 0;
			//Assert.AreEqual(
			//	12, blocks[index].Properties.Get<int>(WordCounterPathUtility.WordCountPath));
			//Assert.AreEqual(
			//	36, blocks[index].Properties.Get<int>(WordCounterPathUtility.CharacterCountPath));
			//Assert.AreEqual(
			//	30, blocks[index].Properties.Get<int>(WordCounterPathUtility.NonWhitespaceCountPath));
			//Assert.AreEqual(
			//	1,
			//	blocks[index].Properties.Get<int>(WordCounterPathUtility.GetPath(blockTypes.Chapter)));
			//Assert.AreEqual(
			//	1, blocks[index].Properties.Get<int>(WordCounterPathUtility.GetPath(blockTypes.Scene)));
			//Assert.AreEqual(
			//	1,
			//	blocks[index].Properties.Get<int>(WordCounterPathUtility.GetPath(blockTypes.Epigraph)));
			//Assert.AreEqual(
			//	1,
			//	blocks[index].Properties.Get<int>(
			//		WordCounterPathUtility.GetPath(blockTypes.EpigraphAttribution)));
			//Assert.AreEqual(
			//	2,
			//	blocks[index].Properties.Get<int>(
			//		WordCounterPathUtility.GetPath(blockTypes.Paragraph)));

			//index++;
			//Assert.AreEqual(
			//	10, blocks[index].Properties.Get<int>(WordCounterPathUtility.WordCountPath));
			//Assert.AreEqual(
			//	30, blocks[index].Properties.Get<int>(WordCounterPathUtility.CharacterCountPath));
			//Assert.AreEqual(
			//	25, blocks[index].Properties.Get<int>(WordCounterPathUtility.NonWhitespaceCountPath));
			//Assert.AreEqual(
			//	1, blocks[index].Properties.Get<int>(WordCounterPathUtility.GetPath(blockTypes.Scene)));
			//Assert.AreEqual(
			//	1,
			//	blocks[index].Properties.Get<int>(WordCounterPathUtility.GetPath(blockTypes.Epigraph)));
			//Assert.AreEqual(
			//	1,
			//	blocks[index].Properties.Get<int>(
			//		WordCounterPathUtility.GetPath(blockTypes.EpigraphAttribution)));
			//Assert.AreEqual(
			//	2,
			//	blocks[index].Properties.Get<int>(
			//		WordCounterPathUtility.GetPath(blockTypes.Paragraph)));
		}

		[Test]
		public void SingleBlockTwoWords()
		{
			// Arrange
			ProjectBlockCollection blocks;
			BlockCommandSupervisor commands;
			WordCounterProjectPlugin projectPlugin;
			SetupPlugin(out blocks, out commands, out projectPlugin);

			// Act
			commands.InsertText(blocks[0], 0, "Line 1");
			blocks.Project.Plugins.WaitForBlockAnalzyers();

			// Assert
			var path = new HierarchicalPath("/Plugins/Word Counter");
			var total = new HierarchicalPath("Total", path);
			var paragraph = new HierarchicalPath("Block Types/Paragraph", path);
			Project project = blocks.Project;
			PropertiesDictionary blockProperties = blocks[0].Properties;
			PropertiesDictionary projectProperties = project.Properties;

			Assert.AreEqual(1, project.Plugins.Controllers.Count);
			Assert.AreEqual(2, blockProperties.Get<int>("Words", total));
			Assert.AreEqual(6, blockProperties.Get<int>("Characters", total));
			Assert.AreEqual(5, blockProperties.Get<int>("Non-Whitespace", total));
			Assert.AreEqual(1, blockProperties.Get<int>("Whitespace", total));
			Assert.AreEqual(1, blockProperties.Get<int>("Count", paragraph));
			Assert.AreEqual(2, blockProperties.Get<int>("Words", paragraph));
			Assert.AreEqual(6, blockProperties.Get<int>("Characters", paragraph));
			Assert.AreEqual(5, blockProperties.Get<int>("Non-Whitespace", paragraph));
			Assert.AreEqual(1, blockProperties.Get<int>("Whitespace", paragraph));

			Assert.AreEqual(2, projectProperties.Get<int>("Words", total));
			Assert.AreEqual(6, projectProperties.Get<int>("Characters", total));
			Assert.AreEqual(5, projectProperties.Get<int>("Non-Whitespace", total));
			Assert.AreEqual(1, projectProperties.Get<int>("Whitespace", total));
			Assert.AreEqual(1, projectProperties.Get<int>("Count", paragraph));
			Assert.AreEqual(2, projectProperties.Get<int>("Words", paragraph));
			Assert.AreEqual(6, projectProperties.Get<int>("Characters", paragraph));
			Assert.AreEqual(5, projectProperties.Get<int>("Non-Whitespace", paragraph));
			Assert.AreEqual(1, projectProperties.Get<int>("Whitespace", paragraph));
		}

		[Test]
		public void ChangeSingleBlockTwoWords()
		{
			// Arrange
			ProjectBlockCollection blocks;
			BlockCommandSupervisor commands;
			WordCounterProjectPlugin projectPlugin;
			SetupPlugin(out blocks,out commands,out projectPlugin);

			// Arrange: Initial insert
			commands.InsertText(blocks[0],0,"Line 1");
			blocks.Project.Plugins.WaitForBlockAnalzyers();

			// Act
			commands.InsertText(blocks[0],0,"One ");
			blocks.Project.Plugins.WaitForBlockAnalzyers();

			// Assert
			var path = new HierarchicalPath("/Plugins/Word Counter");
			var total = new HierarchicalPath("Total",path);
			var paragraph = new HierarchicalPath("Block Types/Paragraph",path);
			Project project = blocks.Project;
			PropertiesDictionary blockProperties = blocks[0].Properties;
			PropertiesDictionary projectProperties = project.Properties;

			Assert.AreEqual(1,project.Plugins.Controllers.Count);

			Assert.AreEqual(3,blockProperties.Get<int>("Words",total));
			Assert.AreEqual(10,blockProperties.Get<int>("Characters",total));
			Assert.AreEqual(8,blockProperties.Get<int>("Non-Whitespace",total));
			Assert.AreEqual(2,blockProperties.Get<int>("Whitespace",total));
			Assert.AreEqual(1,blockProperties.Get<int>("Count",paragraph));
			Assert.AreEqual(3,blockProperties.Get<int>("Words",paragraph));
			Assert.AreEqual(10,blockProperties.Get<int>("Characters",paragraph));
			Assert.AreEqual(8,blockProperties.Get<int>("Non-Whitespace",paragraph));
			Assert.AreEqual(2,blockProperties.Get<int>("Whitespace",paragraph));

			Assert.AreEqual(3,projectProperties.Get<int>("Words",total));
			Assert.AreEqual(10,projectProperties.Get<int>("Characters",total));
			Assert.AreEqual(8,projectProperties.Get<int>("Non-Whitespace",total));
			Assert.AreEqual(2,projectProperties.Get<int>("Whitespace",total));
			Assert.AreEqual(1,projectProperties.Get<int>("Count",paragraph));
			Assert.AreEqual(3,projectProperties.Get<int>("Words",paragraph));
			Assert.AreEqual(10,projectProperties.Get<int>("Characters",paragraph));
			Assert.AreEqual(8,projectProperties.Get<int>("Non-Whitespace",paragraph));
			Assert.AreEqual(2,projectProperties.Get<int>("Whitespace",paragraph));
		}

		[Test]
		public void InsertTwoBlocksFourWords()
		{
			// Arrange
			ProjectBlockCollection blocks;
			BlockCommandSupervisor commands;
			WordCounterProjectPlugin projectPlugin;
			SetupPlugin(out blocks,out commands,out projectPlugin);

			// Arrange: Initial insert
			commands.InsertText(blocks[0],0,"Line 1");
			blocks.Project.Plugins.WaitForBlockAnalzyers();

			blocks.Add(new Block(blocks));

			// Act
			commands.InsertText(blocks[1],0,"Line 2");
			blocks.Project.Plugins.WaitForBlockAnalzyers();

			// Assert
			var path = new HierarchicalPath("/Plugins/Word Counter");
			var total = new HierarchicalPath("Total",path);
			var paragraph = new HierarchicalPath("Block Types/Paragraph",path);
			Project project = blocks.Project;
			PropertiesDictionary blockProperties = blocks[0].Properties;
			PropertiesDictionary projectProperties = project.Properties;

			Assert.AreEqual(2,blockProperties.Get<int>("Words",total));
			Assert.AreEqual(6,blockProperties.Get<int>("Characters",total));
			Assert.AreEqual(5,blockProperties.Get<int>("Non-Whitespace",total));
			Assert.AreEqual(1,blockProperties.Get<int>("Whitespace",total));
			Assert.AreEqual(1,blockProperties.Get<int>("Count",paragraph));
			Assert.AreEqual(2,blockProperties.Get<int>("Words",paragraph));
			Assert.AreEqual(6,blockProperties.Get<int>("Characters",paragraph));
			Assert.AreEqual(5,blockProperties.Get<int>("Non-Whitespace",paragraph));
			Assert.AreEqual(1,blockProperties.Get<int>("Whitespace",paragraph));

			Assert.AreEqual(4,projectProperties.Get<int>("Words",total));
			Assert.AreEqual(12,projectProperties.Get<int>("Characters",total));
			Assert.AreEqual(10,projectProperties.Get<int>("Non-Whitespace",total));
			Assert.AreEqual(2,projectProperties.Get<int>("Whitespace",total));
			Assert.AreEqual(2,projectProperties.Get<int>("Count",paragraph));
			Assert.AreEqual(4,projectProperties.Get<int>("Words",paragraph));
			Assert.AreEqual(12,projectProperties.Get<int>("Characters",paragraph));
			Assert.AreEqual(10,projectProperties.Get<int>("Non-Whitespace",paragraph));
			Assert.AreEqual(2,projectProperties.Get<int>("Whitespace",paragraph));
		}

		/// <summary>
		/// Configures the environment to load the plugin manager and verify we
		/// have access to the ImmediateCorrectionPlugin.
		/// </summary>
		private void SetupPlugin(
			out ProjectBlockCollection blocks,
			out BlockCommandSupervisor commands,
			out WordCounterProjectPlugin projectPlugin)
		{
			// Start getting us a simple plugin manager.
			var plugin = new WordCounterPlugin();
			var pluginManager = new PluginManager(plugin);

			PluginManager.Instance = pluginManager;

			// Create a project and pull out the useful properties we'll use to
			// make changes.
			var project = new Project();

			blocks = project.Blocks;
			commands = project.Commands;

			// Load in the immediate correction editor.
			if (!project.Plugins.Add("Word Counter"))
			{
				// We couldn't load it for some reason.
				throw new ApplicationException("Cannot load word counter plugin.");
			}

			// Pull out the controller for the correction and cast it (since we know
			// what type it is).
			ProjectPluginController pluginController = project.Plugins.Controllers[0];
			projectPlugin = (WordCounterProjectPlugin) pluginController.ProjectPlugin;

			// Set up logging for the controller.
			WordCounterProjectPlugin.Logger = Console.WriteLine;
		}

		#endregion
	}
}
