// <copyright file="MemoryPersistenceTestsBase.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System;
using System.Collections.Generic;

using NUnit.Framework;

namespace AuthorIntrusion.Tests.IO.MarkdownBufferFormatTests
{
	/// <summary>
	/// Contains common functionality for tests based on memory persistence.
	/// </summary>
	public class MemoryPersistenceTestsBase
	{
		#region Methods

		/// <summary>
		/// Verifies that the lines given are identical to the expected lines.
		/// </summary>
		/// <param name="lines">
		/// The lines.
		/// </param>
		/// <param name="expected">
		/// The expected.
		/// </param>
		protected void AssertLines(
			List<string> lines,
			params string[] expected)
		{
			// Identify when we need to display the results to the buffer.
			bool reportResults = lines.Count != expected.Length;

			if (reportResults)
			{
				Console.WriteLine(
					"Number of lines didn't match. Expected {0}, Actual {1}.",
					expected.Length,
					lines.Count);
				Console.WriteLine();
			}

			// If the lines match, then report differences.
			if (!reportResults)
			{
				for (var index = 0; index < expected.Length; index++)
				{
					// If the lines match, then we're good.
					if (expected[index] == lines[index])
					{
						continue;
					}

					Console.WriteLine(
						"{0} > '{1}'",
						index.ToString()
							.PadLeft(3),
						expected[index]);
					Console.WriteLine(
						"{0} < '{1}'",
						index.ToString()
							.PadLeft(3),
						lines[index]);
					Console.WriteLine();

					// This test will be failing.
					reportResults = true;
				}
			}

			// If we have to report, then do so.
			if (!reportResults)
			{
				return;
			}

			// Write out a short header.
			Console.WriteLine("Expected Output:");
			Console.WriteLine();

			// Write out the expected lines.
			for (var index = 0; index < expected.Length; index++)
			{
				Console.WriteLine(
					"{0} > {1}",
					index.ToString()
						.PadLeft(3),
					expected[index]);
			}

			// Write out the actual lines.
			Console.WriteLine();
			Console.WriteLine("Actual Output:");
			Console.WriteLine();

			// Write out the expected lines.
			for (var index = 0; index < lines.Count; index++)
			{
				Console.WriteLine(
					"{0} < {1}",
					index.ToString()
						.PadLeft(3),
					lines[index]);
			}

			// Fail the test.
			Assert.Fail("Expected lines did not match.");
		}

		#endregion
	}
}
