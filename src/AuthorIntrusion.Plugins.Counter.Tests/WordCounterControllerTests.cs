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
			SetupCorrectionPlugin(out blocks, out commands, out projectPlugin);

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
			SetupCorrectionPlugin(out blocks, out commands, out projectPlugin);
			SetupComplexMultilineTest(blocks.Project, 6);

			BlockTypeSupervisor blockTypes = blocks.Project.BlockTypes;

			// Act
			blocks.Project.Plugins.WaitForBlockAnalzyers();

			// Assert
			Project project = blocks.Project;

			Assert.AreEqual(1, project.Plugins.Controllers.Count);

			int index = 0;
			Assert.AreEqual(
				12, blocks[index].Properties.Get<int>(CounterPaths.WordCountPath));
			Assert.AreEqual(
				36, blocks[index].Properties.Get<int>(CounterPaths.CharacterCountPath));
			Assert.AreEqual(
				30, blocks[index].Properties.Get<int>(CounterPaths.NonWhitespaceCountPath));
			Assert.AreEqual(
				1,
				blocks[index].Properties.Get<int>(CounterPaths.GetPath(blockTypes.Chapter)));
			Assert.AreEqual(
				1, blocks[index].Properties.Get<int>(CounterPaths.GetPath(blockTypes.Scene)));
			Assert.AreEqual(
				1,
				blocks[index].Properties.Get<int>(CounterPaths.GetPath(blockTypes.Epigraph)));
			Assert.AreEqual(
				1,
				blocks[index].Properties.Get<int>(
					CounterPaths.GetPath(blockTypes.EpigraphAttribution)));
			Assert.AreEqual(
				2,
				blocks[index].Properties.Get<int>(
					CounterPaths.GetPath(blockTypes.Paragraph)));

			index++;
			Assert.AreEqual(
				10, blocks[index].Properties.Get<int>(CounterPaths.WordCountPath));
			Assert.AreEqual(
				30, blocks[index].Properties.Get<int>(CounterPaths.CharacterCountPath));
			Assert.AreEqual(
				25, blocks[index].Properties.Get<int>(CounterPaths.NonWhitespaceCountPath));
			Assert.AreEqual(
				1, blocks[index].Properties.Get<int>(CounterPaths.GetPath(blockTypes.Scene)));
			Assert.AreEqual(
				1,
				blocks[index].Properties.Get<int>(CounterPaths.GetPath(blockTypes.Epigraph)));
			Assert.AreEqual(
				1,
				blocks[index].Properties.Get<int>(
					CounterPaths.GetPath(blockTypes.EpigraphAttribution)));
			Assert.AreEqual(
				2,
				blocks[index].Properties.Get<int>(
					CounterPaths.GetPath(blockTypes.Paragraph)));
		}

		[Test]
		public void SimpleChange()
		{
			// Arrange
			ProjectBlockCollection blocks;
			BlockCommandSupervisor commands;
			WordCounterProjectPlugin projectPlugin;
			SetupCorrectionPlugin(out blocks, out commands, out projectPlugin);

			// Act
			commands.InsertText(blocks[0], 0, "Line 1");
			blocks.Project.Plugins.WaitForBlockAnalzyers();

			// Assert
			Project project = blocks.Project;

			Assert.AreEqual(1, project.Plugins.Controllers.Count);
			Assert.AreEqual(2, blocks[0].Properties.Get<int>(CounterPaths.WordCountPath));
			Assert.AreEqual(
				6, blocks[0].Properties.Get<int>(CounterPaths.CharacterCountPath));
			Assert.AreEqual(
				5, blocks[0].Properties.Get<int>(CounterPaths.NonWhitespaceCountPath));
			Assert.AreEqual(
				1,
				blocks[0].Properties.Get<int>(
					new HierarchicalPath("Paragraph", CounterPaths.BlockTypePath)));
		}

		/// <summary>
		/// Configures the environment to load the plugin manager and verify we
		/// have access to the ImmediateCorrectionPlugin.
		/// </summary>
		private void SetupCorrectionPlugin(
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
