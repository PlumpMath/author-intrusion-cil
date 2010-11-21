#region Namespaces

using System;

using MfGames.Author.Contract.Languages;
using MfGames.Author.Contract.Structures;

#endregion

namespace MfGames.Author.English
{
	/// <summary>
	/// Implements a basic paragraph parser that goes through the unparsed
	/// contents and breaks it into sentence elements.
	/// </summary>
	public class EnglishParagraphParser : EnglishSpecificBase, IParagraphParser
	{
		#region Parsing

		/// <summary>
		/// Parses the unparsed contents of the specified paragraph and puts the
		/// parsed sentences. The unparsed contents will be cleared by the
		/// calling method and must not be altered by this parser.
		/// </summary>
		/// <param name="paragraph">The paragraph with unparsed contents.</param>
		/// <returns>True if successfully parsed, false if not.</returns>
		public bool Parse(Paragraph paragraph)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}