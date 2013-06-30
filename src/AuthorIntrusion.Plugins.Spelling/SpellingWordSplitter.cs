// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System.Collections.Generic;
using AuthorIntrusion.Common.Blocks;

namespace AuthorIntrusion.Plugins.Spelling
{
	/// <summary>
	/// Specialized splitter class that normalizes input and breaks each word apart
	/// in a format usable by the spelling framework.
	/// </summary>
	public class SpellingWordSplitter
	{
		#region Methods

		/// <summary>
		/// Splits and normalizes the string for processing spell-checking.
		/// </summary>
		/// <param name="line">The line.</param>
		/// <returns>A list of normalized words.</returns>
		public IList<TextSpan> SplitAndNormalize(string line)
		{
			// Because we need to keep track of the start and stop text indexes,
			// we loop through the string once and keep track of the last text span
			// we are populating.
			var textSpans = new List<TextSpan>();
			TextSpan textSpan = null;

			for (int index = 0;
				index < line.Length;
				index++)
			{
				// Grab the character for this line.
				char c = line[index];

				// Normalize some Unicode characters.
				switch (c)
				{
					case '\u2018':
					case '\u2019':
						c = '\'';
						break;
					case '\u201C':
					case '\u201D':
						c = '"';
						break;
				}

				// We need to determine if this is a word break or not. Word breaks
				// happen with whitespace and punctuation, but there are special rules
				// with single quotes.
				bool isWordBreak = char.IsWhiteSpace(c) || char.IsPunctuation(c);

				if (c == '\'')
				{
					// For single quotes, it is considered not a word break if there is
					// a letter or digit on both sides of the quote.
					bool isLetterBefore = index > 0 && char.IsLetterOrDigit(line[index - 1]);
					bool isLetterAfter = index < line.Length - 1
						&& char.IsLetterOrDigit(line[index + 1]);

					isWordBreak = !isLetterAfter || !isLetterBefore;
				}

				// What we do is based on if we have a word break or not.
				if (isWordBreak)
				{
					// If we have a text span, we need to finish it off.
					if (textSpan != null)
					{
						textSpan.StopTextIndex = index;
						textSpan = null;
					}
				}
				else
				{
					// If we don't have a text span, then we need to create a new one.
					// Otherwise, we bump up the index on the span.
					if (textSpan == null)
					{
						// Create a new text span.
						textSpan = new TextSpan
						{
							StartTextIndex = index,
							StopTextIndex = index
						};

						// Add it to the collection.
						textSpans.Add(textSpan);
					}
					else
					{
						// Update the existing text span.
						textSpan.StopTextIndex = index;
					}
				}
			}

			// Finish up the last span, if we had one.
			if (textSpan != null)
			{
				textSpan.StopTextIndex++;
			}

			// Return the list of text spans. We don't have to populate the strings
			// because the calling class will do that for us.
			return textSpans;
		}

		#endregion
	}
}
