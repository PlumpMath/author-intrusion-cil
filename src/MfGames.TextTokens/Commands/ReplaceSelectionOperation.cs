// <copyright file="ReplaceSelectionOperation.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System.Collections.Generic;

using MfGames.TextTokens.Buffers;
using MfGames.TextTokens.Texts;

namespace MfGames.TextTokens.Commands
{
	/// <summary>
	/// An operation to change the selection of the currently active buffer.
	/// </summary>
	public class ReplaceSelectionOperation : IBufferOperation
	{
		#region Constructors and Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="ReplaceSelectionOperation"/> class.
		/// </summary>
		/// <param name="textRange">
		/// The text range.
		/// </param>
		public ReplaceSelectionOperation(TextRange textRange)
		{
			NewTextRange = textRange;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ReplaceSelectionOperation"/> class.
		/// </summary>
		/// <param name="anchor">
		/// The anchor.
		/// </param>
		public ReplaceSelectionOperation(TextLocation anchor)
			: this(anchor,
				anchor)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ReplaceSelectionOperation"/> class.
		/// </summary>
		/// <param name="anchor">
		/// The anchor.
		/// </param>
		/// <param name="cursor">
		/// The cursor.
		/// </param>
		public ReplaceSelectionOperation(
			TextLocation anchor,
			TextLocation cursor)
			: this(new TextRange(
				anchor,
				cursor))
		{
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the new text range for the operation.
		/// </summary>
		/// <value>
		/// The new text range.
		/// </value>
		protected TextRange NewTextRange { get; private set; }

		/// <summary>
		/// Gets the old text ranges, associated with each buffer.
		/// </summary>
		/// <value>
		/// The old text ranges.
		/// </value>
		protected Dictionary<object, TextRange> OldTextRanges { get; private set; }

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Performs the operation on the buffer.
		/// </summary>
		/// <param name="buffer">
		/// The buffer to execute the operations on.
		/// </param>
		public void Do(IBuffer buffer)
		{
			OldTextRanges = buffer.RaiseReplaceSelection(NewTextRange);
		}

		/// <summary>
		/// Reverses the operation on the given buffer.
		/// </summary>
		/// <param name="buffer">
		/// The buffer to execute the operations on.
		/// </param>
		public void Undo(IBuffer buffer)
		{
			buffer.RaiseRestoreSelection(OldTextRanges);
		}

		#endregion
	}
}
