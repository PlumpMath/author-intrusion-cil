// <copyright file="InsertThreeLinesTextIntoSingleLineMiddleToken.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using MfGames.TextTokens.Texts;

using NUnit.Framework;

namespace MfGames.TextTokens.Tests
{
	/// <summary>
	/// Verifies the result of pasting three lines into a token.
	/// </summary>
	[TestFixture]
	public class InsertThreeLinesTextIntoSingleLineMiddleToken :
		MemoryBufferTests
	{
		#region Public Methods and Operators

		/// <summary>
		/// Verifies the cursor is in the correct location.
		/// </summary>
		[Test]
		public virtual void AnchorPositionIsRight()
		{
			Setup();
			Assert.AreEqual(
				new TextLocation(
					2,
					0,
					1),
				Controller.SelectionAnchor);
		}

		/// <summary>
		/// Verifies the cursor is in the correct location.
		/// </summary>
		[Test]
		public virtual void CursorPositionIsRight()
		{
			Setup();
			Assert.AreEqual(
				new TextLocation(
					2,
					0,
					1),
				Controller.SelectionCursor);
		}

		/// <summary>
		/// Verifies that there is only a single line in the buffer.
		/// </summary>
		[Test]
		public virtual void HasProperNumberOfLines()
		{
			Setup();
			Assert.AreEqual(
				3,
				State.Lines.Count);
		}

		/// <summary>
		/// Verifies that line 1 has the correct text.
		/// </summary>
		[Test]
		public virtual void Line1HasCorrectText()
		{
			Setup();
			Assert.AreEqual(
				"zero on_",
				State.Lines[0].Tokens.GetVisibleText());
		}

		/// <summary>
		/// Verifies that line 1 has the correct number of tokens.
		/// </summary>
		[Test]
		public virtual void Line1HasCorrectTokenCount()
		{
			Setup();
			Assert.AreEqual(
				4,
				State.Lines[0].Tokens.Count);
		}

		/// <summary>
		/// Verifies that line 2 has the correct text.
		/// </summary>
		[Test]
		public virtual void Line2HasCorrectText()
		{
			Setup();
			Assert.AreEqual(
				"|",
				State.Lines[1].Tokens.GetVisibleText());
		}

		/// <summary>
		/// Verifies that line 2 has the correct number of tokens.
		/// </summary>
		[Test]
		public virtual void Line2HasCorrectTokenCount()
		{
			Setup();
			Assert.AreEqual(
				1,
				State.Lines[1].Tokens.Count);
		}

		/// <summary>
		/// Verifies that line 3 has the correct text.
		/// </summary>
		[Test]
		public virtual void Line3HasCorrectText()
		{
			Setup();
			Assert.AreEqual(
				"_e two",
				State.Lines[2].Tokens.GetVisibleText());
		}

		/// <summary>
		/// Verifies line 3 has the correct number of tokens.
		/// </summary>
		[Test]
		public virtual void Line3HasCorrectTokenCount()
		{
			Setup();
			Assert.AreEqual(
				4,
				State.Lines[2].Tokens.Count);
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
				"_\n|\n_");
		}

		#endregion

		/// <summary>
		/// Performs the task and then an undo.
		/// </summary>
		public class Undo : InsertThreeLinesTextIntoSingleLineMiddleToken
		{
			#region Public Methods and Operators

			/// <summary>
			/// Verifies the cursor is in the correct location.
			/// </summary>
			[Test]
			public override void AnchorPositionIsRight()
			{
				Setup();
				Assert.AreEqual(
					new TextLocation(
						0,
						2,
						2),
					Controller.SelectionAnchor);
			}

			/// <summary>
			/// Verifies the cursor is in the correct location.
			/// </summary>
			[Test]
			public override void CursorPositionIsRight()
			{
				Setup();
				Assert.AreEqual(
					new TextLocation(
						0,
						2,
						2),
					Controller.SelectionAnchor);
			}

			/// <summary>
			/// Verifies that there is only a single line in the buffer.
			/// </summary>
			[Test]
			public override void HasProperNumberOfLines()
			{
				Setup();
				Assert.AreEqual(
					1,
					State.Lines.Count);
			}

			/// <summary>
			/// Verifies that line 1 has the correct text.
			/// </summary>
			[Test]
			public override void Line1HasCorrectText()
			{
				base.Setup();
				Setup();
				Assert.AreEqual(
					"zero one two",
					State.Lines[0].Tokens.GetVisibleText());
			}

			/// <summary>
			/// Verifies that line 1 has the correct number of tokens.
			/// </summary>
			[Test]
			public override void Line1HasCorrectTokenCount()
			{
				Setup();
				Assert.AreEqual(
					5,
					State.Lines[0].Tokens.Count);
			}

			/// <summary>
			/// Verifies that line 2 has the correct text.
			/// </summary>
			[Test]
			public override void Line2HasCorrectText()
			{
				Assert.Pass();
			}

			/// <summary>
			/// Verifies that line 2 has the correct number of tokens.
			/// </summary>
			[Test]
			public override void Line2HasCorrectTokenCount()
			{
				Assert.Pass();
			}

			/// <summary>
			/// Verifies that line 3 has the correct text.
			/// </summary>
			public override void Line3HasCorrectText()
			{
				Assert.Pass();
			}

			/// <summary>
			/// Verifies line 3 has the correct number of tokens.
			/// </summary>
			public override void Line3HasCorrectTokenCount()
			{
				Assert.Pass();
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
		public class UndoRedo : InsertThreeLinesTextIntoSingleLineMiddleToken
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
		/// Performs the task, an undo, a redo, an undo, and then a redo.
		/// </summary>
		public class UndoRedoUndoRedo :
			InsertThreeLinesTextIntoSingleLineMiddleToken
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
