// <copyright file="DefaultTokenSplitter.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System.Collections.Generic;
using System.Text;

namespace MfGames.TextTokens.Tokens
{
	/// <summary>
	/// Implements a simplistic token parser that splits on whitespace and punctuation
	/// characters.
	/// </summary>
	public class DefaultTokenSplitter : ITokenSplitter
	{
		#region Fields

		/// <summary>
		/// Contains the list of separators that identify a token break.
		/// </summary>
		private readonly char[] tokenBreaks =
		{
			' ', '\t', '.', ';', ':', '\'', '"'
		};

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Tokenizes the specified input and returns a list of individual tokens.
		/// </summary>
		/// <param name="input">
		/// The input.
		/// </param>
		/// <returns>
		/// An enumeration of string tokens, starting from left to right.
		/// </returns>
		public IEnumerable<string> Tokenize(string input)
		{
			// If we have a null, then return a blank string.
			if (input == null)
			{
				return new string[0] { };
			}

			// Build up a list of tokens as we look through the string.
			var tokens = new List<string>();
			var token = new StringBuilder();

			for (var index = 0; index < input.Length; index++)
			{
				// Pull out the character we're processing.
				char test = input[index];

				// Figure out if we are going to add this character to the current token
				// or the next one. If the token is currently zero, we always add. Otherwise,
				// we append to the current token if it is the same category (character to
				// character or whitespace to whitespace), or it is is punctuation which is
				// never bound to the same token.
				if (token.Length == 0)
				{
					// Always add this.
					token.Append(test);
					continue;
				}

				// If we are a whitespace character and the token is a whitespace token,
				// then we append it. Otherwise, it will be a new token.
				char tokenCharacter = token[0];

				if (char.IsWhiteSpace(test))
				{
					if (char.IsWhiteSpace(tokenCharacter))
					{
						token.Append(test);
						continue;
					}
				}
				else if (char.IsPunctuation(test))
				{
					// Punctuation is always a unique token.
				}
				else
				{
					// If the current token is not punctuation or whitespace, append it.
					if (!char.IsWhiteSpace(tokenCharacter)
						&& !char.IsPunctuation(tokenCharacter))
					{
						token.Append(test);
						continue;
					}
				}

				// In all other cases, we are creating a new token.
				tokens.Add(token.ToString());
				token.Length = 0;
				token.Append(test);
			}

			// If we have a non-zero length token, add it.
			if (token.Length > 0)
			{
				tokens.Add(token.ToString());
			}

			// Return the resulting tokens.
			return tokens;
		}

		#endregion
	}
}
