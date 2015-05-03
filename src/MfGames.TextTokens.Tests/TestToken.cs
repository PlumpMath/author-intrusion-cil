// <copyright file="TestToken.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System.Diagnostics.Contracts;

using MfGames.TextTokens.Tokens;

namespace MfGames.TextTokens.Tests
{
	/// <summary>
	/// Reflects the state of a token inside the buffer.
	/// </summary>
	public class TestToken : IToken
	{
		#region Constructors and Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="TestToken"/> class.
		/// </summary>
		/// <param name="tokenKey">
		/// The token key.
		/// </param>
		/// <param name="text">
		/// The text.
		/// </param>
		public TestToken(
			TokenKey tokenKey,
			string text)
		{
			TokenKey = tokenKey;
			Text = text;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TestToken"/> class.
		/// </summary>
		/// <param name="token">
		/// The token.
		/// </param>
		public TestToken(IToken token)
			: this(token.TokenKey,
				token.Text)
		{
			// Establish our contracts.
			Contract.Requires(token != null);
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets the text of the token.
		/// </summary>
		public string Text { get; private set; }

		/// <summary>
		/// Gets the key for the token.
		/// </summary>
		public TokenKey TokenKey { get; private set; }

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return string.Format(
				"({1}, \"{0}\")",
				Text,
				TokenKey.Id);
		}

		#endregion
	}
}
