// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Blocks;
using AuthorIntrusion.Common.Commands;
using NUnit.Framework;

namespace AuthorIntrusion.Common.Tests
{
	[TestFixture]
	public class DeleteBlockCommandTests: CommonMultilineTests
	{
		#region Methods

		[Test]
		public void SingleLineTestCommand()
		{
			// Arrange
			BlockOwnerCollection blocks;
			BlockCommandSupervisor commands;
			BlockTypeSupervisor blockTypes;
			SetupMultilineTest(out blocks, out blockTypes, out commands, 1);

			// Act
			var command = new DeleteBlockCommand(blocks[0].BlockKey);
			commands.Do(command);

			// Assert
			Assert.AreEqual(1, blocks.Count);

			const int index = 0;
			Assert.AreEqual("", blocks[index].Text);
			Assert.AreEqual(blockTypes.Paragraph, blocks[index].BlockType);
		}

		[Test]
		public void SingleLineTestUndoCommand()
		{
			// Arrange
			BlockOwnerCollection blocks;
			BlockCommandSupervisor commands;
			BlockTypeSupervisor blockTypes;
			SetupMultilineTest(out blocks, out blockTypes, out commands, 1);

			var command = new DeleteBlockCommand(blocks[0].BlockKey);
			commands.Do(command);

			// Act
			commands.Undo();

			// Assert
			Assert.AreEqual(1, blocks.Count);

			const int index = 0;
			Assert.AreEqual("Line 1", blocks[index].Text);
			Assert.AreEqual(blockTypes.Chapter, blocks[index].BlockType);
		}

		[Test]
		public void SingleLineTestUndoRedoCommand()
		{
			// Arrange
			BlockOwnerCollection blocks;
			BlockCommandSupervisor commands;
			BlockTypeSupervisor blockTypes;
			SetupMultilineTest(out blocks, out blockTypes, out commands, 1);

			var command = new DeleteBlockCommand(blocks[0].BlockKey);
			commands.Do(command);
			commands.Undo();

			// Act
			commands.Redo();

			// Assert
			Assert.AreEqual(1, blocks.Count);

			const int index = 0;
			Assert.AreEqual("", blocks[index].Text);
			Assert.AreEqual(blockTypes.Paragraph, blocks[index].BlockType);
		}

		[Test]
		public void SingleLineTestUndoRedoUndoCommand()
		{
			// Arrange
			BlockOwnerCollection blocks;
			BlockCommandSupervisor commands;
			BlockTypeSupervisor blockTypes;
			SetupMultilineTest(out blocks, out blockTypes, out commands, 1);

			var command = new DeleteBlockCommand(blocks[0].BlockKey);
			commands.Do(command);
			commands.Undo();
			commands.Redo();

			// Act
			commands.Undo();

			// Assert
			Assert.AreEqual(1, blocks.Count);

			const int index = 0;
			Assert.AreEqual("Line 1", blocks[index].Text);
			Assert.AreEqual(blockTypes.Chapter, blocks[index].BlockType);
		}

		[Test]
		public void TestCommand()
		{
			// Arrange
			BlockOwnerCollection blocks;
			BlockCommandSupervisor commands;
			BlockTypeSupervisor blockTypes;
			SetupMultilineTest(out blocks, out blockTypes, out commands);

			// Act
			var command = new DeleteBlockCommand(blocks[0].BlockKey);
			commands.Do(command);

			// Assert
			Assert.AreEqual(3, blocks.Count);

			int index = 0;
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
			BlockOwnerCollection blocks;
			BlockCommandSupervisor commands;
			BlockTypeSupervisor blockTypes;
			SetupMultilineTest(out blocks, out blockTypes, out commands);

			var command = new DeleteBlockCommand(blocks[0].BlockKey);
			commands.Do(command);

			// Act
			commands.Undo();

			// Assert
			Assert.AreEqual(4, blocks.Count);

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
			BlockOwnerCollection blocks;
			BlockCommandSupervisor commands;
			BlockTypeSupervisor blockTypes;
			SetupMultilineTest(out blocks, out blockTypes, out commands);

			var command = new DeleteBlockCommand(blocks[0].BlockKey);
			commands.Do(command);
			commands.Undo();

			// Act
			commands.Redo();

			// Assert
			Assert.AreEqual(3, blocks.Count);

			int index = 0;
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
			BlockOwnerCollection blocks;
			BlockCommandSupervisor commands;
			BlockTypeSupervisor blockTypes;
			SetupMultilineTest(out blocks, out blockTypes, out commands);

			var command = new DeleteBlockCommand(blocks[0].BlockKey);
			commands.Do(command);
			commands.Undo();
			commands.Redo();

			// Act
			commands.Undo();

			// Assert
			Assert.AreEqual(4, blocks.Count);

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
