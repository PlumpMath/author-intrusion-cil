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

using NUnit.Framework;

#endregion

namespace AuthorIntrusion.English.Tests
{
	/// <summary>
	/// Contains various sentence splitter tests.
	/// </summary>
	[TestFixture]
	public class EnglishSentenceSplitterTests
	{
		#region Tests

		/// <summary>
		/// Tests a sentence with an honorific.
		/// </summary>
		[Test]
		public void SentenceWithHonorific()
		{
			TestSingleSentence(new Unparsed("I saw Mr. Smith."));
		}

		/// <summary>
		/// Tests a sentence with a quote that contains two sentences.
		/// </summary>
		[Test]
		public void SentenceWithMultipleSentenceQuote()
		{
			TestSingleSentence(
				new Unparsed("I said, "), new Quote("You like me. And then she hit me."));
		}

		/// <summary>
		/// Tests a sentence with a quote.
		/// </summary>
		[Test]
		public void SentenceWithQuote()
		{
			TestSingleSentence(new Unparsed("I said, "), new Quote("You like me."));
		}

		/// <summary>
		/// Tests a sentence with a split quote.
		/// </summary>
		[Test]
		public void SentenceWithSplitQuote()
		{
			TestSingleSentence(
				new Unparsed("I said, "),
				new Quote("You like me,"),
				new Unparsed(" while flinching."));
		}

		/// <summary>
		/// Tests a simple sentence.
		/// </summary>
		[Test]
		public void SimpleSentence()
		{
			var sentence =
				TestSingleSentence(new Unparsed("This is a simple sentence.")) as Sentence;

			Assert.IsNotNull(sentence, "Content type was not expected");
			Assert.AreEqual(
				6,
				sentence.Contents.Count,
				"Unexpected number of content elements in results");
		}

		#endregion

		#region Utility

		/// <summary>
		/// Takes the list of content elements and splits the resulting sentences. Then
		/// the resulting sentence is verify that it is parsed correctly.
		/// </summary>
		/// <param name="contents">The contents.</param>
		private static Content TestSingleSentence(params Content[] contents)
		{
			// Create the paragraph and add the sentence to the unparsed content.
			var parsed = new ContentList();
			parsed.Add(new Unparsed("This is the first sentence."));

			foreach (Content content in contents)
			{
				parsed.Add(content);
			}

			parsed.Add(new Unparsed("This is the third sentence."));

			// Split the input into parsed content.
			var contentSplitter = new EnglishUnparsedSplitter();
			contentSplitter.Parse(parsed);

			// Split the sentences out and compare the results.
			var sentenceSplitter = new EnglishSentenceSplitter();
			sentenceSplitter.Parse(parsed);

			Assert.AreEqual(
				3, parsed.Count, "Could not parse isolate the sentence with the splitter.");

			// Return the parsed sentence if we got this far. We don't bother with
			// the first and third since those are just used to detect run-ons.
			return parsed[1];
		}

		#endregion
	}
}
#endif