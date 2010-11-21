#region Namespaces

using MfGames.Author.Contract.Contents;
using MfGames.Author.Contract.Contents.Collections;

using NUnit.Framework;

#endregion

namespace MfGames.Author.English.Tests
{
	/// <summary>
	/// Contains the tests used to excercise the English content splitter.
	/// </summary>
	[TestFixture]
	public class EnglishContentSplitterTests
	{
		#region Tests

		/// <summary>
		/// Parses a three word sentence into contents.
		/// </summary>
		[Test]
		public void ThreeWordSentence()
		{
			ContentList contents = SplitContents("I like cheese.");

			Assert.AreEqual(4, contents.Count, "Unexpected number of contents");
			Assert.AreEqual(
				typeof(Word),
				contents[0].GetType(),
				"contents[0] type is unexpected");
			Assert.AreEqual(
				typeof(Word),
				contents[1].GetType(),
				"contents[1] type is unexpected");
			Assert.AreEqual(
				typeof(Word),
				contents[2].GetType(),
				"contents[2] type is unexpected");
			Assert.AreEqual(
				typeof(Terminator),
				contents[3].GetType(),
				"contents[3] type is unexpected");
		}

		/// <summary>
		/// Parses a simple quoted string.
		/// </summary>
		[Test]
		public void SimpleQuote()
		{
			// Set up the input list.
			ContentList input = new ContentList();
			input.Add("I like ");
			input.Add(new Quote("cheese."));

			// Parse and assert the results.
			ContentList contents = SplitContents(input);

			Assert.AreEqual(3, contents.Count, "Unexpected number of contents");
			Assert.AreEqual(
				typeof(Word),
				contents[0].GetType(),
				"contents[0] type is unexpected");
			Assert.AreEqual(
				typeof(Word),
				contents[1].GetType(),
				"contents[1] type is unexpected");
			Assert.AreEqual(
				typeof(Quote),
				contents[2].GetType(),
				"contents[2] type is unexpected");

			// Assert the contents of the quote.
			Quote quote = (Quote) contents[2];

			Assert.AreEqual(
				2,
				quote.Contents.Count, 
				"Unexpeted content count in quote");
			Assert.AreEqual(
				typeof(Word),
				quote.Contents[0].GetType(),
				"quote.contents[0] type is unexpected");
			Assert.AreEqual(
				typeof(Terminator),
				quote.Contents[1].GetType(),
				"quote.contents[1] type is unexpected");
		}

		#endregion

		#region Utility

		/// <summary>
		/// Splits the input into a list of contents.
		/// </summary>
		/// <param name="input">The input.</param>
		/// <returns></returns>
		private static ContentList SplitContents(string input)
		{
			// Create a content list with the unparsed string.
			ContentList contents = new ContentList();
			contents.Add(input);
			return SplitContents(contents);
		}

		/// <summary>
		/// Splits the input into a list of contents.
		/// </summary>
		/// <param name="contents">The contents.</param>
		/// <returns></returns>
		private static ContentList SplitContents(ContentList contents)
		{
			// Split the contents.
			EnglishContentSplitter splitter = new EnglishContentSplitter();
			ContentList parsed = splitter.SplitContents(contents);
			return parsed;
		}

		#endregion
	}
}