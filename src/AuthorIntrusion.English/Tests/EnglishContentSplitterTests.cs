#if DISABLED
#region Namespaces

using AuthorIntrusion.Contracts.Collections;
using AuthorIntrusion.Contracts.Contents;
using AuthorIntrusion.Contracts.Enumerations;

using NUnit.Framework;

#endregion

namespace AuthorIntrusion.English.Tests
{
	/// <summary>
	/// Contains the tests used to excercise the English content splitter.
	/// </summary>
	[TestFixture]
	public class EnglishContentSplitterTests
	{
		#region Tests

		/// <summary>
		/// Parses a simple quoted string.
		/// </summary>
		[Test]
		public void SimpleQuote()
		{
			// Set up the input list.
			var input = new ContentList();
			input.Add("I like ");
			input.Add(new Quote("cheese."));

			// Parse and assert the results.
			ContentList contents = SplitContents(input);

			Assert.AreEqual(3, contents.Count, "Unexpected number of contents");
			Assert.AreEqual(
				ContentType.Word, contents[0].ContentType, "contents[0] type is unexpected");
			Assert.AreEqual(
				ContentType.Word, contents[1].ContentType, "contents[1] type is unexpected");
			Assert.AreEqual(
				ContentType.Quote, contents[2].ContentType, "contents[2] type is unexpected");

			// Assert the contents of the quote.
			var quote = (Quote) contents[2];

			Assert.AreEqual(2, quote.Contents.Count, "Unexpeted content count in quote");
			Assert.AreEqual(
				ContentType.Word,
				quote.Contents[0].ContentType,
				"quote.contents[0] type is unexpected");
			Assert.AreEqual(
				ContentType.Terminator,
				quote.Contents[1].ContentType,
				"quote.contents[1] type is unexpected");
		}

		/// <summary>
		/// Parses a three word sentence into contents.
		/// </summary>
		[Test]
		public void ThreeWordSentence()
		{
			ContentList contents = SplitContents("I like cheese.");

			Assert.AreEqual(4, contents.Count, "Unexpected number of contents");
			Assert.AreEqual(
				ContentType.Word, contents[0].ContentType, "contents[0] type is unexpected");
			Assert.AreEqual(
				ContentType.Word, contents[1].ContentType, "contents[1] type is unexpected");
			Assert.AreEqual(
				ContentType.Word, contents[2].ContentType, "contents[2] type is unexpected");
			Assert.AreEqual(
				ContentType.Terminator,
				contents[3].ContentType,
				"contents[3] type is unexpected");
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
			var contents = new ContentList();
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
			var splitter = new EnglishUnparsedSplitter();
			splitter.Parse(contents);
			return contents;
		}

		#endregion
	}
}
#endif
