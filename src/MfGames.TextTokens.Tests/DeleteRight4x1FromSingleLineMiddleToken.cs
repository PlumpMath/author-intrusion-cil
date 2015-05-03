// <copyright file="DeleteRight4x1FromSingleLineMiddleToken.cs" company="Moonfire Games">
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
	/// Deletes four characters from the text. This is to verify that the results are
	/// identical to the 1x4 version.
	/// </summary>
	public class DeleteRight4x1FromSingleLineMiddleToken : MemoryBufferTests
	{
		#region Public Methods and Operators

		/// <summary>
		/// Verifies the cursor is in the correct location.
		/// </summary>
		[Fact]
		public virtual void AnchorPositionIsRight()
		{
			Setup();
			Assert.Equal(
				new TextLocation(
					0,
					2,
					2),
				Controller.SelectionAnchor);
		}

		/// <summary>
		/// Verifies the cursor is in the correct location.
		/// </summary>
		[Fact]
		public virtual void CursorPositionIsRight()
		{
			Setup();
			Assert.Equal(
				new TextLocation(
					0,
					2,
					2),
				Controller.SelectionCursor);
		}

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
				"zero ono",
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
				3,
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
			var textLocation = new TextLocation(
				0,
				2,
				2);
			Controller.SetCursor(textLocation);
			Controller.DeleteRight(4);
		}

		#endregion

		/// <summary>
		/// Performs the task and then an undo.
		/// </summary>
		public class Undo : DeleteRight4x1FromSingleLineMiddleToken
		{
			#region Public Methods and Operators

			/// <summary>
			/// Verifies the cursor is in the correct location.
			/// </summary>
			[Fact]
			public override void AnchorPositionIsRight()
			{
				Setup();
				Assert.Equal(
					new TextLocation(
						0,
						2,
						2),
					Controller.SelectionAnchor);
			}

			/// <summary>
			/// Verifies the cursor is in the correct location.
			/// </summary>
			[Fact]
			public override void CursorPositionIsRight()
			{
				Setup();
				Assert.Equal(
					new TextLocation(
						0,
						2,
						2),
					Controller.SelectionCursor);
			}

			/// <summary>
			/// Verifies that line 1 has the correct text.
			/// </summary>
			[Fact]
			public override void Line1HasCorrectText()
			{
				Setup();
				Assert.Equal(
					"zero one two",
					State.Lines[0].Tokens.GetVisibleText());
			}

			/// <summary>
			/// Verifies that line 1 has the correct number of tokens.
			/// </summary>
			[Fact]
			public override void Line1HasCorrectTokenCount()
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
				Controller.Undo();
			}

			#endregion
		}

		/// <summary>
		/// Performs the task, an undo, and then a redo.
		/// </summary>
		public class UndoRedo : DeleteRight4x1FromSingleLineMiddleToken
		{
			#region Methods

			/// <summary>
			/// Sets up the unit test.
			/// </summary>
			protected override void Setup()
			{
				base.Setup();
				Controller.Undo();
				Controller.Redo();
			}

			#endregion
		}

		/// <summary>
		/// Performs the task, an undo, a redo, and then an undo.
		/// </summary>
		public class UndoRedoUndo : Undo
		{
			#region Methods

			/// <summary>
			/// Sets up the unit test.
			/// </summary>
			protected override void Setup()
			{
				base.Setup();
				Controller.Redo();
				Controller.Undo();
			}

			#endregion
		}
	}
}
