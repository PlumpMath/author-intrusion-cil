// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Blocks;
using NUnit.Framework;

namespace AuthorIntrusion.Common.Tests
{
	[TestFixture]
	public class BlockOperationTests
	{
		#region Methods

		[Test]
		public void TestInitialState()
		{
			// Act
			var project = new Project();

			// Assert
			ProjectBlockCollection blocks = project.Blocks;

			Assert.AreEqual(1, blocks.Count);

			Block block = blocks[0];

			Assert.AreEqual(string.Empty, block.Text);
		}

		#endregion
	}
}
