// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Blocks;
using C5;
using NUnit.Framework;

namespace AuthorIntrusion.Plugins.Spelling.Tests
{
	[TestFixture]
	public class SpellingWordSplitterTests
	{
		#region Methods

		[Test]
		public void SplitBlankString()
		{
			// Arrange
			var splitter = new SpellingWordSplitter();

			// Act
			IList<TextSpan> words = splitter.SplitAndNormalize("");

			// Assert
			Assert.AreEqual(0, words.Count);
		}

		[Test]
		public void SplitOneContractedWord()
		{
			// Arrange
			var splitter = new SpellingWordSplitter();
			const string input = "don't";

			// Act
			IList<TextSpan> words = splitter.SplitAndNormalize(input);

			// Assert
			Assert.AreEqual(1, words.Count);
			Assert.AreEqual(input, words[0].GetText(input));
		}

		[Test]
		public void SplitOneWord()
		{
			// Arrange
			var splitter = new SpellingWordSplitter();
			const string input = "one";

			// Act
			IList<TextSpan> words = splitter.SplitAndNormalize(input);

			// Assert
			Assert.AreEqual(1, words.Count);
			Assert.AreEqual(input, words[0].GetText(input));
		}

		[Test]
		public void SplitTwoWords()
		{
			// Arrange
			var splitter = new SpellingWordSplitter();
			const string input = "one two";

			// Act
			IList<TextSpan> words = splitter.SplitAndNormalize(input);

			// Assert
			Assert.AreEqual(2, words.Count);
			Assert.AreEqual("one", words[0].GetText(input));
			Assert.AreEqual("two", words[1].GetText(input));
		}

		#endregion
	}
}
