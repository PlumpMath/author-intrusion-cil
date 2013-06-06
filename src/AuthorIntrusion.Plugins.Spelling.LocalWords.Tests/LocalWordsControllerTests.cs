// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System;
using System.Collections.Generic;
using AuthorIntrusion.Common;
using AuthorIntrusion.Common.Actions;
using AuthorIntrusion.Common.Blocks;
using AuthorIntrusion.Common.Commands;
using AuthorIntrusion.Common.Plugins;
using NUnit.Framework;

namespace AuthorIntrusion.Plugins.Spelling.LocalWords.Tests
{
	[TestFixture]
	public class LocalWordsControllerTests
	{
		#region Methods

		[Test]
		public void ActivatePlugin()
		{
			// Act
			ProjectBlockCollection blocks;
			BlockCommandSupervisor commands;
			PluginSupervisor plugins;
			LocalWordsController controller;
			SetupPlugin(out blocks, out commands, out plugins, out controller);

			// Assert
			Project project = blocks.Project;

			Assert.AreEqual(2, project.Plugins.Controllers.Count);
		}

		[Test]
		public void CheckCaseInsensitiveCorrectWord()
		{
			// Arrange
			ProjectBlockCollection blocks;
			BlockCommandSupervisor commands;
			PluginSupervisor plugins;
			LocalWordsController controller;
			SetupPlugin(out blocks, out commands, out plugins, out controller);

			// Act
			commands.InsertText(blocks[0], 0, "one.");
			plugins.WaitForBlockAnalzyers();

			// Assert
			Assert.AreEqual(0, blocks[0].TextSpans.Count);
		}

		[Test]
		public void CheckCaseInsensitiveCorrectWordDifferentCase()
		{
			// Arrange
			ProjectBlockCollection blocks;
			BlockCommandSupervisor commands;
			PluginSupervisor plugins;
			LocalWordsController controller;
			SetupPlugin(out blocks, out commands, out plugins, out controller);

			// Act
			commands.InsertText(blocks[0], 0, "ONE.");
			plugins.WaitForBlockAnalzyers();

			// Assert
			Assert.AreEqual(0, blocks[0].TextSpans.Count);
		}

		[Test]
		public void CheckCaseInsensitiveIncorrectWord()
		{
			// Arrange
			ProjectBlockCollection blocks;
			BlockCommandSupervisor commands;
			PluginSupervisor plugins;
			LocalWordsController controller;
			SetupPlugin(out blocks, out commands, out plugins, out controller);

			// Act
			commands.InsertText(blocks[0], 0, "two.");
			plugins.WaitForBlockAnalzyers();

			// Assert
			Assert.AreEqual(1, blocks[0].TextSpans.Count);
		}

		[Test]
		public void CheckCaseSensitiveCorrectWord()
		{
			// Arrange
			ProjectBlockCollection blocks;
			BlockCommandSupervisor commands;
			PluginSupervisor plugins;
			LocalWordsController controller;
			SetupPlugin(out blocks, out commands, out plugins, out controller);

			// Act
			commands.InsertText(blocks[0], 0, "Correct.");
			plugins.WaitForBlockAnalzyers();

			// Assert
			Assert.AreEqual(0, blocks[0].TextSpans.Count);
		}

		[Test]
		public void CheckCaseSensitiveCorrectWordWrongCase()
		{
			// Arrange
			ProjectBlockCollection blocks;
			BlockCommandSupervisor commands;
			PluginSupervisor plugins;
			LocalWordsController controller;
			SetupPlugin(out blocks, out commands, out plugins, out controller);

			// Act
			commands.InsertText(blocks[0], 0, "correct.");
			plugins.WaitForBlockAnalzyers();

			// Assert
			Assert.AreEqual(1, blocks[0].TextSpans.Count);
		}

		[Test]
		public void CheckCaseSensitiveIncorrectActions()
		{
			// Arrange
			ProjectBlockCollection blocks;
			BlockCommandSupervisor commands;
			PluginSupervisor plugins;
			LocalWordsController controller;
			SetupPlugin(out blocks, out commands, out plugins, out controller);

			// Arrange: Edit the text
			Block block = blocks[0];

			commands.InsertText(block, 0, "Correc.");
			plugins.WaitForBlockAnalzyers();

			// Act: Get the editor actions.
			TextSpan textSpan = block.TextSpans[0];

			IList<IEditorAction> actions = plugins.GetEditorActions(block, textSpan);

			// Assert
			Assert.AreEqual(0, actions.Count);
		}

		[Test]
		public void CheckCaseSensitiveIncorrectTextSpan()
		{
			// Arrange
			ProjectBlockCollection blocks;
			BlockCommandSupervisor commands;
			PluginSupervisor plugins;
			LocalWordsController controller;
			SetupPlugin(out blocks, out commands, out plugins, out controller);

			// Act
			commands.InsertText(blocks[0], 0, "Correc.");
			plugins.WaitForBlockAnalzyers();

			// Assert: TextSpans created
			Assert.AreEqual(1, blocks[0].TextSpans.Count);
		}

		[Test]
		public void CheckCaseSensitiveIncorrectWord()
		{
			// Arrange
			ProjectBlockCollection blocks;
			BlockCommandSupervisor commands;
			PluginSupervisor plugins;
			LocalWordsController controller;
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
			out ProjectBlockCollection blocks,
			out BlockCommandSupervisor commands,
			out PluginSupervisor plugins,
			out LocalWordsController controller)
		{
			// Start getting us a simple plugin manager.
			var spelling = new SpellingFrameworkPlugin();
			var nhunspell = new LocalWordsPlugin();
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

			if (!plugins.Add("Local Words"))
			{
				// We couldn't load it for some reason.
				throw new ApplicationException("Cannot load 'Local Words' plugin.");
			}

			// Pull out the controller for the correction and cast it (since we know
			// what type it is).
			ProjectPluginController pluginController = plugins.Controllers[1];
			controller = (LocalWordsController) pluginController.Controller;
			controller.CaseSensitiveDictionary.Add("Correct");
			controller.CaseInsensitiveDictionary.Add("one");
		}

		#endregion
	}
}
