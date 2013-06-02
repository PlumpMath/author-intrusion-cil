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
		public void ChangeMiddleParagraph()
		{
			// Arrange
			BlockOwnerCollection blocks;
			BlockCommandSupervisor commands;
			BlockTypeSupervisor blockTypes;
			SetupMultilineTest(out blocks, out blockTypes, out commands, lineCount: 8);

			// Act

			// Assert
		}

		#endregion
	}
}
