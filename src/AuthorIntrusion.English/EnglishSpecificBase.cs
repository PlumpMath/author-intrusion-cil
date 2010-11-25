#region Namespaces

using AuthorIntrusion.Contracts.Languages;

#endregion

namespace AuthorIntrusion.English
{
	/// <summary>
	/// Base class that handles the common processing for English classes.
	/// </summary>
	public abstract class EnglishSpecificBase : ILanguageSpecific
	{
		#region Languages

		/// <summary>
		/// Gets the language codes for this element. The codes are ordered in
		/// terms of most specific to least specific and correspond to ISO 639-3
		/// (3 character) and ISO 3166-1 alpha-3 codes. This is used to ensure
		/// that all the codes are identical.
		/// 
		/// For example, a parser for English may provide "eng-USA" and "eng",
		/// but one specific to only US English would only return "eng-USA".
		/// </summary>
		/// <value>The language codes.</value>
		public string[] LanguageCodes
		{
			get { return new[] { "eng" }; }
		}

		#endregion
	}
}