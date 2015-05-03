// <copyright file="RestoreSelectionEventArgs.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

using MfGames.TextTokens.Texts;

namespace MfGames.TextTokens.Events
{
	/// <summary>
	/// Event arguments for restoring the selection after a change.
	/// </summary>
	public class RestoreSelectionEventArgs : EventArgs
	{
		#region Constructors and Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="RestoreSelectionEventArgs"/> class.
		/// </summary>
		/// <param name="previousTextRanges">
		/// The previous text ranges.
		/// </param>
		public RestoreSelectionEventArgs(
			Dictionary<object, TextRange> previousTextRanges)
		{
			Contract.Requires(previousTextRanges != null);

			PreviousTextRanges = previousTextRanges;
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets the previous text ranges, with the source as the key.
		/// </summary>
		/// <value>
		/// The previous text ranges.
		/// </value>
		public Dictionary<object, TextRange> PreviousTextRanges { get; private set; }

		#endregion
	}
}
