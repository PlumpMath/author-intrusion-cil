// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System.Collections.Generic;
using MfGames.Enumerations;

namespace AuthorIntrusion.Plugins.Spelling.Common
{
	/// <summary>
	/// Defines the signature for a controller that provides spell checking and
	/// suggestions.
	/// </summary>
	public interface ISpellingController
	{
		#region Properties

		/// <summary>
		/// Gets or sets the overall weight of the spelling control and its
		/// suggestions.
		/// </summary>
		Importance Weight { get; set; }

		#endregion

		#region Methods

		/// <summary>
		/// Gets a list of suggestions for the given word.
		/// </summary>
		/// <param name="word">The word to get suggestions for.</param>
		/// <returns>A list of <see cref="SpellingSuggestion">suggestions</see>.</returns>
		IEnumerable<SpellingSuggestion> GetSuggestions(string word);

		/// <summary>
		/// Determines whether the specified word is correct.
		/// </summary>
		/// <param name="word">The word.</param>
		/// <returns>
		///   <c>true</c> if the specified word is correct; otherwise, <c>false</c>.
		/// </returns>
		bool IsCorrect(string word);

		#endregion
	}
}
