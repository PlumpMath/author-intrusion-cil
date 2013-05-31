// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Blocks;
using AuthorIntrusion.Common.Commands;
using NUnit.Framework;

namespace AuthorIntrusion.Common.Tests
{
	[TestFixture]
	public class SetTextBlockCommandTests
	{
		#region Methods

		[Test]
		public void TestCommand()
		{
			// Arrange
			var project = new Project();
			BlockOwnerCollection blocks = project.Blocks;
			Block block = blocks[0];
			int blockVersion = block.Version;
			BlockKey blockKey = block.BlockKey;

			// Act
			var command = new SetTextCommand(blockKey, "Testing 123");
			project.Commands.Do(command);

			// Assert
			Assert.AreEqual(1, blocks.Count);
			Assert.AreEqual(
				new BlockPosition(blocks[0], "Testing 123".Length),
				project.Commands.LastPosition);

			const int index = 0;
			Assert.AreEqual("Testing 123", blocks[index].Text);
			Assert.AreEqual(blockVersion + 1, blocks[index].Version);
		}

		[Test]
		public void TestUndoCommand()
		{
			// Arrange
			var project = new Project();
			BlockOwnerCollection blocks = project.Blocks;
			Block block = blocks[0];
			int blockVersion = block.Version;
			BlockKey blockKey = block.BlockKey;

			var command = new SetTextCommand(blockKey, "Testing 123");
			project.Commands.Do(command);

			// Act
			project.Commands.Undo();

			// Assert
			Assert.AreEqual(1, blocks.Count);
			Assert.AreEqual(
				new BlockPosition(blocks[0], 0), project.Commands.LastPosition);

			const int index = 0;
			Assert.AreEqual("", blocks[index].Text);
			Assert.AreEqual(blockVersion + 2, blocks[index].Version);
		}

		[Test]
		public void TestUndoRedoCommand()
		{
			// Arrange
			var project = new Project();
			BlockOwnerCollection blocks = project.Blocks;
			Block block = blocks[0];
			int blockVersion = block.Version;
			BlockKey blockKey = block.BlockKey;

			var command = new SetTextCommand(blockKey, "Testing 123");
			project.Commands.Do(command);

			project.Commands.Undo();

			// Act
			project.Commands.Redo();

			// Assert
			Assert.AreEqual(1, blocks.Count);
			Assert.AreEqual(
				new BlockPosition(blocks[0], "Testing 123".Length),
				project.Commands.LastPosition);

			const int index = 0;
			Assert.AreEqual("Testing 123", blocks[index].Text);
			Assert.AreEqual(blockVersion + 3, blocks[index].Version);
		}

		#endregion
	}
}
