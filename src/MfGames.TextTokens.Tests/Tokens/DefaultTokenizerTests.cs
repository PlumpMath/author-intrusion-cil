// <copyright file="DefaultTokenizerTests.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System.Collections.Generic;
using System.Linq;

using MfGames.TextTokens.Tokens;

using NUnit.Framework;

namespace MfGames.TextTokens.Tests.Tokens
{
	/// <summary>
	/// Tests the functionality of the default token parser to ensure it produces the
	/// correct tokens.
	/// </summary>
	[TestFixture]
	public class DefaultTokenizerTests
	{
		#region Public Methods and Operators

		/// <summary>
		/// Verifies how the splitter handles a blank (empty) string.
		/// </summary>
		[Test]
		public void HandleBlankString()
		{
			// Arrange
			string input = string.Empty;
			var tokenizer = new DefaultTokenSplitter();

			// Act
			List<string> results = tokenizer.Tokenize(input)
				.ToList();

			// Assert
			Assert.IsNotNull(
				results,
				"The results were null");
			Assert.AreEqual(
				0,
				results.Count,
				"The number of tokens is unexpected.");
		}

		/// <summary>
		/// Verifies how the splitter works with a contraction.
		/// </summary>
		[Test]
		public void HandleContraction()
		{
			// Arrange
			var input = "didn't";
			var tokenizer = new DefaultTokenSplitter();

			// Act
			List<string> results = tokenizer.Tokenize(input)
				.ToList();

			// Assert
			Assert.IsNotNull(
				results,
				"The results were null");
			Assert.AreEqual(
				3,
				results.Count,
				"The number of tokens is unexpected.");
			Assert.AreEqual(
				"didn",
				results[0],
				"1st result is unexpected.");
			Assert.AreEqual(
				"'",
				results[1],
				"2nd result is unexpected.");
			Assert.AreEqual(
				"t",
				results[2],
				"3rd result is unexpected.");
		}

		/// <summary>
		/// Verifies how the token handles a leading underscore.
		/// </summary>
		[Test]
		public void HandleLeadingUnderscoreTwoWordsString()
		{
			// Arrange
			var input = "_e two";
			var tokenizer = new DefaultTokenSplitter();

			// Act
			List<string> results = tokenizer.Tokenize(input)
				.ToList();

			// Assert
			Assert.IsNotNull(
				results,
				"The results were null");
			Assert.AreEqual(
				4,
				results.Count,
				"The number of tokens is unexpected.");

			Assert.AreEqual(
				"_",
				results[0],
				"1st token is unexpected.");
			Assert.AreEqual(
				"e",
				results[1],
				"2nd token is unexpected.");
			Assert.AreEqual(
				" ",
				results[2],
				"3rd token is unexpected.");
			Assert.AreEqual(
				"two",
				results[3],
				"4th token is unexpected.");
		}

		/// <summary>
		/// Verifies how the splitter handles a null.
		/// </summary>
		[Test]
		public void HandleNullString()
		{
			// Arrange
			string input = null;
			var tokenizer = new DefaultTokenSplitter();

			// Act
			List<string> results = tokenizer.Tokenize(input)
				.ToList();

			// Assert
			Assert.IsNotNull(
				results,
				"The results were null");
			Assert.AreEqual(
				0,
				results.Count,
				"The number of tokens is unexpected.");
		}

		/// <summary>
		/// Verifies how the splitter handles a single word string.
		/// </summary>
		[Test]
		public void HandleSingleWordString()
		{
			// Arrange
			var input = "one";
			var tokenizer = new DefaultTokenSplitter();

			// Act
			List<string> results = tokenizer.Tokenize(input)
				.ToList();

			// Assert
			Assert.IsNotNull(
				results,
				"The results were null");
			Assert.AreEqual(
				1,
				results.Count,
				"The number of tokens is unexpected.");
		}

		/// <summary>
		/// Verifies how the splitter handles a single space.
		/// </summary>
		[Test]
		public void HandleSpaceString()
		{
			// Arrange
			var input = " ";
			var tokenizer = new DefaultTokenSplitter();

			// Act
			List<string> results = tokenizer.Tokenize(input)
				.ToList();

			// Assert
			Assert.IsNotNull(
				results,
				"The results were null");
			Assert.AreEqual(
				1,
				results.Count,
				"The number of tokens is unexpected.");
		}

		/// <summary>
		/// Verifies how the splitter handles three, space-separated words.
		/// </summary>
		[Test]
		public void HandleThreeWordsString()
		{
			// Arrange
			var input = "one two three";
			var tokenizer = new DefaultTokenSplitter();

			// Act
			List<string> results = tokenizer.Tokenize(input)
				.ToList();

			// Assert
			Assert.IsNotNull(
				results,
				"The results were null");
			Assert.AreEqual(
				5,
				results.Count,
				"The number of tokens is unexpected.");

			Assert.AreEqual(
				"one",
				results[0],
				"1st token is unexpected.");
			Assert.AreEqual(
				" ",
				results[1],
				"2nd token is unexpected.");
			Assert.AreEqual(
				"two",
				results[2],
				"3rd token is unexpected.");
			Assert.AreEqual(
				" ",
				results[3],
				"4th token is unexpected.");
			Assert.AreEqual(
				"three",
				results[4],
				"5th token is unexpected.");
		}

		/// <summary>
		/// Verifies how the splitter handles two sequential spaces.
		/// </summary>
		[Test]
		public void HandleTwoSpacesString()
		{
			// Arrange
			var input = "  ";
			var tokenizer = new DefaultTokenSplitter();

			// Act
			List<string> results = tokenizer.Tokenize(input)
				.ToList();

			// Assert
			Assert.IsNotNull(
				results,
				"The results were null");
			Assert.AreEqual(
				1,
				results.Count,
				"The number of tokens is unexpected.");
		}

		#endregion
	}
}
