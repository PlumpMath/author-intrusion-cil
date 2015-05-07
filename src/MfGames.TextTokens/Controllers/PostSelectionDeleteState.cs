// <copyright file="PostSelectionDeleteState.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System.Collections.Immutable;

using MfGames.TextTokens.Texts;
using MfGames.TextTokens.Tokens;

namespace MfGames.TextTokens.Controllers
{
	/// <summary>
	/// A results class that contains the state of the buffer after a selection
	/// was deleted. If there was no selection, this will contain the appropriate
	/// variables to simplify logic.
	/// </summary>
	public class PostSelectionDeleteState
	{
		#region Constructors and Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="PostSelectionDeleteState"/> class.
		/// </summary>
		/// <param name="cursor">
		/// The cursor.
		/// </param>
		/// <param name="cursorToken">
		/// The cursor token.
		/// </param>
		/// <param name="remainingTokens">
		/// The remaining tokens.
		/// </param>
		public PostSelectionDeleteState(
			TextLocation cursor,
			IToken cursorToken,
			ImmutableList<IToken> remainingTokens)
		{
			Cursor = cursor;
			CursorToken = cursorToken;
			RemainingTokens = remainingTokens;
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets the cursor position after the delete.
		/// </summary>
		/// <value>
		/// The cursor.
		/// </value>
		public TextLocation Cursor { get; private set; }

		/// <summary>
		/// Gets the token under the cursor, which may be a modified token.
		/// </summary>
		/// <value>
		/// The cursor token.
		/// </value>
		public IToken CursorToken { get; private set; }

		/// <summary>
		/// Gets the remaining tokens on the line.
		/// </summary>
		/// <value>
		/// The remaining tokens.
		/// </value>
		public ImmutableList<IToken> RemainingTokens { get; private set; }

		#endregion
	}
}
