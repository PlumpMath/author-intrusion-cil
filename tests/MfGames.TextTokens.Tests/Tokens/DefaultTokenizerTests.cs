// <copyright file="DefaultTokenizerTests.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System.Collections.Generic;
using System.Linq;

using MfGames.TextTokens.Tokens;

using Xunit;

namespace MfGames.TextTokens.Tests.Tokens
{
	/// <summary>
	/// Tests the functionality of the default token parser to ensure it produces the
	/// correct tokens.
	/// </summary>
	public class DefaultTokenizerTests
	{
		#region Public Methods and Operators

		/// <summary>
		/// Verifies how the splitter handles a blank (empty) string.
		/// </summary>
		[Fact]
		public void HandleBlankString()
		{
			// Arrange
			string input = string.Empty;
			var tokenizer = new DefaultTokenSplitter();

			// Act
			List<string> results = tokenizer.Tokenize(input)
				.ToList();

			// Assert
			Assert.NotNull(
				results);
			Assert.Equal(
				0,
				results.Count);
		}

		/// <summary>
		/// Verifies how the splitter works with a contraction.
		/// </summary>
		[Fact]
		public void HandleContraction()
		{
			// Arrange
			var input = "didn't";
			var tokenizer = new DefaultTokenSplitter();

			// Act
			List<string> results = tokenizer.Tokenize(input)
				.ToList();

			// Assert
			Assert.NotNull(
				results);
			Assert.Equal(
				3,
				results.Count);
			Assert.Equal(
				"didn",
				results[0]);
			Assert.Equal(
				"'",
				results[1]);
			Assert.Equal(
				"t",
				results[2]);
		}

		/// <summary>
		/// Verifies how the token handles a leading underscore.
		/// </summary>
		[Fact]
		public void HandleLeadingUnderscoreTwoWordsString()
		{
			// Arrange
			var input = "_e two";
			var tokenizer = new DefaultTokenSplitter();

			// Act
			List<string> results = tokenizer.Tokenize(input)
				.ToList();

			// Assert
			Assert.NotNull(
				results);
			Assert.Equal(
				4,
				results.Count);

			Assert.Equal(
				"_",
				results[0]);
			Assert.Equal(
				"e",
				results[1]);
			Assert.Equal(
				" ",
				results[2]);
			Assert.Equal(
				"two",
				results[3]);
		}

		/// <summary>
		/// Verifies how the splitter handles a null.
		/// </summary>
		[Fact]
		public void HandleNullString()
		{
			// Arrange
			string input = null;
			var tokenizer = new DefaultTokenSplitter();

			// Act
			List<string> results = tokenizer.Tokenize(input)
				.ToList();

			// Assert
			Assert.NotNull(
				results);
			Assert.Equal(
				0,
				results.Count);
		}

		/// <summary>
		/// Verifies how the splitter handles a single word string.
		/// </summary>
		[Fact]
		public void HandleSingleWordString()
		{
			// Arrange
			var input = "one";
			var tokenizer = new DefaultTokenSplitter();

			// Act
			List<string> results = tokenizer.Tokenize(input)
				.ToList();

			// Assert
			Assert.NotNull(
				results);
			Assert.Equal(
				1,
				results.Count);
		}

		/// <summary>
		/// Verifies how the splitter handles a single space.
		/// </summary>
		[Fact]
		public void HandleSpaceString()
		{
			// Arrange
			var input = " ";
			var tokenizer = new DefaultTokenSplitter();

			// Act
			List<string> results = tokenizer.Tokenize(input)
				.ToList();

			// Assert
			Assert.NotNull(
				results);
			Assert.Equal(
				1,
				results.Count);
		}

		/// <summary>
		/// Verifies how the splitter handles three, space-separated words.
		/// </summary>
		[Fact]
		public void HandleThreeWordsString()
		{
			// Arrange
			var input = "one two three";
			var tokenizer = new DefaultTokenSplitter();

			// Act
			List<string> results = tokenizer.Tokenize(input)
				.ToList();

			// Assert
			Assert.NotNull(
				results);
			Assert.Equal(
				5,
				results.Count);

			Assert.Equal(
				"one",
				results[0]);
			Assert.Equal(
				" ",
				results[1]);
			Assert.Equal(
				"two",
				results[2]);
			Assert.Equal(
				" ",
				results[3]);
			Assert.Equal(
				"three",
				results[4]);
		}

		/// <summary>
		/// Verifies how the splitter handles two sequential spaces.
		/// </summary>
		[Fact]
		public void HandleTwoSpacesString()
		{
			// Arrange
			var input = "  ";
			var tokenizer = new DefaultTokenSplitter();

			// Act
			List<string> results = tokenizer.Tokenize(input)
				.ToList();

			// Assert
			Assert.NotNull(
				results);
			Assert.Equal(
				1,
				results.Count);
		}

		#endregion
	}
}
