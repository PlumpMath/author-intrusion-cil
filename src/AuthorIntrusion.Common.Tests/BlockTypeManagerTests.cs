// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Blocks;
using NUnit.Framework;

namespace AuthorIntrusion.Common.Tests
{
	[TestFixture]
	public class BlockTypeManagerTests
	{
		#region Methods

		[Test]
		public void CreateEmptyBlockTypeManager()
		{
			// Arrange
			var project = new Project();

			// Act
			var manager = new BlockTypeSupervisor(project);

			// Assert
			Assert.AreEqual(project, manager.Project);
		}

		#endregion
	}
}
