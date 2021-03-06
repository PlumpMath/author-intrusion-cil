﻿// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Blocks;
using AuthorIntrusion.Common.Blocks.Locking;
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
			var context = new BlockCommandContext(project);
			ProjectBlockCollection blocks = project.Blocks;
			Block block = blocks[0];
			using (block.AcquireBlockLock(RequestLock.Write))
			{
				block.SetText("Testing 123");
			}
			BlockKey blockKey = block.BlockKey;

			// Act
			var command = new InsertAfterBlockCommand(blockKey, 1);
			project.Commands.Do(command, context);

			// Assert
			Assert.AreEqual(2, blocks.Count);
			Assert.AreEqual(
				new BlockPosition(blocks[1], 0), project.Commands.LastPosition);

			int index = 0;
			Assert.AreEqual("Testing 123", blocks[index].Text);

			index++;
			Assert.AreEqual("", blocks[index].Text);
		}

		[Test]
		public void TestUndoCommand()
		{
			// Arrange
			var project = new Project();
			var context = new BlockCommandContext(project);
			ProjectBlockCollection blocks = project.Blocks;
			Block block = blocks[0];
			using (block.AcquireBlockLock(RequestLock.Write))
			{
				block.SetText("Testing 123");
			}
			BlockKey blockKey = block.BlockKey;

			var command = new InsertAfterBlockCommand(blockKey, 1);
			project.Commands.Do(command, context);

			// Act
			project.Commands.Undo(context);

			// Assert
			Assert.AreEqual(1, blocks.Count);
			Assert.AreEqual(
				new BlockPosition(blocks[0], "Testing 123".Length),
				project.Commands.LastPosition);

			const int index = 0;
			Assert.AreEqual("Testing 123", blocks[index].Text);
		}

		[Test]
		public void TestUndoRedoCommand()
		{
			// Arrange
			var project = new Project();
			var context = new BlockCommandContext(project);
			ProjectBlockCollection blocks = project.Blocks;
			Block block = blocks[0];
			using (block.AcquireBlockLock(RequestLock.Write))
			{
				block.SetText("Testing 123");
			}
			BlockKey blockKey = block.BlockKey;

			var command = new InsertAfterBlockCommand(blockKey, 1);
			project.Commands.Do(command, context);
			project.Commands.Undo(context);

			// Act
			project.Commands.Redo(context);

			// Assert
			Assert.AreEqual(2, blocks.Count);
			Assert.AreEqual(
				new BlockPosition(blocks[1], 0), project.Commands.LastPosition);

			int index = 0;
			Assert.AreEqual("Testing 123", blocks[index].Text);

			index++;
			Assert.AreEqual("", blocks[index].Text);
		}

		[Test]
		public void TestUndoRedoUndoCommand()
		{
			// Arrange
			var project = new Project();
			var context = new BlockCommandContext(project);
			ProjectBlockCollection blocks = project.Blocks;
			Block block = blocks[0];
			using (block.AcquireBlockLock(RequestLock.Write))
			{
				block.SetText("Testing 123");
			}
			BlockKey blockKey = block.BlockKey;

			var command = new InsertAfterBlockCommand(blockKey, 1);
			project.Commands.Do(command, context);

			// Act
			project.Commands.Undo(context);

			// Assert
			Assert.AreEqual(1, blocks.Count);
			Assert.AreEqual(
				new BlockPosition(blocks[0], "Testing 123".Length),
				project.Commands.LastPosition);

			const int index = 0;
			Assert.AreEqual("Testing 123", blocks[index].Text);
		}

		#endregion
	}
}
