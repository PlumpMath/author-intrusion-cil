#region Copyright and License

// Copyright (c) 2005-2011, Moonfire Games
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

#endregion


#if DISABLED
#region Namespaces

using AuthorIntrusion.Contracts.Collections;
using AuthorIntrusion.Contracts.Contents;
using AuthorIntrusion.Contracts.Enumerations;
using AuthorIntrusion.Contracts.Languages;

#endregion

namespace AuthorIntrusion.English
{
	/// <summary>
	/// Implements a sentence splitter that takes the parsed contents and
	/// creates sentence objects from it.
	/// </summary>
	public class EnglishSentenceSplitter : EnglishSpecificBase, IContentParser
	{
		#region Parsing

		/// <summary>
		/// Parses the content of the content container and replaces the contents
		/// with parsed data.
		/// </summary>
		/// <param name="contents">The content container.</param>
		/// <returns>The status result from the parse.</returns>
		public ParserStatus Parse(ContentList contents)
		{
			// If we have unparsed content, then we can't operation on this
			// container at this time.
			if (contents.GetUnparsedCount() > 0)
			{
				return ParserStatus.Deferred;
			}

			// Create a list of individual sentences which we'll be building
			// or passing on if they were already created.
			var sentences = new ContentList();
			Sentence current = null;

			// Go through the content to build up the sentences.
			foreach (Content content in contents)
			{
				// Check to see if this is already a sentence.
				if (content.ContentType == ContentType.Sentence)
				{
					// Already parsed, so add it to the list.
					sentences.Add(content);
					continue;
				}

				// If we don't have a current sentence, create it and add it to
				// the list.
				if (current == null)
				{
					current = new Sentence();
					sentences.Add(current);
				}

				// Add the content to the current sentence.
				current.Contents.Add(content);

				// Check for a sentence terminator either in this content or
				// at the end of a quote since English allows for the period
				// to be inside the quote.
				bool hasTerminator = false;

				if (content.ContentType == ContentType.Terminator)
				{
					hasTerminator = true;
				}

				// If the sentence has a quote, see if we have a terminator
				// at the end of that quote.
				if (content.ContentType == ContentType.Quote)
				{
					var quote = (Quote) content;
					hasTerminator = quote.Contents.GetEndsWithTerminator();
				}

				// We are at the end of the sentence. Start a new one.
				if (hasTerminator)
				{
					// We are done with the sentence, so just clear out the
					// current sentence.
					current = null;
				}
			}

			// Replace the contents with the new parsed contents.
			contents.Replace(sentences);

			// Return that we parsed it successfully.
			return ParserStatus.Succeeded;
		}

		#endregion
	}
}
#endif