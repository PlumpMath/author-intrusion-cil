// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

namespace AuthorIntrusion.Plugins.Counter
{
	/// <summary>
	/// A static class that implements a basic word counter. This is intended to
	/// duplicate the word counting logic of Microsoft Words 2010.
	/// </summary>
	public static class WordCounterHelper
	{
		#region Methods

		/// <summary>
		/// Counts the words in a text and returns the various counts.
		/// </summary>
		/// <param name="text">The text to measure.</param>
		/// <param name="wordCount">The word count.</param>
		/// <param name="characterCount">The character count.</param>
		/// <param name="nonWhitespaceCount">The non whitespace count.</param>
		public static void CountWords(
			string text,
			out int wordCount,
			out int characterCount,
			out int nonWhitespaceCount)
		{
			// Initialize the variables.
			wordCount = 0;
			characterCount = 0;
			nonWhitespaceCount = 0;

			// Go through the characters in the string.
			bool lastWasWhitespace = true;

			foreach (char c in text)
			{
				// Count the number of characters in the string.
				characterCount++;

				// If we are whitespace, then we set a flag to identify the beginning
				// of the next word.
				if (char.IsWhiteSpace(c))
				{
					lastWasWhitespace = true;
				}
				else
				{
					// This is a non-whitespace character.
					nonWhitespaceCount++;

					// If the last character was a whitespace, then we bump up the
					// words.
					if (lastWasWhitespace)
					{
						wordCount++;
					}

					lastWasWhitespace = false;
				}
			}
		}

		/// <summary>
		/// Counts the words in the given text.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <returns>The number of words in the string.</returns>
		public static int CountWords(string text)
		{
			int wordCount;
			int characterCount;
			int nonWhitespaceCount;
			CountWords(text, out wordCount, out characterCount, out nonWhitespaceCount);

			return wordCount;
		}

		#endregion
	}
}
