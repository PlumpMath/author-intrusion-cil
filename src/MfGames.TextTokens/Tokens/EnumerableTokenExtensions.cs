// <copyright file="EnumerableTokenExtensions.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace MfGames.TextTokens.Tokens
{
	/// <summary>
	/// Includes various extensions to enumerable classes of IToken.
	/// </summary>
	public static class EnumerableTokenExtensions
	{
		#region Public Methods and Operators

		/// <summary>
		/// Gets the visible text from the collection of tokens.
		/// </summary>
		/// <param name="tokens">
		/// The tokens.
		/// </param>
		/// <returns>
		/// A string of the visible text.
		/// </returns>
		public static string GetVisibleText(this IEnumerable<IToken> tokens)
		{
			// Establish our contracts.
			Contract.Requires(tokens != null);

			// Append all of the visible strings together.
			var buffer = new StringBuilder();

			foreach (IToken token in tokens)
			{
				buffer.Append(token.Text);
			}

			return buffer.ToString();
		}

		#endregion
	}
}
