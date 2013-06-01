// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System;
using AuthorIntrusion.Common;
using AuthorIntrusion.Common.Blocks;
using AuthorIntrusion.Common.Commands;
using AuthorIntrusion.Common.Plugins;
using NUnit.Framework;

namespace AuthorIntrusion.Plugins.ImmediateCorrection.Tests
{
	[TestFixture]
	public class ImmedicateCorrectionEditorTests
	{
		#region Methods

		[Test]
		public void ActivatePlugin()
		{
			// Act
			BlockOwnerCollection blocks;
			BlockCommandSupervisor commands;
			ImmediateCorrectionController controller;
			SetupCorrectionPlugin(out blocks, out commands, out controller);

			// Assert
			Project project = blocks.Project;

			Assert.AreEqual(1, project.Plugins.Controllers.Count);
		}

		[Test]
		public void SimpleLargerWordSubstitution()
		{
			// Arrange
			BlockOwnerCollection blocks;
			BlockCommandSupervisor commands;
			ImmediateCorrectionController controller;
			SetupCorrectionPlugin(out blocks, out commands, out controller);

			// Act
			controller.AddSubstitution(
				"abbr", "abbreviation", SubstitutionOptions.WholeWord);

			commands.InsertText(blocks[0], 0, "abbr ");

			// Assert
			Assert.AreEqual("abbreviation ", blocks[0].Text);
			Assert.AreEqual(
				new BlockPosition(blocks[0], "abbreviation ".Length), commands.LastPosition);
		}

		[Test]
		public void SimpleWordSubstitution()
		{
			// Arrange
			BlockOwnerCollection blocks;
			BlockCommandSupervisor commands;
			ImmediateCorrectionController controller;
			SetupCorrectionPlugin(out blocks, out commands, out controller);

			// Act
			controller.AddSubstitution("teh", "the", SubstitutionOptions.WholeWord);

			commands.InsertText(blocks[0], 0, "teh ");

			// Assert
			Assert.AreEqual("the ", blocks[0].Text);
			Assert.IsFalse(commands.CanRedo);
			Assert.IsTrue(commands.CanUndo);
			Assert.AreEqual(new BlockPosition(blocks[0], 4), commands.LastPosition);
		}

		[Test]
		public void SimpleWordSubstitutionUndo()
		{
			// Arrange
			BlockOwnerCollection blocks;
			BlockCommandSupervisor commands;
			ImmediateCorrectionController controller;
			SetupCorrectionPlugin(out blocks, out commands, out controller);

			controller.AddSubstitution("teh", "the", SubstitutionOptions.WholeWord);

			commands.InsertText(blocks[0], 0, "teh ");

			// Act
			commands.Undo();

			// Assert
			Assert.AreEqual("teh ", blocks[0].Text);
			Assert.IsTrue(commands.CanRedo);
			Assert.IsTrue(commands.CanUndo);
			Assert.AreEqual(new BlockPosition(blocks[0], 4), commands.LastPosition);
		}

		[Test]
		public void SimpleWordSubstitutionUndoRedo()
		{
			// Arrange
			BlockOwnerCollection blocks;
			BlockCommandSupervisor commands;
			ImmediateCorrectionController controller;
			SetupCorrectionPlugin(out blocks, out commands, out controller);

			controller.AddSubstitution("teh", "the", SubstitutionOptions.WholeWord);

			commands.InsertText(blocks[0], 0, "teh ");
			commands.Undo();

			// Act
			commands.Redo();

			// Assert
			Assert.AreEqual("the ", blocks[0].Text);
			Assert.IsFalse(commands.CanRedo);
			Assert.IsTrue(commands.CanUndo);
			Assert.AreEqual(new BlockPosition(blocks[0], 4), commands.LastPosition);
		}

		[Test]
		public void SimpleWordSubstitutionUndoUndo()
		{
			// Arrange
			BlockOwnerCollection blocks;
			BlockCommandSupervisor commands;
			ImmediateCorrectionController controller;
			SetupCorrectionPlugin(out blocks, out commands, out controller);

			controller.AddSubstitution("teh", "the", SubstitutionOptions.WholeWord);

			commands.InsertText(blocks[0], 0, "teh ");
			commands.Undo();

			// Act
			commands.Undo();

			// Assert
			Assert.AreEqual("", blocks[0].Text);
			Assert.IsTrue(commands.CanRedo);
			Assert.IsFalse(commands.CanUndo);
			Assert.AreEqual(new BlockPosition(blocks[0], 0), commands.LastPosition);
		}

		[Test]
		public void SimpleWordSubstitutionUndoUndoRedo()
		{
			// Arrange
			BlockOwnerCollection blocks;
			BlockCommandSupervisor commands;
			ImmediateCorrectionController controller;
			SetupCorrectionPlugin(out blocks, out commands, out controller);

			controller.AddSubstitution("teh", "the", SubstitutionOptions.WholeWord);

			commands.InsertText(blocks[0], 0, "teh ");
			commands.Undo();
			commands.Undo();

			// Act
			commands.Redo();

			// Assert
			Assert.AreEqual("teh ", blocks[0].Text);
			Assert.IsTrue(commands.CanRedo);
			Assert.IsTrue(commands.CanUndo);
			Assert.AreEqual(new BlockPosition(blocks[0], 4), commands.LastPosition);
		}

		[Test]
		public void SimpleWordSubstitutionUndoUndoRedoRedo()
		{
			// Arrange
			BlockOwnerCollection blocks;
			BlockCommandSupervisor commands;
			ImmediateCorrectionController controller;
			SetupCorrectionPlugin(out blocks, out commands, out controller);

			controller.AddSubstitution("teh", "the", SubstitutionOptions.WholeWord);

			commands.InsertText(blocks[0], 0, "teh ");
			commands.Undo();
			commands.Undo();
			commands.Redo();

			// Act
			commands.Redo();

			// Assert
			Assert.AreEqual("the ", blocks[0].Text);
			Assert.IsFalse(commands.CanRedo);
			Assert.IsTrue(commands.CanUndo);
			Assert.AreEqual(new BlockPosition(blocks[0], 4), commands.LastPosition);
		}

		/// <summary>
		/// Configures the environment to load the plugin manager and verify we
		/// have access to the ImmediateCorrectionPlugin.
		/// </summary>
		private void SetupCorrectionPlugin(
			out BlockOwnerCollection blocks,
			out BlockCommandSupervisor commands,
			out ImmediateCorrectionController controller)
		{
			// Start getting us a simple plugin manager.
			var plugin = new ImmediateCorrectionPlugin();
			var pluginManager = new PluginManager(plugin);

			PluginManager.Instance = pluginManager;

			// Create a project and pull out the useful properties we'll use to
			// make changes.
			var project = new Project();

			blocks = project.Blocks;
			commands = project.Commands;

			// Load in the immediate correction editor.
			if (!project.Plugins.Add("Immediate Correction"))
			{
				// We couldn't load it for some reason.
				throw new ApplicationException("Cannot load immediate correction plugin");
			}

			// Pull out the controller for the correction and cast it (since we know
			// what type it is).
			ProjectPluginController pluginController = project.Plugins.Controllers[0];
			controller = (ImmediateCorrectionController) pluginController.Controller;
		}

		#endregion
	}
}
