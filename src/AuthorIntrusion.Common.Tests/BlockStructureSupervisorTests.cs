// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Blocks;
using AuthorIntrusion.Common.Commands;
using NUnit.Framework;

namespace AuthorIntrusion.Common.Tests
{
	[TestFixture]
	public class BlockStructureSupervisorTests: CommonMultilineTests
	{
		#region Methods

		[Test]
		public void ComplicatedRelationshipTest()
		{
			// Act
			BlockOwnerCollection blocks;
			BlockCommandSupervisor commands;
			BlockTypeSupervisor blockTypes;
			SetupComplexMultilineTest(out blocks, out blockTypes, out commands);

			// Assert
			Assert.AreEqual(10, blocks.Count);

			int index = 0;
			Assert.AreEqual(blockTypes.Chapter, blocks[index].BlockType);
			Assert.IsNull(blocks[index].ParentBlock);

			index++;
			Assert.AreEqual(blockTypes.Scene, blocks[index].BlockType);
			Assert.AreEqual(blocks[0], blocks[index].ParentBlock);

			index++;
			Assert.AreEqual(blockTypes.Epigraph, blocks[index].BlockType);
			Assert.AreEqual(blocks[1], blocks[index].ParentBlock);

			index++;
			Assert.AreEqual(blockTypes.EpigraphAttribution, blocks[index].BlockType);
			Assert.AreEqual(blocks[1], blocks[index].ParentBlock);

			index++;
			Assert.AreEqual(blockTypes.Paragraph, blocks[index].BlockType);
			Assert.AreEqual(blocks[1], blocks[index].ParentBlock);

			index++;
			Assert.AreEqual(blockTypes.Paragraph, blocks[index].BlockType);
			Assert.AreEqual(blocks[1], blocks[index].ParentBlock);

			index++;
			Assert.AreEqual(blockTypes.Scene, blocks[index].BlockType);
			Assert.AreEqual(blocks[0], blocks[index].ParentBlock);

			index++;
			Assert.AreEqual(blockTypes.Epigraph, blocks[index].BlockType);
			Assert.AreEqual(blocks[6], blocks[index].ParentBlock);

			index++;
			Assert.AreEqual(blockTypes.EpigraphAttribution, blocks[index].BlockType);
			Assert.AreEqual(blocks[6], blocks[index].ParentBlock);

			index++;
			Assert.AreEqual(blockTypes.Paragraph, blocks[index].BlockType);
			Assert.AreEqual(blocks[6], blocks[index].ParentBlock);
		}

		[Test]
		public void ChangeBlockTypeFromChapter()
		{
			// Arrange
			BlockOwnerCollection blocks;
			BlockCommandSupervisor commands;
			BlockTypeSupervisor blockTypes;
			SetupComplexMultilineTest(out blocks, out blockTypes, out commands);

			// Act
			blocks[6].SetBlockType(blockTypes.Paragraph);

			// Assert
			Assert.AreEqual(10, blocks.Count);

			int index = 0;
			Assert.AreEqual(blockTypes.Chapter, blocks[index].BlockType);
			Assert.IsNull(blocks[index].ParentBlock);

			index++;
			Assert.AreEqual(blockTypes.Scene, blocks[index].BlockType);
			Assert.AreEqual(blocks[0], blocks[index].ParentBlock);

			index++;
			Assert.AreEqual(blockTypes.Epigraph, blocks[index].BlockType);
			Assert.AreEqual(blocks[1], blocks[index].ParentBlock);

			index++;
			Assert.AreEqual(blockTypes.EpigraphAttribution, blocks[index].BlockType);
			Assert.AreEqual(blocks[1], blocks[index].ParentBlock);

			index++;
			Assert.AreEqual(blockTypes.Paragraph, blocks[index].BlockType);
			Assert.AreEqual(blocks[1], blocks[index].ParentBlock);

			index++;
			Assert.AreEqual(blockTypes.Paragraph, blocks[index].BlockType);
			Assert.AreEqual(blocks[1], blocks[index].ParentBlock);

			index++;
			Assert.AreEqual(blockTypes.Paragraph, blocks[index].BlockType);
			Assert.AreEqual(blocks[1], blocks[index].ParentBlock);

			index++;
			Assert.AreEqual(blockTypes.Epigraph, blocks[index].BlockType);
			Assert.AreEqual(blocks[1], blocks[index].ParentBlock);

			index++;
			Assert.AreEqual(blockTypes.EpigraphAttribution, blocks[index].BlockType);
			Assert.AreEqual(blocks[1], blocks[index].ParentBlock);

			index++;
			Assert.AreEqual(blockTypes.Paragraph, blocks[index].BlockType);
			Assert.AreEqual(blocks[1], blocks[index].ParentBlock);
		}

		[Test]
		public void SimpleRelationshipTest()
		{
			// Act
			BlockOwnerCollection blocks;
			BlockCommandSupervisor commands;
			BlockTypeSupervisor blockTypes;
			SetupMultilineTest(out blocks, out blockTypes, out commands);

			// Assert
			Assert.AreEqual(4, blocks.Count);

			int index = 0;
			Assert.AreEqual(blockTypes.Chapter, blocks[index].BlockType);
			Assert.IsNull(blocks[index].ParentBlock);

			index++;
			Assert.AreEqual(blockTypes.Scene, blocks[index].BlockType);
			Assert.AreEqual(blocks[0], blocks[index].ParentBlock);

			index++;
			Assert.AreEqual(blockTypes.Scene, blocks[index].BlockType);
			Assert.AreEqual(blocks[0], blocks[index].ParentBlock);

			index++;
			Assert.AreEqual(blockTypes.Scene, blocks[index].BlockType);
			Assert.AreEqual(blocks[0], blocks[index].ParentBlock);
		}

		#endregion
	}
}
