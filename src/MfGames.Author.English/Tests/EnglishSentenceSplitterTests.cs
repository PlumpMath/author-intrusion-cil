#region Namespaces

using System.Collections.Generic;

using MfGames.Author.Contract.Contents;
using MfGames.Author.Contract.Contents.Collections;

using NUnit.Framework;

#endregion

namespace MfGames.Author.English.Tests
{
	/// <summary>
	/// Contains various sentence splitter tests.
	/// </summary>
	[TestFixture]
	public class EnglishSentenceSplitterTests
	{
		#region Tests

		/// <summary>
		/// Tests a simple sentence.
		/// </summary>
		[Test]
		public void SimpleSentence()
		{
			ContentList sentence = TestSingleSentence(
				new UnparsedString("This is a simple sentence."));

			Assert.AreEqual(
				6,
				sentence.Count, 
				"Unexpected number of content elements in results");
		}

		/// <summary>
		/// Tests a sentence with an honorific.
		/// </summary>
		[Test]
		public void SentenceWithHonorific()
		{
			TestSingleSentence(new UnparsedString("I saw Mr. Smith."));
		}

		/// <summary>
		/// Tests a sentence with a quote.
		/// </summary>
		[Test]
		public void SentenceWithQuote()
		{
			TestSingleSentence(
				new UnparsedString("I said, "),
				new Quote("You like me."));
		}

		/// <summary>
		/// Tests a sentence with a quote that contains two sentences.
		/// </summary>
		[Test]
		public void SentenceWithMultipleSentenceQuote()
		{
			TestSingleSentence(
				new UnparsedString("I said, "),
				new Quote("You like me. And then she hit me."));
		}

		/// <summary>
		/// Tests a sentence with a split quote.
		/// </summary>
		[Test]
		public void SentenceWithSplitQuote()
		{
			TestSingleSentence(
				new UnparsedString("I said, "),
				new Quote("You like me,"),
				new UnparsedString(" while flinching."));
		}

		#endregion

		#region Utility

		/// <summary>
		/// Takes the list of content elements and splits the resulting sentences. Then
		/// the resulting sentence is verify that it is parsed correctly.
		/// </summary>
		/// <param name="contents">The contents.</param>
		private static ContentList TestSingleSentence(params ContentBase[] contents)
		{
			// Create the paragraph and add the sentence to the unparsed content.
			var unparsed = new ContentList();
			unparsed.Add(
				new UnparsedString("This is the first sentence."));

			foreach (ContentBase content in contents)
			{
				unparsed.Add(content);
			}

			unparsed.Add(
				new UnparsedString("This is the third sentence."));

			// Split the input into parsed content.
			EnglishContentSplitter contentSplitter = new EnglishContentSplitter();
			ContentList parsed = contentSplitter.SplitContents(unparsed);

			// Split the sentences out and compare the results.
			List<ContentList> sentences = EnglishSentenceSplitter.SplitSentences(parsed);

			Assert.AreEqual(
				3,
				sentences.Count,
				"Could not parse isolate the sentence with the splitter.");

			// Return the parsed sentence if we got this far. We don't bother with
			// the first and third since those are just used to detect run-ons.
			return sentences[1];
		}

		#endregion
	}
}