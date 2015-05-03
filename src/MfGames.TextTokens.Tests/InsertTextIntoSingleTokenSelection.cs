// <copyright file="InsertTextIntoSingleTokenSelection.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using MfGames.TextTokens.Texts;

using Xunit;

namespace MfGames.TextTokens.Tests
{
	/// <summary>
	/// Inserts text into a selection within a token.
	/// </summary>
	public class InsertTextIntoSingleTokenSelection : MemoryBufferTests
	{
		#region Public Methods and Operators

		/// <summary>
		/// Verifies that there is only a single line in the buffer.
		/// </summary>
		[Fact]
		public void HasCorrectLineCount()
		{
			Setup();
			Assert.Equal(
				1,
				State.Lines.Count);
		}

		/// <summary>
		/// Verifies that line 1 has the correct text.
		/// </summary>
		[Fact]
		public virtual void Line1HasCorrectText()
		{
			Setup();
			Assert.Equal(
				"zero oBe two",
				State.Lines[0].Tokens.GetVisibleText());
		}

		/// <summary>
		/// Verifies that line 1 has the correct number of tokens.
		/// </summary>
		[Fact]
		public virtual void Line1HasCorrectTokenCount()
		{
			Setup();
			Assert.Equal(
				5,
				State.Lines[0].Tokens.Count);
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
				1,
				3);
			var anchor = new TextLocation(
				0,
				2,
				1);
			var cursor = new TextLocation(
				0,
				2,
				2);
			Controller.SetCursor(anchor);
			Controller.Select(cursor);
			Controller.InsertText("B");
		}

		#endregion
	}
}
