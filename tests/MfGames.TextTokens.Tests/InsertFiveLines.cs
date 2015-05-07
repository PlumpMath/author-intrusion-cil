// <copyright file="InsertFiveLines.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using Xunit;

namespace MfGames.TextTokens.Tests
{
	/// <summary>
	/// Verifies the result of inserting five lines.
	/// </summary>
	public class InsertFiveLines : MemoryBufferTests
	{
		#region Public Methods and Operators

		/// <summary>
		/// Verifies that the buffer has the correct number of lines.
		/// </summary>
		[Fact]
		public void HasCorrectLineCount()
		{
			Setup();
			Assert.Equal(
				5,
				State.Lines.Count);
		}

		/// <summary>
		/// Verifies that line 1 has the correct text.
		/// </summary>
		[Fact]
		public void Line1HasCorrectText()
		{
			Setup();
			Assert.Equal(
				"zero one two three four",
				State.Lines[0].Tokens.GetVisibleText());
		}

		/// <summary>
		/// Verifies that line 1 has the correct number of tokens.
		/// </summary>
		[Fact]
		public void Line1HasCorrectTokenCount()
		{
			Setup();
			Assert.Equal(
				9,
				State.Lines[0].Tokens.Count);
		}

		/// <summary>
		/// Verifies that line 2 has the correct text.
		/// </summary>
		[Fact]
		public void Line2HasCorrectText()
		{
			Setup();
			Assert.Equal(
				"five six seven eight nine",
				State.Lines[1].Tokens.GetVisibleText());
		}

		/// <summary>
		/// Verifies that line 3 has the correct text.
		/// </summary>
		[Fact]
		public void Line3HasCorrectText()
		{
			Setup();
			Assert.Equal(
				"ten eleven twelve thirteen fourteen",
				State.Lines[2].Tokens.GetVisibleText());
		}

		/// <summary>
		/// Verifies that line 4 has the correct text.
		/// </summary>
		[Fact]
		public void Line4HasCorrectText()
		{
			Setup();
			Assert.Equal(
				"fifteen sixteen seventeen eighteen nineteen",
				State.Lines[3].Tokens.GetVisibleText());
		}

		/// <summary>
		/// Verifies that line 5 has the correct text.
		/// </summary>
		[Fact]
		public void Line5HasCorrectText()
		{
			Setup();
			Assert.Equal(
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
