#if DISABLED
#region Namespaces

using System;
using System.Text.RegularExpressions;

using AuthorIntrusion.Contracts.Collections;
using AuthorIntrusion.Contracts.Contents;
using AuthorIntrusion.Contracts.Enumerations;
using AuthorIntrusion.Contracts.Interfaces;
using AuthorIntrusion.Contracts.Languages;

#endregion

namespace AuthorIntrusion.English
{
	/// <summary>
	/// Parses content items, including unparsed content, a convert the various
	/// contents into parsed versions.
	/// </summary>
	public class EnglishUnparsedSplitter : EnglishSpecificBase, IContentParser
	{
		#region Constants

		/// <summary>
		/// Contains the regex used to split tokens on word barriers.
		/// </summary>
		private static readonly Regex SplitRegex = new Regex(
			@"([\.!\?:;\,]|\s+)", RegexOptions.Compiled);

		#endregion

		#region Parsing

		/// <summary>
		/// Parses the content of the content container and replaces the contents
		/// with parsed data.
		/// </summary>
		/// <param name="contents">The content container.</param>
		/// <returns>The status result from the parse.</returns>
		public ParserStatus Parse(ContentList contents)
		{
			// Make sure we don't get any null arguments.
			if (contents == null)
			{
				throw new ArgumentNullException("contents");
			}

			// If we don't have any unparsed text, then we don't need to do
			// anything at all.
			if (contents.GetUnparsedCount() == 0)
			{
				return ParserStatus.Succeeded;
			}

			// Go through the contents and look for Unparsed content elements.
			// Everything else, we simply pass add into the new list. Once done,
			// we replace the contents of the container with the new one.
			var newContents = new ContentList();

			foreach (Content content in contents)
			{
				// If this unparsed, then process it directly.
				if (content.ContentType == ContentType.Unparsed)
				{
					var unparsed = (Unparsed) content;
					newContents.AddRange(ParseString(unparsed.Text));
					continue;
				}

				// We don't have unparsed, so we check to see if this is a
				// content container.
				if (content is IContentContainer)
				{
					// This is a container, so see if there is any unparsed
					// content inside it. If there isn't, we can just add it
					// without processing.
					var childContentContainer = (IContentContainer) content;

					if (childContentContainer.Contents.GetUnparsedCount() > 0)
					{
						// There is unparsed content inside this container. We don't
						// care about the return value since this class doesn't
						// fail parses.
						Parse(childContentContainer.Contents);
					}
				}

				// This content isn't a container and it doesn't have unparsed
				// data, so just add it.
				newContents.Add(content);
			}

			// Replace the contents of the container with the new contents.
			contents.Replace(newContents);

			// Return that we have successful parsed the contents.
			return ParserStatus.Succeeded;
		}

		/// <summary>
		/// Parses the string and populates the content list.
		/// </summary>
		/// <param name="content">The content.</param>
		/// <returns></returns>
		internal ContentList ParseString(string content)
		{
			// Create a list of content to place the results.
			var parsed = new ContentList();

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
						parsed.Add(new Puncuation(token, true));
						break;

					case ":":
					case ";":
					case ",":
						parsed.Add(new Puncuation(token, false));
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
#endif
