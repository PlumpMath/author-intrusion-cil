// <copyright file="InsertLinesOperation.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using MfGames.TextTokens.Buffers;
using MfGames.TextTokens.Lines;

namespace MfGames.TextTokens.Commands
{
	/// <summary>
	/// Represents an operation that inserts lines into the buffer.
	/// </summary>
	public class InsertLinesOperation : IBufferOperation
	{
		#region Constructors and Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="InsertLinesOperation"/> class.
		/// </summary>
		/// <param name="lineIndex">
		/// Index of the line.
		/// </param>
		/// <param name="count">
		/// The count.
		/// </param>
		public InsertLinesOperation(
			LineIndex lineIndex,
			int count)
		{
			LineIndex = lineIndex;
			Count = count;
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets the number of lines to insert.
		/// </summary>
		/// <value>
		/// The count.
		/// </value>
		public int Count { get; }

		/// <summary>
		/// Gets the index of the line to insert after.
		/// </summary>
		/// <value>
		/// The index of the line.
		/// </value>
		public LineIndex LineIndex { get; }

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
			buffer.InsertLines(
				LineIndex,
				Count);
		}

		/// <summary>
		/// Reverses the operation on the given buffer.
		/// </summary>
		/// <param name="buffer">
		/// The buffer to execute the operations on.
		/// </param>
		public void Undo(IBuffer buffer)
		{
			buffer.DeleteLines(
				LineIndex,
				Count);
		}

		#endregion
	}
}
