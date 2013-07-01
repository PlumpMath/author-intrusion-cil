// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Projects;
using NUnit.Framework;

namespace AuthorIntrusion.Common.Tests
{
	[TestFixture]
	public class ProjectMacrosTest
	{
		#region Methods

		[Test]
		public void ExpandBlankString()
		{
			// Arrange
			var macros = new ProjectMacros();

			macros.Substitutions["ProjectDir"] = "pd";

			// Act
			string results = macros.ExpandMacros("");

			// Assert
			Assert.AreEqual("", results);
		}

		[Test]
		public void ExpandNonExistantValue()
		{
			// Arrange
			var macros = new ProjectMacros();

			macros.Substitutions["ProjectDir"] = "pd";

			// Act
			string results = macros.ExpandMacros("{ProjectPath}");

			// Assert
			Assert.AreEqual("", results);
		}

		[Test]
		public void ExpandRepeatedValue()
		{
			// Arrange
			var macros = new ProjectMacros();

			macros.Substitutions["ProjectDir"] = "pd";
			macros.Substitutions["ProjectPath"] = "{ProjectDir}/p";

			// Act
			string results = macros.ExpandMacros("{ProjectPath}");

			// Assert
			Assert.AreEqual("pd/p", results);
		}

		[Test]
		public void ExpandValue()
		{
			// Arrange
			var macros = new ProjectMacros();

			macros.Substitutions["ProjectDir"] = "pd";

			// Act
			string results = macros.ExpandMacros("{ProjectDir}");

			// Assert
			Assert.AreEqual("pd", results);
		}

		#endregion
	}
}
