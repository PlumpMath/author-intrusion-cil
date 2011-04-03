#region Copyright and License

// Copyright (c) 2011, Moonfire Games
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

#region Namespaces

using System.Collections.Generic;
using System.IO;
using System.Text;

#endregion

namespace TestGenerator
{
	/// <summary>
	/// Generates tests for the small DocumentLineBuffer tests. The small document
	/// has the following elements: article, 3 paragraphs, section, 3 paragraphs,
	/// section, and 3 more paragraphs.
	/// </summary>
	public class SmallDocumentLineBufferGenerator
	{
		#region Test Generation

		private TextWriter writer;

		/// <summary>
		/// Generates the tests for this instance.
		/// </summary>
		public void Generate()
		{
			// Create the new output file for these tests.
			using (
				Stream stream = File.Open(
					"SmallDocumentLineBufferGenerator.cs", FileMode.Create, FileAccess.Write))
			{
				using (writer = new StreamWriter(stream))
				{
					GenerateDeletes();
				}
			}
		}

		/// <summary>
		/// Cycles through deleting every element in the small document from
		/// the first to the last and generates the resulting data from that
		/// output. This works with linear data instead of the nested data that
		/// the actual Document represents, so it is easier to generate the
		/// output from here, then apply it to the nested structures.
		/// </summary>
		private void GenerateDeletes()
		{
			// Figure out the delete range for every element, starting at the
			// first and going to the end.
			for (int start = 0; start < 12; start++)
			{
				for (int end = start; end < 12; end++)
				{
					// Figure out how many items we'll be deleting.
					int count = end - start + 1;

					// We don't do the entire buffer.
					if (start == 0 && end == 11)
					{
						continue;
					}

					// Create a pristine line of elements, then delete a range
					// of them.
					List<string> results = CreateSmallDocument();
					results.RemoveRange(start, count);

					// Create a unit test from these results.
					writer.WriteLine("[Test]");
					writer.WriteLine(
						"public void DeleteLineRangeFrom{0}_{1}()",
						start.ToString().PadLeft(2, '0'),
						end.ToString().PadLeft(2, '0'));
					writer.WriteLine("{");

					// Operation
					writer.WriteLine("\t// Operation");
					writer.WriteLine("\tbuffer.DeleteLines({0}, {1});", start, count);
					writer.WriteLine();

					// Verification
					writer.WriteLine("\t// Verification");
					writer.WriteLine(
						"\tAssert.AreEqual(\"{0}\", document.GetThumbprint());",
						GetThumbprint(results));
					writer.WriteLine();

					for (int i = 0; i < results.Count; i++)
					{
						writer.WriteLine(
							"\tAssert.AreEqual(\"{0}\", document.Matters[{1}].GetContents());",
							results[i],
							i);
					}

					// Finish up the test.
					writer.WriteLine("}");
				}
			}
		}

		#endregion

		#region Mock Document

		/// <summary>
		/// Creates a list that represents the structure of the small
		/// document.
		/// </summary>
		/// <returns></returns>
		private static List<string> CreateSmallDocument()
		{
			var list = new List<string>();

			list.Add("Article");
			list.Add("P01");
			list.Add("P02");
			list.Add("P03");
			list.Add("Section 1");
			list.Add("P04");
			list.Add("P05");
			list.Add("P06");
			list.Add("Section 2");
			list.Add("P07");
			list.Add("P08");
			list.Add("P09");

			return list;
		}

		/// <summary>
		/// Gets the thumbprint from the mock document.
		/// </summary>
		/// <param name="document">The document.</param>
		/// <returns></returns>
		private static string GetThumbprint(IEnumerable<string> document)
		{
			// Build up a string buffer of the thumbprint.
			var thumbprint = new StringBuilder();

			// Go through the mock document.
			bool first = true;

			foreach (string name in document)
			{
				// The first is always an article.
				if (first)
				{
					thumbprint.Append("A");
					first = false;
					continue;
				}

				// Modify the thumbprint to match the one Document produces.
				char prefix = name[0];

				switch (prefix)
				{
					case 'P':
						prefix = 'p';
						break;
					case 'S':
						prefix = '1';
						break;
				}

				thumbprint.Append(prefix);
			}

			// Return the resulting thumbprint.
			return thumbprint.ToString();
		}

		#endregion
	}
}