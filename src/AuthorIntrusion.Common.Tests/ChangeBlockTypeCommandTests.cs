// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Blocks;
using AuthorIntrusion.Common.Commands;
using NUnit.Framework;

namespace AuthorIntrusion.Common.Tests
{
	[TestFixture]
	public class ChangeBlockTypeCommandTests: CommonMultilineTests
	{
		#region Methods

		[Test]
		public void TestCommand()
		{
			// Arrange
			BlockOwnerCollection blocks;
			BlockCommandSupervisor commands;
			BlockTypeSupervisor blockTypes;
			SetupMultilineTest(out blocks, out blockTypes, out commands);

			// Act
			var command = new ChangeBlockTypeCommand(
				blocks[0].BlockKey, blockTypes.Paragraph);
			commands.Do(command);

			// Assert
			Assert.AreEqual(4, blocks.Count);

			int index = 0;
			Assert.AreEqual("Line 1", blocks[index].Text);
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
			BlockOwnerCollection blocks;
			BlockCommandSupervisor commands;
			BlockTypeSupervisor blockTypes;
			SetupMultilineTest(out blocks, out blockTypes, out commands);

			var command = new ChangeBlockTypeCommand(
				blocks[0].BlockKey, blockTypes.Paragraph);
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

			var command = new ChangeBlockTypeCommand(
				blocks[0].BlockKey, blockTypes.Paragraph);
			commands.Do(command);
			commands.Undo();

			// Act
			commands.Redo();

			// Assert
			Assert.AreEqual(4, blocks.Count);

			int index = 0;
			Assert.AreEqual("Line 1", blocks[index].Text);
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
			BlockOwnerCollection blocks;
			BlockCommandSupervisor commands;
			BlockTypeSupervisor blockTypes;
			SetupMultilineTest(out blocks, out blockTypes, out commands);

			var command = new ChangeBlockTypeCommand(
				blocks[0].BlockKey, blockTypes.Paragraph);
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
