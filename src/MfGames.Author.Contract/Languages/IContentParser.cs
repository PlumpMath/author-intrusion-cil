#region Namespaces

using MfGames.Author.Contract.Collections;
using MfGames.Author.Contract.Enumerations;
using MfGames.Author.Contract.Interfaces;

#endregion

namespace MfGames.Author.Contract.Languages
{
	/// <summary>
	/// Describes a class that can parse the unparsed contents of a paragraph
	/// and populate it a parsed sentences.
	/// </summary>
	public interface IContentParser : ILanguageSpecific
	{
		#region Parsing

		/// <summary>
		/// Parses the content of the content container and replaces the contents
		/// with parsed data.
		/// </summary>
		/// <param name="contents">The content container.</param>
		/// <returns>The status result from the parse.</returns>
		ParserStatus Parse(ContentList contents);

		#endregion
	}
}