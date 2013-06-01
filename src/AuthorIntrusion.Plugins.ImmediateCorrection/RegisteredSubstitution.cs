// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

namespace AuthorIntrusion.Plugins.ImmediateCorrection
{
	/// <summary>
	/// A substitution that was registered with the system in one or more
	/// configurations.
	/// </summary>
	public class RegisteredSubstitution
	{
		#region Properties

		public bool IsWholeWord
		{
			get { return (Options & SubstitutionOptions.WholeWord) != 0; }
		}

		#endregion

		#region Constructors

		public RegisteredSubstitution(
			string search,
			string replacement,
			SubstitutionOptions options)
		{
			// Save the fields for the substitution.
			Search = search;
			Replacement = replacement;
			Options = options;
		}

		#endregion

		#region Fields

		public readonly SubstitutionOptions Options;
		public readonly string Replacement;
		public readonly string Search;

		#endregion
	}
}
