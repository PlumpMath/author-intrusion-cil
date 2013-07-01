// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

namespace AuthorIntrusion.Plugins.Spelling.Common
{
	/// <summary>
	/// Describes a spelling suggestion including a word and its given weight.
	/// </summary>
	public class SpellingSuggestion
	{
		#region Properties

		/// <summary>
		/// Gets or sets the suggested word.
		/// </summary>
		public string Suggestion { get; set; }

		/// <summary>
		/// Gets or sets the weight in the range of -4 to +4. When sorting suggestions
		/// for display to the user, the higher weighted suggestions will go first. In
		/// most cases, a weight should only be -1 to +1.
		/// </summary>
		public int Weight { get; set; }

		#endregion

		#region Constructors

		public SpellingSuggestion(
			string suggestedWord,
			int weight = 0)
		{
			Suggestion = suggestedWord;
			Weight = weight;
		}

		#endregion
	}
}
