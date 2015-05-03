// <copyright file="ITokenSplitter.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System.Collections.Generic;

namespace MfGames.TextTokens.Tokens
{
	/// <summary>
	/// Defines the interface of a token splitter, which is used to break apart lines.
	/// </summary>
	public interface ITokenSplitter
	{
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
		IEnumerable<string> Tokenize(string input);

		#endregion
	}
}
