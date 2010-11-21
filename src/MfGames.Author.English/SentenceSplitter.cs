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
	public class SentenceSplitter
	{
		/// <summary>
		/// Splits the content list into one or more sentences.
		/// </summary>
		/// <param name="contents">The contents.</param>
		/// <returns></returns>
		public static List<ContentList> Split(ContentList contents)
		{
			// Create a list of individual sentences.
			List<ContentList> sentences = new List<ContentList>();

			// Go through each of the items inside the contents list and break
			// them into words, puncuation, and quotes.
			ContentList tokenizedContents = Tokenize(contents);

			// Split the sentence with whitespace and the known puncuation marks.

			// Start at the beginning and look for puncuation that would indicate
			// the end of a sentence.
			ContentList current = new ContentList();
			sentences.Add(current);
			
			// Return the resulting sentences.
			return sentences;
		}

		/// <summary>
		/// Takes the list of contents and break them into the various content
		/// elements.
		/// </summary>
		/// <param name="contents">The contents.</param>
		/// <returns></returns>
		private static ContentList Tokenize(ContentList contents)
		{
			// Create a list of tokens. We'll be moving content elements into it
			// as they are parsed from left to right.
			ContentList tokens = new ContentList();

			//// Parse through the contents.
			//foreach (ContentBase content in contents)
			//{
			//    // If the content is already parsed, then just move it on.
			//    if (content.IsTokenized)
			//    {
			//        tokens.Add(content);
			//        continue;
			//    }

			//}

			// Return the resulting list of tokens.
			return tokens;
		}
	}
}
