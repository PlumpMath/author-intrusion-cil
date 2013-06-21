// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System;
using System.IO;
using System.Xml.Serialization;
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
			ProjectBlockCollection blocks;
			BlockCommandSupervisor commands;
			ImmediateCorrectionProjectPlugin projectPlugin;
			BlockCommandContext context;
			SetupCorrectionPlugin(out context, out blocks, out commands, out projectPlugin);

			// Assert
			Project project = blocks.Project;

			Assert.AreEqual(1, project.Plugins.Controllers.Count);
		}

		[Test]
		public void SerializeDeserializeSettings()
		{
			// Arrange
			var settings = new ImmediateCorrectionSettings();
			settings.Substitutions.Add(
				new RegisteredSubstitution("a", "b", SubstitutionOptions.WholeWord));

			var serializer = new XmlSerializer(typeof (ImmediateCorrectionSettings));
			var memoryStream = new MemoryStream();

			// Act
			serializer.Serialize(memoryStream, settings);

			byte[] buffer = memoryStream.GetBuffer();
			var newMemoryStream = new MemoryStream(buffer);
			var newSettings =
				(ImmediateCorrectionSettings) serializer.Deserialize(newMemoryStream);

			// Assert
			Assert.AreEqual(1, newSettings.Substitutions.Count);
			Assert.AreEqual("a", newSettings.Substitutions[0].Search);
			Assert.AreEqual("b", newSettings.Substitutions[0].Replacement);
			Assert.AreEqual(
				SubstitutionOptions.WholeWord, newSettings.Substitutions[0].Options);
		}

		[Test]
		public void SimpleLargerWordSubstitution()
		{
			// Arrange
			ProjectBlockCollection blocks;
			BlockCommandSupervisor commands;
			ImmediateCorrectionProjectPlugin projectPlugin;
			BlockCommandContext context;
			SetupCorrectionPlugin(out context, out blocks, out commands, out projectPlugin);

			// Act
			projectPlugin.AddSubstitution(
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
			ProjectBlockCollection blocks;
			BlockCommandSupervisor commands;
			ImmediateCorrectionProjectPlugin projectPlugin;
			BlockCommandContext context;
			SetupCorrectionPlugin(out context, out blocks, out commands, out projectPlugin);

			// Act
			projectPlugin.AddSubstitution("teh", "the", SubstitutionOptions.WholeWord);

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
			ProjectBlockCollection blocks;
			BlockCommandSupervisor commands;
			ImmediateCorrectionProjectPlugin projectPlugin;
			BlockCommandContext context;
			SetupCorrectionPlugin(out context, out blocks, out commands, out projectPlugin);

			projectPlugin.AddSubstitution("teh", "the", SubstitutionOptions.WholeWord);

			commands.InsertText(blocks[0], 0, "teh ");

			// Act
			commands.Undo(context);

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
			ProjectBlockCollection blocks;
			BlockCommandSupervisor commands;
			ImmediateCorrectionProjectPlugin projectPlugin;
			BlockCommandContext context;
			SetupCorrectionPlugin(out context, out blocks, out commands, out projectPlugin);

			projectPlugin.AddSubstitution("teh", "the", SubstitutionOptions.WholeWord);

			commands.InsertText(blocks[0], 0, "teh ");
			commands.Undo(context);

			// Act
			commands.Redo(context);

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
			ProjectBlockCollection blocks;
			BlockCommandSupervisor commands;
			ImmediateCorrectionProjectPlugin projectPlugin;
			BlockCommandContext context;
			SetupCorrectionPlugin(out context, out blocks, out commands, out projectPlugin);

			projectPlugin.AddSubstitution("teh", "the", SubstitutionOptions.WholeWord);

			commands.InsertText(blocks[0], 0, "teh ");
			commands.Undo(context);

			// Act
			commands.Undo(context);

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
			ProjectBlockCollection blocks;
			BlockCommandSupervisor commands;
			ImmediateCorrectionProjectPlugin projectPlugin;
			BlockCommandContext context;
			SetupCorrectionPlugin(out context, out blocks, out commands, out projectPlugin);

			projectPlugin.AddSubstitution("teh", "the", SubstitutionOptions.WholeWord);

			commands.InsertText(blocks[0], 0, "teh ");
			commands.Undo(context);
			commands.Undo(context);

			// Act
			commands.Redo(context);

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
			ProjectBlockCollection blocks;
			BlockCommandSupervisor commands;
			ImmediateCorrectionProjectPlugin projectPlugin;
			BlockCommandContext context;
			SetupCorrectionPlugin(out context, out blocks, out commands, out projectPlugin);

			projectPlugin.AddSubstitution("teh", "the", SubstitutionOptions.WholeWord);

			commands.InsertText(blocks[0], 0, "teh ");
			commands.Undo(context);
			commands.Undo(context);
			commands.Redo(context);

			// Act
			commands.Redo(context);

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
		private void SetupCorrectionPlugin(out BlockCommandContext context,
			out ProjectBlockCollection blocks,
			out BlockCommandSupervisor commands,
			out ImmediateCorrectionProjectPlugin projectPlugin)
		{
			// Start getting us a simple plugin manager.
			var plugin = new ImmediateCorrectionPlugin();
			var pluginManager = new PluginManager(plugin);

			PluginManager.Instance = pluginManager;

			// Create a project and pull out the useful properties we'll use to
			// make changes.
			var project = new Project();

			context = new BlockCommandContext(project);
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
			projectPlugin =
				(ImmediateCorrectionProjectPlugin) pluginController.ProjectPlugin;
		}

		#endregion
	}
}
