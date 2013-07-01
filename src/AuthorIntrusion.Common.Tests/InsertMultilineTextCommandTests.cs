// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Blocks;
using AuthorIntrusion.Common.Commands;
using NUnit.Framework;

namespace AuthorIntrusion.Common.Tests
{
	[TestFixture]
	public class InsertMultilineTextCommandTests: CommonMultilineTests
	{
		#region Methods

		[Test]
		public void LastLineTestCommand()
		{
			// Arrange
			ProjectBlockCollection blocks;
			BlockCommandSupervisor commands;
			BlockTypeSupervisor blockTypes;
			BlockCommandContext context;
			SetupMultilineTest(out context, out blocks, out blockTypes, out commands);

			// Act
			var command =
				new InsertMultilineTextCommand(
					new BlockPosition(blocks[3].BlockKey, 6), "AAA\nBBB\nCCC");
			commands.Do(command, context);

			// Assert
			Assert.AreEqual(6, blocks.Count);
			Assert.AreEqual(new BlockPosition(blocks[5], 3), commands.LastPosition);

			int index = 0;
			Assert.AreEqual("Line 1", blocks[index].Text);
			Assert.AreEqual(blockTypes.Chapter, blocks[index].BlockType);

			index++;
			Assert.AreEqual("Line 2", blocks[index].Text);
			Assert.AreEqual(blockTypes.Scene, blocks[index].BlockType);

			index++;
			Assert.AreEqual("Line 3", blocks[index].Text);
			Assert.AreEqual(blockTypes.Scene, blocks[index].BlockType);

			index++;
			Assert.AreEqual("Line 4AAA", blocks[index].Text);
			Assert.AreEqual(blockTypes.Scene, blocks[index].BlockType);

			index++;
			Assert.AreEqual("BBB", blocks[index].Text);
			Assert.AreEqual(blockTypes.Paragraph, blocks[index].BlockType);

			index++;
			Assert.AreEqual("CCC", blocks[index].Text);
			Assert.AreEqual(blockTypes.Paragraph, blocks[index].BlockType);
		}

		[Test]
		public void LastLineTestUndoCommand()
		{
			// Arrange
			ProjectBlockCollection blocks;
			BlockCommandSupervisor commands;
			BlockTypeSupervisor blockTypes;
			BlockCommandContext context;
			SetupMultilineTest(out context, out blocks, out blockTypes, out commands);

			var command =
				new InsertMultilineTextCommand(
					new BlockPosition(blocks[3].BlockKey, 5), "AAA\nBBB\nCCC");
			commands.Do(command, context);

			// Act
			commands.Undo(context);

			// Assert
			Assert.AreEqual(4, blocks.Count);
			Assert.AreEqual(new BlockPosition(blocks[3], 5), commands.LastPosition);

			int index = 0;
			Assert.AreEqual("Line 1", blocks[index].Text);
			Assert.AreEqual(blockTypes.Chapter, blocks[index].BlockType);

			index++;
			Assert.AreEqual("Line 2", blocks[index].Text);
			Assert.AreEqual(blockTypes.Scene, blocks[index].BlockType);

			index++;
			Assert.AreEqual("Line 3", blocks[index].Text);
			Assert.AreEqual(blockTypes.Scene, blocks[index].BlockType);

			index++;
			Assert.AreEqual("Line 4", blocks[index].Text);
			Assert.AreEqual(blockTypes.Scene, blocks[index].BlockType);
		}

		[Test]
		public void LastLineTestUndoRedoCommand()
		{
			// Arrange
			ProjectBlockCollection blocks;
			BlockCommandSupervisor commands;
			BlockTypeSupervisor blockTypes;
			BlockCommandContext context;
			SetupMultilineTest(out context, out blocks, out blockTypes, out commands);

			var command =
				new InsertMultilineTextCommand(
					new BlockPosition(blocks[3].BlockKey, 6), "AAA\nBBB\nCCC");
			commands.Do(command, context);
			commands.Undo(context);

			// Act
			commands.Redo(context);

			// Assert
			Assert.AreEqual(6, blocks.Count);
			Assert.AreEqual(new BlockPosition(blocks[5], 3), commands.LastPosition);

			int index = 0;
			Assert.AreEqual("Line 1", blocks[index].Text);
			Assert.AreEqual(blockTypes.Chapter, blocks[index].BlockType);

			index++;
			Assert.AreEqual("Line 2", blocks[index].Text);
			Assert.AreEqual(blockTypes.Scene, blocks[index].BlockType);

			index++;
			Assert.AreEqual("Line 3", blocks[index].Text);
			Assert.AreEqual(blockTypes.Scene, blocks[index].BlockType);

			index++;
			Assert.AreEqual("Line 4AAA", blocks[index].Text);
			Assert.AreEqual(blockTypes.Scene, blocks[index].BlockType);

			index++;
			Assert.AreEqual("BBB", blocks[index].Text);
			Assert.AreEqual(blockTypes.Paragraph, blocks[index].BlockType);

			index++;
			Assert.AreEqual("CCC", blocks[index].Text);
			Assert.AreEqual(blockTypes.Paragraph, blocks[index].BlockType);
		}

		[Test]
		public void LastLineTestUndoRedoUndoCommand()
		{
			// Arrange
			ProjectBlockCollection blocks;
			BlockCommandSupervisor commands;
			BlockTypeSupervisor blockTypes;
			BlockCommandContext context;
			SetupMultilineTest(out context, out blocks, out blockTypes, out commands);

			var command =
				new InsertMultilineTextCommand(
					new BlockPosition(blocks[3].BlockKey, 5), "AAA\nBBB\nCCC");
			commands.Do(command, context);
			commands.Undo(context);
			commands.Redo(context);

			// Act
			commands.Undo(context);

			// Assert
			Assert.AreEqual(4, blocks.Count);
			Assert.AreEqual(new BlockPosition(blocks[3], 5), commands.LastPosition);

			int index = 0;
			Assert.AreEqual("Line 1", blocks[index].Text);
			Assert.AreEqual(blockTypes.Chapter, blocks[index].BlockType);

			index++;
			Assert.AreEqual("Line 2", blocks[index].Text);
			Assert.AreEqual(blockTypes.Scene, blocks[index].BlockType);

			index++;
			Assert.AreEqual("Line 3", blocks[index].Text);
			Assert.AreEqual(blockTypes.Scene, blocks[index].BlockType);

			index++;
			Assert.AreEqual("Line 4", blocks[index].Text);
			Assert.AreEqual(blockTypes.Scene, blocks[index].BlockType);
		}

		[Test]
		public void TestCommand()
		{
			// Arrange
			ProjectBlockCollection blocks;
			BlockCommandSupervisor commands;
			BlockTypeSupervisor blockTypes;
			BlockCommandContext context;
			SetupMultilineTest(out context, out blocks, out blockTypes, out commands);

			// Act
			var command =
				new InsertMultilineTextCommand(
					new BlockPosition(blocks[0].BlockKey, 5), "AAA\nBBB\nCCC");
			commands.Do(command, context);

			// Assert
			Assert.AreEqual(6, blocks.Count);
			Assert.AreEqual(new BlockPosition(blocks[2], 3), commands.LastPosition);

			int index = 0;
			Assert.AreEqual("Line AAA", blocks[index].Text);
			Assert.AreEqual(blockTypes.Chapter, blocks[index].BlockType);

			index++;
			Assert.AreEqual("BBB", blocks[index].Text);
			Assert.AreEqual(blockTypes.Paragraph, blocks[index].BlockType);

			index++;
			Assert.AreEqual("CCC1", blocks[index].Text);
			Assert.AreEqual(blockTypes.Paragraph, blocks[index].BlockType);

			index++;
			Assert.AreEqual("Line 2", blocks[index].Text);
			Assert.AreEqual(blockTypes.Scene, blocks[index].BlockType);

			index++;
			Assert.AreEqual("Line 3", blocks[index].Text);
			Assert.AreEqual(blockTypes.Scene, blocks[index].BlockType);

			index++;
			Assert.AreEqual("Line 4", blocks[index].Text);
			Assert.AreEqual(blockTypes.Scene, blocks[index].BlockType);
		}

		[Test]
		public void TestUndoCommand()
		{
			// Arrange
			ProjectBlockCollection blocks;
			BlockCommandSupervisor commands;
			BlockTypeSupervisor blockTypes;
			BlockCommandContext context;
			SetupMultilineTest(out context, out blocks, out blockTypes, out commands);

			var command =
				new InsertMultilineTextCommand(
					new BlockPosition(blocks[0].BlockKey, 5), "AAA\nBBB\nCCC");
			commands.Do(command, context);

			// Act
			commands.Undo(context);

			// Assert
			Assert.AreEqual(4, blocks.Count);
			Assert.AreEqual(new BlockPosition(blocks[0], 5), commands.LastPosition);

			int index = 0;
			Assert.AreEqual("Line 1", blocks[index].Text);
			Assert.AreEqual(blockTypes.Chapter, blocks[index].BlockType);

			index++;
			Assert.AreEqual("Line 2", blocks[index].Text);
			Assert.AreEqual(blockTypes.Scene, blocks[index].BlockType);

			index++;
			Assert.AreEqual("Line 3", blocks[index].Text);
			Assert.AreEqual(blockTypes.Scene, blocks[index].BlockType);

			index++;
			Assert.AreEqual("Line 4", blocks[index].Text);
			Assert.AreEqual(blockTypes.Scene, blocks[index].BlockType);
		}

		[Test]
		public void TestUndoRedoCommand()
		{
			// Arrange
			ProjectBlockCollection blocks;
			BlockCommandSupervisor commands;
			BlockTypeSupervisor blockTypes;
			BlockCommandContext context;
			SetupMultilineTest(out context, out blocks, out blockTypes, out commands);

			var command =
				new InsertMultilineTextCommand(
					new BlockPosition(blocks[0].BlockKey, 5), "AAA\nBBB\nCCC");
			commands.Do(command, context);
			commands.Undo(context);

			// Act
			commands.Redo(context);

			// Assert
			Assert.AreEqual(6, blocks.Count);
			Assert.AreEqual(new BlockPosition(blocks[2], 3), commands.LastPosition);

			int index = 0;
			Assert.AreEqual("Line AAA", blocks[index].Text);
			Assert.AreEqual(blockTypes.Chapter, blocks[index].BlockType);

			index++;
			Assert.AreEqual("BBB", blocks[index].Text);
			Assert.AreEqual(blockTypes.Paragraph, blocks[index].BlockType);

			index++;
			Assert.AreEqual("CCC1", blocks[index].Text);
			Assert.AreEqual(blockTypes.Paragraph, blocks[index].BlockType);

			index++;
			Assert.AreEqual("Line 2", blocks[index].Text);
			Assert.AreEqual(blockTypes.Scene, blocks[index].BlockType);

			index++;
			Assert.AreEqual("Line 3", blocks[index].Text);
			Assert.AreEqual(blockTypes.Scene, blocks[index].BlockType);

			index++;
			Assert.AreEqual("Line 4", blocks[index].Text);
			Assert.AreEqual(blockTypes.Scene, blocks[index].BlockType);
		}

		[Test]
		public void TestUndoRedoUndoCommand()
		{
			// Arrange
			ProjectBlockCollection blocks;
			BlockCommandSupervisor commands;
			BlockTypeSupervisor blockTypes;
			BlockCommandContext context;
			SetupMultilineTest(out context, out blocks, out blockTypes, out commands);

			var command =
				new InsertMultilineTextCommand(
					new BlockPosition(blocks[0].BlockKey, 5), "AAA\nBBB\nCCC");
			commands.Do(command, context);
			commands.Undo(context);
			commands.Redo(context);

			// Act
			commands.Undo(context);

			// Assert
			Assert.AreEqual(4, blocks.Count);
			Assert.AreEqual(new BlockPosition(blocks[0], 5), commands.LastPosition);

			int index = 0;
			Assert.AreEqual("Line 1", blocks[index].Text);
			Assert.AreEqual(blockTypes.Chapter, blocks[index].BlockType);

			index++;
			Assert.AreEqual("Line 2", blocks[index].Text);
			Assert.AreEqual(blockTypes.Scene, blocks[index].BlockType);

			index++;
			Assert.AreEqual("Line 3", blocks[index].Text);
			Assert.AreEqual(blockTypes.Scene, blocks[index].BlockType);

			index++;
			Assert.AreEqual("Line 4", blocks[index].Text);
			Assert.AreEqual(blockTypes.Scene, blocks[index].BlockType);
		}

		#endregion
	}
}
