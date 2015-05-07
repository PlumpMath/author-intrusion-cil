// <copyright file="ILine.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System.Collections.Immutable;

using MfGames.TextTokens.Tokens;

namespace MfGames.TextTokens.Lines
{
	/// <summary>
	/// Represents a single line inside the buffer. Each line consists of zero or more
	/// IToken objects, ordered from left to right.
	/// </summary>
	public interface ILine
	{
		#region Public Properties

		/// <summary>
		/// Gets the line key associated with this line.
		/// </summary>
		/// <value>
		/// The line key.
		/// </value>
		LineKey LineKey { get; }

		/// <summary>
		/// Gets an ordered list of tokens within the line.
		/// </summary>
		ImmutableList<IToken> Tokens { get; }

		#endregion
	}
}
