// <copyright file="IToken.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

namespace MfGames.TextTokens.Tokens
{
	/// <summary>
	/// <para>Represents a single token within a line. Most tokens are text tokens
	/// which allow users to edit them, but tokens can also be used to represent
	/// ruby text (text above and below another text), read-only sections, or
	/// specially formatted codes. A token's TokenKey or Text will never change.</para>
	/// <para>To avoid memory pressure, a token may actually be the internal object,
	/// however because tokens are effectively immutable outside of two specific
	/// calls to the Buffer (ExecuteUserCommand and ExecuteBackgroundCommands).</para>
	/// </summary>
	public interface IToken
	{
		#region Public Properties

		/// <summary>
		/// Gets the text associated with this token.
		/// </summary>
		/// <value>
		/// The text.
		/// </value>
		string Text { get; }

		/// <summary>
		/// Gets the token key associated with the token.
		/// </summary>
		/// <value>
		/// The token key.
		/// </value>
		TokenKey TokenKey { get; }

		#endregion
	}
}
