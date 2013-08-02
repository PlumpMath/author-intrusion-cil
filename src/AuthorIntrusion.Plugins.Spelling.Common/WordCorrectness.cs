// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

namespace AuthorIntrusion.Plugins.Spelling.Common
{
	/// <summary>
	/// Defines the correcteness of a given word for a plugin.
	/// </summary>
	public enum WordCorrectness: byte
	{
		/// <summary>
		/// Indicates that the corrected cannot be determined by the given
		/// spelling plugin.
		/// </summary>
		Indeterminate,

		/// <summary>
		/// Indicates that the plugin has determined the word was correctly
		/// spelled.
		/// </summary>
		Correct,

		/// <summary>
		/// Indicates that the plugin has determined that the word was
		/// incorrect and needs correction.
		/// </summary>
		Incorrect,
	}
}
