// <copyright file="TokenIndexEventArgs.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System;

using MfGames.TextTokens.Tokens;

namespace MfGames.TextTokens.Events
{
	/// <summary>
	/// Base class for events that perform an action at a specific line and token index.
	/// </summary>
	public abstract class TokenIndexEventArgs : EventArgs
	{
		#region Constructors and Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="TokenIndexEventArgs"/> class.
		/// </summary>
		/// <param name="tokenIndex">
		/// Index of the token.
		/// </param>
		protected TokenIndexEventArgs(TokenIndex tokenIndex)
		{
			TokenIndex = tokenIndex;
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets the index of the token.
		/// </summary>
		/// <value>
		/// The index of the token.
		/// </value>
		public TokenIndex TokenIndex { get; private set; }

		#endregion
	}
}
