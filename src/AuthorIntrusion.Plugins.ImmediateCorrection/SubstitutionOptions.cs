// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System;

namespace AuthorIntrusion.Plugins.ImmediateCorrection
{
	/// <summary>
	/// Identifies the options for immediate corrections and how they are handled.
	/// </summary>
	[Flags]
	public enum SubstitutionOptions
	{
		/// <summary>
		/// Indicates no special options are given.
		/// </summary>
		None = 0,

		/// <summary>
		/// Indicates that the substitution only applies to whole words.
		/// </summary>
		WholeWord = 1,
	}
}
