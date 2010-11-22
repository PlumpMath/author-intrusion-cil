#region Namespaces

using System.Text.RegularExpressions;

using MfGames.Author.Contract.Contents;
using MfGames.Author.Contract.Collections;
using MfGames.Author.Contract.Languages;

#endregion

namespace MfGames.Author.English
{
	/// <summary>
	/// Parses content items, including unparsed content, a convert the various
	/// contents into parsed versions.
	/// </summary>
	public class EnglishContentSplitter : EnglishSpecificBase, IContentSplitter
	{
		#region Constants

		/// <summary>
		/// Contains the regex used to split tokens on word barriers.
		/// </summary>
		private static readonly Regex SplitRegex = new Regex(
			@"([\.!\?]|\s+)", 
			RegexOptions.Compiled);

		#endregion

		#region Splitting

		/// <summary>
		/// Splits the specified contents into parsed components.
		/// </summary>
		/// <param name="contents">The contents.</param>
		/// <returns></returns>
		public ContentList SplitContents(ContentList contents)
		{
			// Build up a list of parsed contents.
			ContentList parsed = new ContentList();

			// Go through each of the individual contents passed into the method.
			foreach (Content content in contents)
			{
				if (content is Quote)
				{
					// Quotes are considered parse, but they may contain
					// unparsed content.
					Quote quote = (Quote) content;

					if (!quote.IsParsed)
					{
					    quote.SplitContents(this);
					}

					// Add the quote to the list.
					parsed.Add(quote);
					continue;
				}

				if (content is Unparsed)
				{
					// Unparsed strings require splitting into elements.
					Unparsed unparsed = (Unparsed) content;

					// Split the parsed content and add it to the list.
					parsed.AddRange(ParseString(unparsed.Contents));
					continue;
				}

				// If we got this far, we assume everything is properly parsed
				// and just add it to the list.
				parsed.Add(content);
			}

			// Return the resulting contents.
			return parsed;
		}

		/// <summary>
		/// Parses the string and populates the content list.
		/// </summary>
		/// <param name="content">The content.</param>
		/// <returns></returns>
		internal ContentList ParseString(string content)
		{
			// Create a list of content to place the results.
			ContentList parsed = new ContentList();

			// Split the content on the various word and sentence breaks. This is
			// a rather simplified version of parsing and could be changed later
			// to use more elegant methods to handle things like "U.S." which is
			// considered a single token.

			string[] tokens = SplitRegex.Split(content);

			foreach (string token in tokens)
			{
				// We don't tokenize blank strings since those are the whitespace
				// between the various tokens.
				if (token.Trim().Length == 0)
				{
					continue;
				}

				// Break the remaining element into content objects.
				switch (token)
				{

					case ".":
					case "!":
					case "?":
						parsed.Add(new Terminator(token));
						break;
					default:
						parsed.Add(new Word(token));
						break;
				}
			}

			// Return the resulting list.
			return parsed;
		}

		#endregion
	}
}
