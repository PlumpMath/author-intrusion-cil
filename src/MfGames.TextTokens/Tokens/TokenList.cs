// <copyright file="TokenList.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MfGames.TextTokens.Tokens
{
	/// <summary>
	/// Extends List&lt;IToken&gt; to include token-specific operations.
	/// </summary>
	/// <typeparam name="TToken">
	/// The class that extends IToken.
	/// </typeparam>
	public class TokenList<TToken> : List<TToken>
		where TToken : IToken
	{
		#region Public Indexers

		/// <summary>
		/// Gets the <see cref="TToken"/> with the specified key.
		/// </summary>
		/// <value>
		/// The <see cref="TToken"/>.
		/// </value>
		/// <param name="key">
		/// The key.
		/// </param>
		/// <returns>
		/// The first token in the list that matches the key.
		/// </returns>
		/// <exception cref="IndexOutOfRangeException">
		/// Thrown when the key cannot be found.
		/// </exception>
		public TToken this[TokenKey key]
		{
			get { return this.First(t => t.TokenKey == key); }
		}

		/// <summary>
		/// Gets the <see cref="TToken"/> at the specified index.
		/// </summary>
		/// <value>
		/// The <see cref="TToken"/>.
		/// </value>
		/// <param name="index">
		/// The index.
		/// </param>
		/// <returns>
		/// A token at the given index.
		/// </returns>
		public TToken this[TokenIndex index] { get { return this[index.Index]; } }

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Gets the visible text, built up from the tokens.
		/// </summary>
		/// <returns>A non-null string containing the visible token's text.</returns>
		public string GetVisibleText()
		{
			var buffer = new StringBuilder();

			foreach (TToken token in this)
			{
				buffer.Append(token.Text);
			}

			return buffer.ToString();
		}

		#endregion
	}
}
