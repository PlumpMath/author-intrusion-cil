// <copyright file="IKeyGenerator.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using MfGames.TextTokens.Lines;
using MfGames.TextTokens.Tokens;

namespace MfGames.TextTokens
{
	/// <summary>
	/// Represents a generator which creates the unique keys for tokens and lines.
	/// </summary>
	public interface IKeyGenerator
	{
		#region Public Methods and Operators

		/// <summary>
		/// Generates and retrieves the next LineKey.
		/// </summary>
		/// <returns>
		/// A LineKey that represents the next ID.
		/// </returns>
		LineKey GetNextLineKey();

		/// <summary>
		/// Generates and retrieves the next TokenKey.
		/// </summary>
		/// <returns>
		/// A TokenKey that represents the next ID.
		/// </returns>
		TokenKey GetNextTokenKey();

		#endregion
	}
}
