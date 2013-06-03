// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using NUnit.Framework;

namespace AuthorIntrusion.Plugins.Counter.Tests
{
	[TestFixture]
	public class WordCounterHelperTests
	{
		#region Methods

		[Test]
		public void CountBlankString()
		{
			// Arrange
			int wordCount;
			int characterCount;
			int nonWhitespaceCount;

			// Act
			WordCounterHelper.CountWords(
				"", out wordCount, out characterCount, out nonWhitespaceCount);

			// Assert
			Assert.AreEqual(0, wordCount);
			Assert.AreEqual(0, characterCount);
			Assert.AreEqual(0, nonWhitespaceCount);
		}

		[Test]
		public void CountSingleWord()
		{
			// Arrange
			int wordCount;
			int characterCount;
			int nonWhitespaceCount;

			// Act
			WordCounterHelper.CountWords(
				"cheese", out wordCount, out characterCount, out nonWhitespaceCount);

			// Assert
			Assert.AreEqual(1, wordCount);
			Assert.AreEqual(6, characterCount);
			Assert.AreEqual(6, nonWhitespaceCount);
		}

		[Test]
		public void CountSingleWordWithLeadingSpace()
		{
			// Arrange
			int wordCount;
			int characterCount;
			int nonWhitespaceCount;

			// Act
			WordCounterHelper.CountWords(
				"  cheese", out wordCount, out characterCount, out nonWhitespaceCount);

			// Assert
			Assert.AreEqual(1, wordCount);
			Assert.AreEqual(8, characterCount);
			Assert.AreEqual(6, nonWhitespaceCount);
		}

		[Test]
		public void CountSingleWordWithTrailingSpace()
		{
			// Arrange
			int wordCount;
			int characterCount;
			int nonWhitespaceCount;

			// Act
			WordCounterHelper.CountWords(
				"cheese  ", out wordCount, out characterCount, out nonWhitespaceCount);

			// Assert
			Assert.AreEqual(1, wordCount);
			Assert.AreEqual(8, characterCount);
			Assert.AreEqual(6, nonWhitespaceCount);
		}

		[Test]
		public void CountSplicedWords()
		{
			// Arrange
			int wordCount;
			int characterCount;
			int nonWhitespaceCount;

			// Act
			WordCounterHelper.CountWords(
				"cheese-curds", out wordCount, out characterCount, out nonWhitespaceCount);

			// Assert
			Assert.AreEqual(1, wordCount);
			Assert.AreEqual(12, characterCount);
			Assert.AreEqual(12, nonWhitespaceCount);
		}

		[Test]
		public void CountTwoWords()
		{
			// Arrange
			int wordCount;
			int characterCount;
			int nonWhitespaceCount;

			// Act
			WordCounterHelper.CountWords(
				"cheese curds", out wordCount, out characterCount, out nonWhitespaceCount);

			// Assert
			Assert.AreEqual(2, wordCount);
			Assert.AreEqual(12, characterCount);
			Assert.AreEqual(11, nonWhitespaceCount);
		}

		#endregion
	}
}
