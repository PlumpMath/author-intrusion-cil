#region Namespaces

using System;
using System.Collections.Generic;

using MfGames.Author.Contract.Contents;
using MfGames.Author.Contract.Contents.Collections;

#endregion

namespace MfGames.Author.English
{
	/// <summary>
	/// Implements a sentence splitter and
	/// </summary>
	public class EnglishSentenceSplitter
	{
		/// <summary>
		/// Splits the content list into one or more sentences.
		/// </summary>
		/// <param name="contents">A list of parsed content elements.</param>
		/// <returns></returns>
		public static List<ContentList> SplitSentences(ContentList contents)
		{
			// Create a list of individual sentences.
			List<ContentList> sentences = new List<ContentList>();

			// Start at the beginning and look for puncuation that would indicate
			// the end of a sentence.
			ContentList current = new ContentList();
			sentences.Add(current);

			foreach (Content content in contents)
			{
				// Add the content to the current sentence.
				current.Add(content);

				// Check for a sentence terminator.
				bool hasTerminator = false;

				if (content is Terminator)
				{
					hasTerminator = true;
				}

				// If the sentence has a quote, see if we have a terminator
				// at the end of that quote.
				if (content is Quote)
				{
					Quote quote = (Quote) content;
					hasTerminator = quote.EndsWithTerminator;
				}

				// We are at the end of the sentence. Start a new one.
				if (hasTerminator)
				{
					current = new ContentList();
					sentences.Add(current);
				}
			}

			// If the current sentence is blank, remove it.
			if (current.Count == 0)
			{
				// The current one is always at the end of the list.
				sentences.RemoveAt(sentences.Count - 1);
			}

			// Return the resulting sentences.
			return sentences;
		}
	}
}
