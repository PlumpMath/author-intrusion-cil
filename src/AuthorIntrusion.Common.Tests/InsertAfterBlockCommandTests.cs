// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Blocks;
using AuthorIntrusion.Common.Commands;
using NUnit.Framework;

namespace AuthorIntrusion.Common.Tests
{
	[TestFixture]
	public class InsertAfterBlockCommandTests
	{
		#region Methods

		[Test]
		public void TestCommand()
		{
			// Arrange
			var project = new Project();
			BlockOwnerCollection blocks = project.Blocks;
			Block block = blocks[0];
			block.Text = "Testing 123";
			BlockKey blockKey = block.BlockKey;

			// Act
			var command = new InsertAfterBlockCommand(blockKey, 1);
			project.Commands.Do(command);

			// Assert
			Assert.AreEqual(2, blocks.Count);
			Assert.AreEqual("Testing 123", block.Text);
			Assert.AreEqual("", blocks[1].Text);
		}

		[Test]
		public void TestUndoCommand()
		{
			// Arrange
			var project = new Project();
			BlockOwnerCollection blocks = project.Blocks;
			Block block = blocks[0];
			block.Text = "Testing 123";
			BlockKey blockKey = block.BlockKey;

			var command = new InsertAfterBlockCommand(blockKey, 1);
			project.Commands.Do(command);

			// Act
			project.Commands.Undo();

			// Assert
			Assert.AreEqual(1, blocks.Count);
			Assert.AreEqual("Testing 123", block.Text);
		}

		[Test]
		public void TestUndoRedoCommand()
		{
			// Arrange
			var project = new Project();
			BlockOwnerCollection blocks = project.Blocks;
			Block block = blocks[0];
			block.Text = "Testing 123";
			BlockKey blockKey = block.BlockKey;

			var command = new InsertAfterBlockCommand(blockKey, 1);
			project.Commands.Do(command);
			project.Commands.Undo();

			// ACT
			project.Commands.Redo();

			// Assert
			Assert.AreEqual(2, blocks.Count);
			Assert.AreEqual("Testing 123", block.Text);
			Assert.AreEqual("", blocks[1].Text);
		}

		[Test]
		public void TestUndoRedoUndoCommand()
		{
			// Arrange
			var project = new Project();
			BlockOwnerCollection blocks = project.Blocks;
			Block block = blocks[0];
			block.Text = "Testing 123";
			BlockKey blockKey = block.BlockKey;

			var command = new InsertAfterBlockCommand(blockKey, 1);
			project.Commands.Do(command);

			// Act
			project.Commands.Undo();

			// Assert
			Assert.AreEqual(1, blocks.Count);
			Assert.AreEqual("Testing 123", block.Text);
		}

		#endregion
	}
}
