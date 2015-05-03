// <copyright file="LineIndexTokenIndexTokensReplacedEventArgs.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System.Collections.Immutable;
using System.Diagnostics.Contracts;

using MfGames.TextTokens.Lines;
using MfGames.TextTokens.Tokens;

namespace MfGames.TextTokens.Events
{
	/// <summary>
	/// Indicates an event where one or more tokens are replaced with zero or more tokens.
	/// This can be used to merge tokens together, split them apart, or replace a single token
	/// with the contents of a new one. This also can be used to insert tokens via a zero count
	/// replacement.
	/// </summary>
	public class LineIndexTokenIndexTokensReplacedEventArgs :
		LineIndexTokenIndexEventArgs
	{
		#region Constructors and Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="LineIndexTokenIndexTokensReplacedEventArgs"/> class.
		/// </summary>
		/// <param name="lineIndex">
		/// Index of the line.
		/// </param>
		/// <param name="tokenIndex">
		/// Index of the token.
		/// </param>
		/// <param name="count">
		/// The count.
		/// </param>
		/// <param name="replacementTokens">
		/// The token replacements.
		/// </param>
		/// <param name="replacementType">
		/// Type of the replacement.
		/// </param>
		public LineIndexTokenIndexTokensReplacedEventArgs(
			LineIndex lineIndex,
			TokenIndex tokenIndex,
			int count,
			ImmutableArray<IToken> replacementTokens,
			TokenReplacement replacementType)
			: base(lineIndex,
				tokenIndex)
		{
			// Establish our contracts.
			Contract.Requires(count >= 0);

			// Save the member variables.
			Count = count;
			ReplacementTokens = replacementTokens;
			ReplacementType = replacementType;
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets the number of tokens to replace starting at TokenIndex.
		/// </summary>
		/// <value>
		/// The count.
		/// </value>
		public int Count { get; private set; }

		/// <summary>
		/// Gets the tokens that will replace the given token.
		/// </summary>
		/// <value>
		/// The token replacements.
		/// </value>
		public ImmutableArray<IToken> ReplacementTokens { get; private set; }

		/// <summary>
		/// Gets the type of the token replacement.
		/// </summary>
		/// <value>
		/// The type of the replacement.
		/// </value>
		public TokenReplacement ReplacementType { get; private set; }

		#endregion
	}
}
