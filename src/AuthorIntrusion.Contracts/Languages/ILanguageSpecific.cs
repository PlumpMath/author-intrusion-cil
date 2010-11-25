namespace MfGames.Author.Contract.Languages
{
	/// <summary>
	/// An interface that indicates that an element is specific to one or more
	/// languages.
	/// </summary>
	public interface ILanguageSpecific
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
		string[] LanguageCodes { get; }

		#endregion
	}
}