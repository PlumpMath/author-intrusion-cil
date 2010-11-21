#region Namespaces

using MfGames.Author.Contract.Structures;

#endregion

namespace MfGames.Author.Contract.Languages
{
	/// <summary>
	/// Describes a class that can parse the unparsed contents of a paragraph
	/// and populate it a parsed sentences.
	/// </summary>
	public interface IParagraphParser : ILanguageSpecific
	{
		#region Parsing

		/// <summary>
		/// Parses the unparsed contents of the specified paragraph and puts the
		/// parsed sentences. The unparsed contents will be cleared by the
		/// calling method and must not be altered by this parser.
		/// </summary>
		/// <param name="paragraph">The paragraph with unparsed contents.</param>
		/// <returns>True if successfully parsed, false if not.</returns>
		void Parse(Paragraph paragraph);

		#endregion
	}
}