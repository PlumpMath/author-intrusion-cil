// <copyright file="TokenIndexTokensReplacedEventArgs.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System.Collections.Immutable;

using MfGames.TextTokens.Tokens;

namespace MfGames.TextTokens.Events
{
	/// <summary>
	/// Indicates an event where zero or more tokens are replaced by zero or more tokens. This
	/// is used to delete, insert, modify, split, or join tokens together. There is no line
	/// contextual information with this event.
	/// </summary>
	public class TokenIndexTokensReplacedEventArgs : TokenIndexEventArgs
	{
		#region Constructors and Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="TokenIndexTokensReplacedEventArgs"/> class.
		/// </summary>
		/// <param name="tokenIndex">
		/// Index of the token.
		/// </param>
		/// <param name="count">
		/// The count.
		/// </param>
		/// <param name="replacementTokens">
		/// The tokens inserted.
		/// </param>
		/// <param name="replacementType">
		/// Type of the replacement.
		/// </param>
		public TokenIndexTokensReplacedEventArgs(
			TokenIndex tokenIndex,
			int count,
			ImmutableArray<IToken> replacementTokens,
			TokenReplacement replacementType)
			: base(tokenIndex)
		{
			Count = count;
			TokensInserted = replacementTokens;
			ReplacementType = replacementType;
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets the number of tokens replaced.
		/// </summary>
		/// <value>
		/// The count.
		/// </value>
		public int Count { get; private set; }

		/// <summary>
		/// Gets the type of the token replacement.
		/// </summary>
		/// <value>
		/// The type of the replacement.
		/// </value>
		public TokenReplacement ReplacementType { get; private set; }

		/// <summary>
		/// Gets the tokens inserted by this event.
		/// </summary>
		/// <value>
		/// The tokens inserted.
		/// </value>
		public ImmutableArray<IToken> TokensInserted { get; private set; }

		#endregion
	}
}
