// <copyright file="ReplaceTokenOperation.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

using MfGames.TextTokens.Buffers;
using MfGames.TextTokens.Lines;
using MfGames.TextTokens.Texts;
using MfGames.TextTokens.Tokens;

namespace MfGames.TextTokens.Commands
{
	/// <summary>
	/// Indicates an operation that replaces a single token with a set of
	/// zero or more tokens.
	/// </summary>
	public class ReplaceTokenOperation : IBufferOperation
	{
		#region Constructors and Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="ReplaceTokenOperation"/> class.
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
		/// The new tokens.
		/// </param>
		public ReplaceTokenOperation(
			LineIndex lineIndex,
			TokenIndex tokenIndex,
			int count,
			params IToken[] replacementTokens)
		{
			LineIndex = lineIndex;
			TokenIndex = tokenIndex;
			Count = count;
			ReplacementTokens = replacementTokens;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ReplaceTokenOperation"/> class.
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
		/// The replacement tokens.
		/// </param>
		public ReplaceTokenOperation(
			LineIndex lineIndex,
			TokenIndex tokenIndex,
			int count,
			IEnumerable<IToken> replacementTokens)
		{
			// Establish our contracts.
			Contract.Requires(replacementTokens != null);

			// Save our member variables.
			LineIndex = lineIndex;
			TokenIndex = tokenIndex;
			Count = count;
			ReplacementTokens = replacementTokens.ToArray();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ReplaceTokenOperation"/> class.
		/// </summary>
		/// <param name="location">
		/// The location.
		/// </param>
		/// <param name="count">
		/// The count.
		/// </param>
		/// <param name="newToken">
		/// The new token.
		/// </param>
		public ReplaceTokenOperation(
			TextLocation location,
			int count,
			IToken newToken)
			: this(location.LineIndex,
				location.TokenIndex,
				count,
				newToken)
		{
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets the count.
		/// </summary>
		/// <value>
		/// The count.
		/// </value>
		public int Count { get; private set; }

		/// <summary>
		/// Gets or sets the index of the line.
		/// </summary>
		/// <value>
		/// The index of the line.
		/// </value>
		public LineIndex LineIndex { get; set; }

		/// <summary>
		/// Gets the tokens that were replaced or null if the operation has not been
		/// completed.
		/// </summary>
		public IToken[] ReplacedTokens { get; private set; }

		/// <summary>
		/// Gets the new tokens.
		/// </summary>
		/// <value>
		/// The new tokens.
		/// </value>
		public IToken[] ReplacementTokens { get; private set; }

		/// <summary>
		/// Gets the index of the token.
		/// </summary>
		/// <value>
		/// The index of the token.
		/// </value>
		public TokenIndex TokenIndex { get; private set; }

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Replaces the OldToken with the NewTokens.
		/// </summary>
		/// <param name="buffer">
		/// The buffer to execute the operations on.
		/// </param>
		public void Do(IBuffer buffer)
		{
			IEnumerable<IToken> replacedTokens =
				buffer.ReplaceTokens(
					LineIndex,
					TokenIndex,
					Count,
					ReplacementTokens);

			ReplacedTokens = replacedTokens.ToArray();
		}

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return string.Format(
				"ReplaceTokenOperation(({0}, {1}x{2}), {3})",
				LineIndex.Index,
				TokenIndex,
				Count,
				ReplacementTokens.Length);
		}

		/// <summary>
		/// Reverses the operation on the given buffer.
		/// </summary>
		/// <param name="buffer">
		/// The buffer to operate on.
		/// </param>
		public void Undo(IBuffer buffer)
		{
			buffer.ReplaceTokens(
				LineIndex,
				TokenIndex,
				ReplacementTokens.Length,
				ReplacedTokens);
		}

		#endregion
	}
}
