// <copyright file="TokenReplacement.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

namespace MfGames.TextTokens.Events
{
	/// <summary>
	/// A boolean enumeration that describes the types of token replacements.
	/// </summary>
	public enum TokenReplacement
	{
		/// <summary>
		/// Indicates that the visible text of the tokens being replaced does not
		/// match the visible text of the replacement tokens.
		/// </summary>
		Different,

		/// <summary>
		/// Indicates that the visible text of the tokens being replaced matches
		/// the visible text of the replacement tokens.
		/// </summary>
		Identity
	}
}
