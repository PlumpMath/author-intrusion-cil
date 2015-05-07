// <copyright file="LineIndexEventArgs.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System;

using MfGames.TextTokens.Lines;

namespace MfGames.TextTokens.Events
{
	/// <summary>
	/// Base class for an event that refers to a single line within a buffer.
	/// </summary>
	public class LineIndexEventArgs : EventArgs
	{
		#region Constructors and Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="LineIndexEventArgs"/> class.
		/// </summary>
		/// <param name="lineIndex">
		/// Index of the line.
		/// </param>
		public LineIndexEventArgs(LineIndex lineIndex)
		{
			LineIndex = lineIndex;
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets the index of the line.
		/// </summary>
		/// <value>
		/// The index of the line.
		/// </value>
		public LineIndex LineIndex { get; private set; }

		#endregion
	}
}
