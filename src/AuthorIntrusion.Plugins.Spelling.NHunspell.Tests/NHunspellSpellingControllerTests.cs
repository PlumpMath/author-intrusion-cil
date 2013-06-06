﻿// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System;
using AuthorIntrusion.Common;
using AuthorIntrusion.Common.Actions;
using AuthorIntrusion.Common.Blocks;
using AuthorIntrusion.Common.Commands;
using AuthorIntrusion.Common.Plugins;
using C5;
using NUnit.Framework;

namespace AuthorIntrusion.Plugins.Spelling.NHunspell.Tests
{
	public class NHunspellSpellingControllerTests
	{
		#region Methods

		[Test]
		public void ActivatePlugin()
		{
			// Act
			BlockOwnerCollection blocks;
			BlockCommandSupervisor commands;
			PluginSupervisor plugins;
			NHunspellSpellingController controller;
			SetupPlugin(out blocks, out commands, out plugins, out controller);

			// Assert
			Project project = blocks.Project;

			Assert.AreEqual(2, project.Plugins.Controllers.Count);
		}

		[Test]
		public void CheckCorrectWord()
		{
			// Arrange
			BlockOwnerCollection blocks;
			BlockCommandSupervisor commands;
			PluginSupervisor plugins;
			NHunspellSpellingController controller;
			SetupPlugin(out blocks, out commands, out plugins, out controller);

			// Act
			commands.InsertText(blocks[0], 0, "Correct.");
			plugins.WaitForBlockAnalzyers();

			// Assert
			Project project = blocks.Project;

			Assert.AreEqual(2, project.Plugins.Controllers.Count);
		}

		[Test]
		public void CheckIncorrectActions()
		{
			// Arrange
			BlockOwnerCollection blocks;
			BlockCommandSupervisor commands;
			PluginSupervisor plugins;
			NHunspellSpellingController controller;
			SetupPlugin(out blocks, out commands, out plugins, out controller);

			// Arrange: Edit the text
			Block block = blocks[0];

			commands.InsertText(block, 0, "Correc.");
			plugins.WaitForBlockAnalzyers();

			// Act: Get the editor actions.
			TextSpan textSpan = block.TextSpans[0];

			IList<IEditorAction> actions = plugins.GetEditorActions(block, textSpan);

			// Assert
			Assert.AreEqual(6, actions.Count);
			Assert.AreEqual("Change to \"Correct\"", actions[1].DisplayName);
		}

		[Test]
		public void CheckIncorrectSelected()
		{
			// Arrange
			BlockOwnerCollection blocks;
			BlockCommandSupervisor commands;
			PluginSupervisor plugins;
			NHunspellSpellingController controller;
			SetupPlugin(out blocks, out commands, out plugins, out controller);

			// Arrange: Edit the text
			Block block = blocks[0];

			commands.InsertText(block, 0, "Correc.");
			plugins.WaitForBlockAnalzyers();

			// Arrange: Get the editor actions.
			TextSpan textSpan = block.TextSpans[0];

			IList<IEditorAction> actions = plugins.GetEditorActions(block, textSpan);

			// Act
			actions[1].Do();
			plugins.WaitForBlockAnalzyers();

			// Assert
			Assert.AreEqual("Correct.", block.Text);
		}

		[Test]
		public void CheckIncorrectSelectedUndo()
		{
			// Arrange
			BlockOwnerCollection blocks;
			BlockCommandSupervisor commands;
			PluginSupervisor plugins;
			NHunspellSpellingController controller;
			SetupPlugin(out blocks, out commands, out plugins, out controller);

			// Arrange: Edit the text
			Block block = blocks[0];

			commands.InsertText(block, 0, "Correc.");
			plugins.WaitForBlockAnalzyers();

			// Arrange: Get the editor actions.
			TextSpan textSpan = block.TextSpans[0];

			IList<IEditorAction> actions = plugins.GetEditorActions(block, textSpan);

			// Arrange: Perform the first edit.
			actions[1].Do();
			plugins.WaitForBlockAnalzyers();

			// Act
			commands.Undo();

			// Assert
			Assert.AreEqual("Correc.", block.Text);
		}

		[Test]
		public void CheckIncorrectTextSpan()
		{
			// Arrange
			BlockOwnerCollection blocks;
			BlockCommandSupervisor commands;
			PluginSupervisor plugins;
			NHunspellSpellingController controller;
			SetupPlugin(out blocks, out commands, out plugins, out controller);

			// Act
			commands.InsertText(blocks[0], 0, "Correc.");
			plugins.WaitForBlockAnalzyers();

			// Assert: TextSpans created
			Assert.AreEqual(1, blocks[0].TextSpans.Count);

			// Assert: TextSpan state
			TextSpan textSpan = blocks[0].TextSpans[0];

			Assert.AreEqual(0, textSpan.StartTextIndex);
			Assert.AreEqual(6, textSpan.StopTextIndex);
		}

		[Test]
		public void CheckIncorrectWord()
		{
			// Arrange
			BlockOwnerCollection blocks;
			BlockCommandSupervisor commands;
			PluginSupervisor plugins;
			NHunspellSpellingController controller;
			SetupPlugin(out blocks, out commands, out plugins, out controller);

			// Act
			commands.InsertText(blocks[0], 0, "Correc.");
			plugins.WaitForBlockAnalzyers();

			// Assert
			Project project = blocks.Project;

			Assert.AreEqual(2, project.Plugins.Controllers.Count);
		}

		/// <summary>
		/// Configures the environment to load the plugin manager and verify we
		/// have access to our plugin and controller.
		/// </summary>
		private void SetupPlugin(
			out BlockOwnerCollection blocks,
			out BlockCommandSupervisor commands,
			out PluginSupervisor plugins,
			out NHunspellSpellingController controller)
		{
			// Start getting us a simple plugin manager.
			var spelling = new SpellingFrameworkPlugin();
			var nhunspell = new NHunspellSpellingPlugin();
			var pluginManager = new PluginManager(spelling, nhunspell);

			PluginManager.Instance = pluginManager;

			// Create a project and pull out the useful properties we'll use to
			// make changes.
			var project = new Project();

			blocks = project.Blocks;
			commands = project.Commands;
			plugins = project.Plugins;

			// Load in the immediate correction editor.
			if (!plugins.Add("Spelling"))
			{
				// We couldn't load it for some reason.
				throw new ApplicationException("Cannot load 'Spelling' plugin.");
			}

			if (!plugins.Add("NHunspell"))
			{
				// We couldn't load it for some reason.
				throw new ApplicationException("Cannot load 'NHunspell' plugin.");
			}

			// Pull out the controller for the correction and cast it (since we know
			// what type it is).
			ProjectPluginController pluginController = plugins.Controllers[1];
			controller = (NHunspellSpellingController) pluginController.Controller;
		}

		#endregion
	}
}
