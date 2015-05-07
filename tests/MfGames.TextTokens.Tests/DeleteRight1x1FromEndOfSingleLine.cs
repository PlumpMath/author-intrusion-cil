// <copyright file="DeleteRight1x1FromEndOfSingleLine.cs" company="Moonfire Games">
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
	/// Tests deleting the last character of a line, which will merge the next
	/// line with the current one.
	/// </summary>
	public class DeleteRight1x1FromEndOfSingleLine : MemoryBufferTests
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
					4,
					3),
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
					4,
					3),
				Controller.SelectionCursor);
		}

		/// <summary>
		/// Verifies that there is only a single line in the buffer.
		/// </summary>
		[Fact]
		public virtual void HasCorrectLineCount()
		{
			Setup();
			Assert.Equal(
				1,
				State.Lines.Count);
		}

		/// <summary>
		/// Tests that line 1 has the correct text.
		/// </summary>
		[Fact]
		public virtual void Line1HasCorrectText()
		{
			Setup();
			Assert.Equal(
				"zero one twothree four five",
				State.Lines[0].Tokens.GetVisibleText());
		}

		/// <summary>
		/// Tests that line 1 has the correct number of tokens.
		/// </summary>
		[Fact]
		public virtual void Line1HasCorrectTokenCount()
		{
			Setup();
			Assert.Equal(
				9,
				State.Lines[0].Tokens.Count);
		}

		/// <summary>
		/// Tests that line 2 has the correct text.
		/// </summary>
		[Fact]
		public virtual void Line2HasCorrectText()
		{
		}

		/// <summary>
		/// Tests that line 2 has the correct number of tokens.
		/// </summary>
		[Fact]
		public virtual void Line2HasCorrectTokenCount()
		{
		}

		#endregion

		#region Methods

		/// <summary>
		/// Sets up the unit tests.
		/// </summary>
		protected override void Setup()
		{
			base.Setup();
			Buffer.PopulateRowColumn(
				2,
				3);
			var textLocation = new TextLocation(
				0,
				4,
				3);
			Controller.SetCursor(textLocation);
			Controller.DeleteRight(1);
		}

		#endregion

		/// <summary>
		/// Perform the parent class and then performs an undo.
		/// </summary>
		public class Undo : DeleteRight1x1FromEndOfSingleLine
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
						4,
						3),
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
						4,
						3),
					Controller.SelectionCursor);
			}

			/// <summary>
			/// Verifies that there is only a single line in the buffer.
			/// </summary>
			[Fact]
			public override void HasCorrectLineCount()
			{
				Setup();
				Assert.Equal(
					2,
					State.Lines.Count);
			}

			/// <summary>
			/// Tests that line 1 has the correct text.
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
			/// Tests that line 1 has the correct number of tokens.
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
			/// Tests that line 2 has the correct text.
			/// </summary>
			[Fact]
			public override void Line2HasCorrectText()
			{
				Setup();
				Assert.Equal(
					"three four five",
					State.Lines[1].Tokens.GetVisibleText());
			}

			/// <summary>
			/// Tests that line 2 has the correct number of tokens.
			/// </summary>
			[Fact]
			public override void Line2HasCorrectTokenCount()
			{
				Setup();
				Assert.Equal(
					5,
					State.Lines[1].Tokens.Count);
			}

			#endregion

			#region Methods

			/// <summary>
			/// Sets up the unit tests.
			/// </summary>
			protected override void Setup()
			{
				base.Setup();
				Controller.Undo();
			}

			#endregion
		}

		/// <summary>
		/// Performs the parent task, then an undo, and then a redo.
		/// </summary>
		public class UndoRedo : DeleteRight1x1FromEndOfSingleLine
		{
			#region Methods

			/// <summary>
			/// Sets up the unit tests.
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
		/// Performs the parent task, an undo, a redo, and an undo.
		/// </summary>
		public class UndoRedoUndo : Undo
		{
			#region Methods

			/// <summary>
			/// Sets up the unit tests.
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
