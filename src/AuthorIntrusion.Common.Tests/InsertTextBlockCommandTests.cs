// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Blocks;
using AuthorIntrusion.Common.Commands;
using NUnit.Framework;

namespace AuthorIntrusion.Common.Tests
{
	[TestFixture]
	public class InsertTextBlockCommandTests
	{
		#region Methods

		[Test]
		public void TestInitialInsertText()
		{
			// Arrange
			var project = new Project();
			BlockOwnerCollection blocks = project.Blocks;
			Block block = blocks[0];
			block.Text = "abcd";
			int blockVersion = block.Version;
			BlockKey blockKey = block.BlockKey;

			// Act
			var command = new InsertTextCommand(new BlockPosition(blockKey, 2), "YES");
			project.Commands.Do(command);

			// Assert
			Assert.AreEqual(1, blocks.Count);
			Assert.AreEqual("abYEScd", block.Text);
			Assert.AreEqual(blockVersion + 1, block.Version);
		}

		[Test]
		public void TestRedoInsertText()
		{
			// Arrange
			var project = new Project();
			BlockOwnerCollection blocks = project.Blocks;
			Block block = blocks[0];
			block.Text = "abcd";
			int blockVersion = block.Version;
			BlockKey blockKey = block.BlockKey;

			var command = new InsertTextCommand(new BlockPosition(blockKey, 2), "YES");
			project.Commands.Do(command);
			project.Commands.Undo();

			// Act
			project.Commands.Redo();

			// Assert
			Assert.AreEqual(1, blocks.Count);
			Assert.AreEqual("abYEScd", block.Text);
			Assert.AreEqual(blockVersion + 3, block.Version);
		}

		[Test]
		public void TestUndoInsertText()
		{
			// Arrange
			var project = new Project();
			BlockOwnerCollection blocks = project.Blocks;
			Block block = blocks[0];
			block.Text = "abcd";
			int blockVersion = block.Version;
			BlockKey blockKey = block.BlockKey;

			var command = new InsertTextCommand(new BlockPosition(blockKey, 2), "YES");
			project.Commands.Do(command);

			// Act
			project.Commands.Undo();

			// Assert
			Assert.AreEqual(1, blocks.Count);
			Assert.AreEqual("abcd", block.Text);
			Assert.AreEqual(blockVersion + 2, block.Version);
		}

		#endregion
	}
}
