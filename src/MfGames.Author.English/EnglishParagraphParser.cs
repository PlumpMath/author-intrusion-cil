#region Namespaces

using System;
using System.Collections.Generic;

using MfGames.Author.Contract.Contents;
using MfGames.Author.Contract.Collections;
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
		public void Parse(ContentContainerStructure paragraph)
		{
			// Pull out the paragraphs' unparsed content.
			ContentList unparsed = paragraph.UnparsedContents;

			// Split the input into parsed content.
			EnglishContentSplitter contentSplitter = new EnglishContentSplitter();
			ContentList parsed = contentSplitter.SplitContents(unparsed);

			// Split the sentences out and put them back into the paragraph.
			List<ContentList> sentences = EnglishSentenceSplitter.SplitSentences(parsed);

			foreach (ContentList sentenceContents in sentences)
			{
				Sentence sentence = new Sentence();
				sentence.AddRange(sentenceContents);
				paragraph.Sentences.Add(sentence);
			}
		}

		#endregion
	}
}