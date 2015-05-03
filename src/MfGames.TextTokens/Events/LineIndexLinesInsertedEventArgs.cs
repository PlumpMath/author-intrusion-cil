// <copyright file="LineIndexLinesInsertedEventArgs.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System.Collections.Generic;

using MfGames.TextTokens.Lines;

namespace MfGames.TextTokens.Events
{
	/// <summary>
	/// Represents an event where one or more lines are inserted into a buffer
	/// after a given index. If the index is 0, then these lines are prepending to
	/// the buffer. If they equal the count of the buffer's line, it is an append.
	/// </summary>
	public class LineIndexLinesInsertedEventArgs : LineIndexEventArgs
	{
		#region Constructors and Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="LineIndexLinesInsertedEventArgs"/> class.
		/// </summary>
		/// <param name="lineIndex">
		/// Index of the line.
		/// </param>
		/// <param name="linesInserted">
		/// The lines inserted.
		/// </param>
		public LineIndexLinesInsertedEventArgs(
			LineIndex lineIndex,
			IReadOnlyList<ILine> linesInserted)
			: base(lineIndex)
		{
			LinesInserted = linesInserted;
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets the lines inserted after the given index.
		/// </summary>
		/// <value>
		/// The lines inserted.
		/// </value>
		public IReadOnlyList<ILine> LinesInserted { get; private set; }

		#endregion
	}
}
