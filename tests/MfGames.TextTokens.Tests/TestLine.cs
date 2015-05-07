// <copyright file="TestLine.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System.Collections.Generic;
using System.Diagnostics.Contracts;

using MfGames.TextTokens.Lines;
using MfGames.TextTokens.Tokens;

namespace MfGames.TextTokens.Tests
{
	/// <summary>
	/// Contains state information about a line in the buffer.
	/// </summary>
	public class TestLine
	{
		#region Constructors and Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="TestLine"/> class.
		/// </summary>
		/// <param name="lineKey">
		/// The line key.
		/// </param>
		public TestLine(LineKey lineKey)
		{
			Tokens = new TokenList<TestToken>();
			LineKey = lineKey;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TestLine"/> class.
		/// </summary>
		/// <param name="line">
		/// The line.
		/// </param>
		public TestLine(ILine line)
			: this(line.LineKey)
		{
			// Establish our contracts.
			Contract.Requires(line != null);

			// Add in all the tokens already existing on the line.
			foreach (IToken token in line.Tokens)
			{
				var testToken = new TestToken(token);
				Tokens.Add(testToken);
			}
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets the line key.
		/// </summary>
		/// <value>
		/// The line key.
		/// </value>
		public LineKey LineKey { get; private set; }

		/// <summary>
		/// Gets the tokens.
		/// </summary>
		/// <value>
		/// The tokens.
		/// </value>
		public TokenList<TestToken> Tokens { get; private set; }

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Inserts the tokens.
		/// </summary>
		/// <param name="tokenIndex">
		/// Index of the token.
		/// </param>
		/// <param name="tokensInserted">
		/// The tokens inserted.
		/// </param>
		public void InsertTokens(
			TokenIndex tokenIndex,
			IEnumerable<IToken> tokensInserted)
		{
			// Establish our contracts.
			Contract.Requires(tokensInserted != null);

			// First wrap the tokens in a test token layer.
			var tokens = new List<TestToken>();

			foreach (IToken token in tokensInserted)
			{
				var testToken = new TestToken(token);
				tokens.Add(testToken);
			}

			// Insert the tokens into the list.
			Tokens.InsertRange(
				tokenIndex.Index,
				tokens);
		}

		#endregion
	}
}
