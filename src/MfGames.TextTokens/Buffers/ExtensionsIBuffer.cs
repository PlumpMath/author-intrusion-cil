// <copyright file="ExtensionsIBuffer.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System.Diagnostics.Contracts;

using MfGames.TextTokens.Texts;
using MfGames.TextTokens.Tokens;

namespace MfGames.TextTokens.Buffers
{
	/// <summary>
	/// Extension methods on the IBuffer interface.
	/// </summary>
	public static class ExtensionsIBuffer
	{
		#region Public Methods and Operators

		/// <summary>
		/// Gets the token for a given location and returns it.
		/// </summary>
		/// <param name="buffer">
		/// The buffer.
		/// </param>
		/// <param name="location">
		/// The location.
		/// </param>
		/// <returns>
		/// Returns the resulting token.
		/// </returns>
		public static IToken GetToken(
			this IBuffer buffer,
			TextLocation location)
		{
			// Establish our contracts.
			Contract.Requires(buffer != null);

			// Get our token.
			IToken results = buffer.GetToken(
				location.LineIndex,
				location.TokenIndex);
			return results;
		}

		#endregion
	}
}
