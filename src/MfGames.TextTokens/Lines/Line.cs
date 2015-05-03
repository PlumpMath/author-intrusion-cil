// <copyright file="Line.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.Contracts;
using System.Linq;

using MfGames.TextTokens.Events;
using MfGames.TextTokens.Tokens;

namespace MfGames.TextTokens.Lines
{
	/// <summary>
	/// A representative line object suitable for most buffers. This contains the basic
	/// functionality of ILine along with some additional methods for manipulation.
	/// </summary>
	public class Line : ILine
	{
		#region Fields

		/// <summary>
		/// The tokens contained in the line.
		/// </summary>
		private readonly TokenList<Token> tokens;

		#endregion

		#region Constructors and Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Line"/> class.
		/// </summary>
		public Line()
		{
			tokens = new TokenList<Token>();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Line"/> class.
		/// </summary>
		/// <param name="lineKey">
		/// The line key.
		/// </param>
		public Line(LineKey lineKey)
			: this()
		{
			LineKey = lineKey;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Line"/> class.
		/// </summary>
		/// <param name="line">
		/// The line.
		/// </param>
		public Line(ILine line)
			: this()
		{
			Contract.Requires(line != null);

			LineKey = line.LineKey;
		}

		#endregion

		#region Public Events

		/// <summary>
		/// Occurs when tokens are inserted into the line.
		/// </summary>
		public event EventHandler<TokenIndexTokensReplacedEventArgs> TokensReplaced;

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets the line key associated with this line.
		/// </summary>
		/// <value>
		/// The line key.
		/// </value>
		public LineKey LineKey { get; private set; }

		/// <summary>
		/// Gets an ordered list of tokens within the line.
		/// </summary>
		public TokenList<Token> Tokens { get { return tokens; } }

		#endregion

		#region Explicit Interface Properties

		/// <summary>
		/// Gets an ordered list of tokens within the line.
		/// </summary>
		ImmutableList<IToken> ILine.Tokens
		{
			get
			{
				return tokens.Cast<IToken>()
					.ToImmutableList();
			}
		}

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Appends the token to the end of the list, raising events as
		/// appropriate.
		/// </summary>
		/// <param name="newTokens">
		/// The new tokens.
		/// </param>
		public void AddTokens(params IToken[] newTokens)
		{
			// Establish our contracts.
			Contract.Requires(newTokens != null);

			// Insert the tokens.
			var afterTokenIndex = new TokenIndex(tokens.Count);
			InsertTokens(
				afterTokenIndex,
				newTokens);
		}

		/// <summary>
		/// Adds the tokens.
		/// </summary>
		/// <param name="newTokens">
		/// The new tokens.
		/// </param>
		public void AddTokens(IEnumerable<IToken> newTokens)
		{
			// Establish our contracts.
			Contract.Requires(newTokens != null);

			// Insert the tokens.
			IToken[] tokenArray = newTokens.ToArray();
			AddTokens(tokenArray);
		}

		/// <summary>
		/// Inserts a token into the token list after the given index and then
		/// raises a token inserted event.
		/// </summary>
		/// <param name="afterTokenIndex">
		/// Index of the token to insert after.
		/// </param>
		/// <param name="newTokens">
		/// The token to insert.
		/// </param>
		public void InsertTokens(
			TokenIndex afterTokenIndex,
			IEnumerable<IToken> newTokens)
		{
			// Establish our contracts.
			Contract.Requires(afterTokenIndex.Index >= 0);
			Contract.Requires(newTokens != null);

			// Insert the token into the list.
			Token[] tokenArray =
				newTokens.Select(t => t as Token ?? new Token(t))
					.ToArray();

			tokens.InsertRange(
				afterTokenIndex.Index,
				tokenArray);

			// Raise an event to indicate we've inserted a token.
			const int ReplacingNothing = 0;
			RaiseTokensInserted(
				afterTokenIndex,
				ReplacingNothing,
				tokenArray,
				TokenReplacement.Different);
		}

		#endregion

		#region Methods

		/// <summary>
		/// Raises the TokensReplaced event.
		/// </summary>
		/// <param name="tokenIndex">
		/// Index of the token.
		/// </param>
		/// <param name="count">
		/// The number of tokens to replace.
		/// </param>
		/// <param name="tokensInserted">
		/// The tokens inserted.
		/// </param>
		/// <param name="replacementType">
		/// Type of the replacement.
		/// </param>
		protected void RaiseTokensInserted(
			TokenIndex tokenIndex,
			int count,
			IEnumerable<IToken> tokensInserted,
			TokenReplacement replacementType)
		{
			// Determien if we have any listeners for this event.
			EventHandler<TokenIndexTokensReplacedEventArgs> listeners =
				TokensReplaced;

			if (listeners == null)
			{
				return;
			}

			// Create the event arguments and raise the event.
			ImmutableArray<IToken> array = tokensInserted.ToImmutableArray();
			var args = new TokenIndexTokensReplacedEventArgs(
				tokenIndex,
				count,
				array,
				replacementType);

			listeners(
				this,
				args);
		}

		#endregion
	}
}
