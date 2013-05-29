// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Blocks;
using AuthorIntrusion.Common.Commands;
using NUnit.Framework;

namespace AuthorIntrusion.Common.Tests
{
	[TestFixture]
	public class DeleteBlockCommandTests
	{
		#region Methods

		[Test]
		public void TestCommand()
		{
			// Arrange
			var project = new Project();
			BlockOwnerCollection blocks = project.Blocks;
			blocks.Add(
				new Block(blocks)
				{
					Text = "Testing 123"
				});
			Block block = blocks[0];
			block.Text = "Testing 321";
			BlockKey blockKey = block.BlockKey;

			// Act
			var command = new DeleteBlockCommand(blockKey);
			project.Commands.Do(command);

			// Assert
			Assert.AreEqual(1, blocks.Count);
			Assert.AreEqual("Testing 123", blocks[0].Text);
		}

		[Test]
		public void TestUndoCommand()
		{
			// Arrange
			var project = new Project();
			BlockOwnerCollection blocks = project.Blocks;
			blocks.Add(
				new Block(blocks)
				{
					Text = "Testing 123"
				});
			Block block = blocks[0];
			block.Text = "Testing 321";
			BlockKey blockKey = block.BlockKey;

			var command = new DeleteBlockCommand(blockKey);
			project.Commands.Do(command);

			// Act
			project.Commands.Undo();

			// Assert
			Assert.AreEqual(2, blocks.Count);
			Assert.AreEqual("Testing 321", blocks[0].Text);
			Assert.AreEqual("Testing 123", blocks[1].Text);
		}

		[Test]
		public void TestUndoRedoCommand()
		{
			// Arrange
			var project = new Project();
			BlockOwnerCollection blocks = project.Blocks;
			blocks.Add(
				new Block(blocks)
				{
					Text = "Testing 123"
				});
			Block block = blocks[0];
			block.Text = "Testing 321";
			BlockKey blockKey = block.BlockKey;

			var command = new DeleteBlockCommand(blockKey);
			project.Commands.Do(command);
			project.Commands.Undo();

			// Act
			project.Commands.Redo();

			// Assert
			Assert.AreEqual(1, blocks.Count);
			Assert.AreEqual("Testing 123", blocks[0].Text);
		}

		[Test]
		public void TestUndoRedoUndoCommand()
		{
			// Arrange
			var project = new Project();
			BlockOwnerCollection blocks = project.Blocks;
			blocks.Add(
				new Block(blocks)
				{
					Text = "Testing 123"
				});
			Block block = blocks[0];
			block.Text = "Testing 321";
			BlockKey blockKey = block.BlockKey;

			var command = new DeleteBlockCommand(blockKey);
			project.Commands.Do(command);
			project.Commands.Undo();
			project.Commands.Redo();

			// Act
			project.Commands.Undo();

			// Assert
			Assert.AreEqual(2, blocks.Count);
			Assert.AreEqual("Testing 321", blocks[0].Text);
			Assert.AreEqual("Testing 123", blocks[1].Text);
		}

		#endregion
	}
}
