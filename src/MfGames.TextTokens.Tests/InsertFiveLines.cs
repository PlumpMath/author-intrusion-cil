// <copyright file="InsertFiveLines.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using NUnit.Framework;

namespace MfGames.TextTokens.Tests
{
	/// <summary>
	/// Verifies the result of inserting five lines.
	/// </summary>
	[TestFixture]
	public class InsertFiveLines : MemoryBufferTests
	{
		#region Public Methods and Operators

		/// <summary>
		/// Verifies that the buffer has the correct number of lines.
		/// </summary>
		[Test]
		public void HasCorrectLineCount()
		{
			Setup();
			Assert.AreEqual(
				5,
				State.Lines.Count);
		}

		/// <summary>
		/// Verifies that line 1 has the correct text.
		/// </summary>
		[Test]
		public void Line1HasCorrectText()
		{
			Setup();
			Assert.AreEqual(
				"zero one two three four",
				State.Lines[0].Tokens.GetVisibleText());
		}

		/// <summary>
		/// Verifies that line 1 has the correct number of tokens.
		/// </summary>
		[Test]
		public void Line1HasCorrectTokenCount()
		{
			Setup();
			Assert.AreEqual(
				9,
				State.Lines[0].Tokens.Count);
		}

		/// <summary>
		/// Verifies that line 2 has the correct text.
		/// </summary>
		[Test]
		public void Line2HasCorrectText()
		{
			Setup();
			Assert.AreEqual(
				"five six seven eight nine",
				State.Lines[1].Tokens.GetVisibleText());
		}

		/// <summary>
		/// Verifies that line 3 has the correct text.
		/// </summary>
		[Test]
		public void Line3HasCorrectText()
		{
			Setup();
			Assert.AreEqual(
				"ten eleven twelve thirteen fourteen",
				State.Lines[2].Tokens.GetVisibleText());
		}

		/// <summary>
		/// Verifies that line 4 has the correct text.
		/// </summary>
		[Test]
		public void Line4HasCorrectText()
		{
			Setup();
			Assert.AreEqual(
				"fifteen sixteen seventeen eighteen nineteen",
				State.Lines[3].Tokens.GetVisibleText());
		}

		/// <summary>
		/// Verifies that line 5 has the correct text.
		/// </summary>
		[Test]
		public void Line5HasCorrectText()
		{
			Setup();
			Assert.AreEqual(
				"twenty twenty-one twenty-two twenty-three twenty-four",
				State.Lines[4].Tokens.GetVisibleText());
		}

		#endregion

		#region Methods

		/// <summary>
		/// Sets up the unit test.
		/// </summary>
		protected override void Setup()
		{
			base.Setup();
			Buffer.PopulateRowColumn(
				5,
				5);
		}

		#endregion
	}
}
