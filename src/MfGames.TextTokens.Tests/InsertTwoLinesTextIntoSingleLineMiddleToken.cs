// <copyright file="InsertTwoLinesTextIntoSingleLineMiddleToken.cs" company="Moonfire Games">
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
	/// Inserts two lines into the middle of a token.
	/// </summary>
	public class InsertTwoLinesTextIntoSingleLineMiddleToken : MemoryBufferTests
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
					1,
					0,
					1),
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
					1,
					0,
					1),
				Controller.SelectionCursor);
		}

		/// <summary>
		/// Verifies that there is only a single line in the buffer.
		/// </summary>
		[Fact]
		public virtual void HasCorrectNumberOfLines()
		{
			Setup();
			Assert.Equal(
				2,
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
				"zero on_",
				State.Lines[0].Tokens.GetVisibleText());
		}

		/// <summary>
		/// Verifies line 1 has the correct number of tokens.
		/// </summary>
		[Fact]
		public virtual void Line1HasCorrectTokenCount()
		{
			Setup();
			Assert.Equal(
				4,
				State.Lines[0].Tokens.Count);
		}

		/// <summary>
		/// Verifies that line 2 has the correct text.
		/// </summary>
		[Fact]
		public virtual void Line2HasCorrectText()
		{
			Setup();
			Assert.Equal(
				"_e two",
				State.Lines[1].Tokens.GetVisibleText());
		}

		/// <summary>
		/// Verifies that line 2 has the correct number of tokens.
		/// </summary>
		[Fact]
		public virtual void Line2HasCorrectTokenCount()
		{
			Setup();
			Assert.Equal(
				4,
				State.Lines[1].Tokens.Count);
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
			Controller.InsertText(
				textLocation,
				"_\n_");
		}

		#endregion

		/// <summary>
		/// Perform the task and an undo.
		/// </summary>
		public class Undo : InsertTwoLinesTextIntoSingleLineMiddleToken
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
					Controller.SelectionAnchor);
			}

			/// <summary>
			/// Determines whether [has proper number of lines].
			/// </summary>
			[Fact]
			public override void HasCorrectNumberOfLines()
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
			public override void Line1HasCorrectText()
			{
				base.Setup();
				Setup();
				Assert.Equal(
					"zero one two",
					State.Lines[0].Tokens.GetVisibleText());
			}

			/// <summary>
			/// Verifies line 1 has the correct number of tokens.
			/// </summary>
			[Fact]
			public override void Line1HasCorrectTokenCount()
			{
				Setup();
				Assert.Equal(
					5,
					State.Lines[0].Tokens.Count);
			}

			/// <summary>
			/// Verifies that line 2 has the correct text.
			/// </summary>
			[Fact]
			public override void Line2HasCorrectText()
			{
				;
			}

			/// <summary>
			/// Verifies that line 2 has the correct number of tokens.
			/// </summary>
			[Fact]
			public override void Line2HasCorrectTokenCount()
			{
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
		public class UndoRedo : InsertTwoLinesTextIntoSingleLineMiddleToken
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

		/// <summary>
		/// Performs a task, an undo, a redo, an undo, or a redo.
		/// </summary>
		public class UndoRedoUndoRedo :
			InsertTwoLinesTextIntoSingleLineMiddleToken
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
				Controller.Undo();
				Controller.Redo();
			}

			#endregion
		}
	}
}
