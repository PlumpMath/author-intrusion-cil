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

using AuthorIntrusion.Contracts;
using AuthorIntrusion.Contracts.Matters;
using AuthorIntrusion.Contracts.Structures;

using AuthorIntrusionGtk.Editors;

using NUnit.Framework;

#endregion

namespace UnitTests
{
	/// <summary>
	/// Performs various tests on a small sample document.
	/// </summary>
	[TestFixture]
	public class SmallDocumentLineBufferTests
	{
		#region Setup

		private Document document;
		private DocumentLineBuffer buffer;

		/// <summary>
		/// Sets up the test and creates the initial line buffer.
		/// </summary>
		[SetUp]
		public void Setup()
		{
			// Set up the standard small document.
			var article = new Region(MatterType.Article, "Article");
			article.Structures.Add(new Paragraph("P01"));
			article.Structures.Add(new Paragraph("P02"));
			article.Structures.Add(new Paragraph("P03"));

			var sect1 = new Region(MatterType.Section, "Section 1");
			sect1.Structures.Add(new Paragraph("P04"));
			sect1.Structures.Add(new Paragraph("P05"));
			sect1.Structures.Add(new Paragraph("P06"));
			article.Structures.Add(sect1);

			sect1 = new Region(MatterType.Section, "Section 2");
			sect1.Structures.Add(new Paragraph("P07"));
			sect1.Structures.Add(new Paragraph("P08"));
			sect1.Structures.Add(new Paragraph("P09"));
			article.Structures.Add(sect1);

			document = new Document();
			document.Structure = article;

			// Create the buffer from the document.
			buffer = new DocumentLineBuffer(document);
		}

		#endregion

		#region Automatically generated code

		[Test]
		public void DeleteLineRangeFrom00_00()
		{
			// Operation
			buffer.DeleteLines(0, 1);

			// Verification
			Assert.AreEqual("App1ppp1ppp", document.GetThumbprint());

			Assert.AreEqual("P01", buffer.GetStructureText(0));
			Assert.AreEqual("P02", buffer.GetStructureText(1));
			Assert.AreEqual("P03", buffer.GetStructureText(2));
			Assert.AreEqual("Section 1", buffer.GetStructureText(3));
			Assert.AreEqual("P04", buffer.GetStructureText(4));
			Assert.AreEqual("P05", buffer.GetStructureText(5));
			Assert.AreEqual("P06", buffer.GetStructureText(6));
			Assert.AreEqual("Section 2", buffer.GetStructureText(7));
			Assert.AreEqual("P07", buffer.GetStructureText(8));
			Assert.AreEqual("P08", buffer.GetStructureText(9));
			Assert.AreEqual("P09", buffer.GetStructureText(10));
		}
		[Test]
		public void DeleteLineRangeFrom00_01()
		{
			// Operation
			buffer.DeleteLines(0, 2);

			// Verification
			Assert.AreEqual("Ap1ppp1ppp", document.GetThumbprint());

			Assert.AreEqual("P02", buffer.GetStructureText(0));
			Assert.AreEqual("P03", buffer.GetStructureText(1));
			Assert.AreEqual("Section 1", buffer.GetStructureText(2));
			Assert.AreEqual("P04", buffer.GetStructureText(3));
			Assert.AreEqual("P05", buffer.GetStructureText(4));
			Assert.AreEqual("P06", buffer.GetStructureText(5));
			Assert.AreEqual("Section 2", buffer.GetStructureText(6));
			Assert.AreEqual("P07", buffer.GetStructureText(7));
			Assert.AreEqual("P08", buffer.GetStructureText(8));
			Assert.AreEqual("P09", buffer.GetStructureText(9));
		}
		[Test]
		public void DeleteLineRangeFrom00_02()
		{
			// Operation
			buffer.DeleteLines(0, 3);

			// Verification
			Assert.AreEqual("A1ppp1ppp", document.GetThumbprint());

			Assert.AreEqual("P03", buffer.GetStructureText(0));
			Assert.AreEqual("Section 1", buffer.GetStructureText(1));
			Assert.AreEqual("P04", buffer.GetStructureText(2));
			Assert.AreEqual("P05", buffer.GetStructureText(3));
			Assert.AreEqual("P06", buffer.GetStructureText(4));
			Assert.AreEqual("Section 2", buffer.GetStructureText(5));
			Assert.AreEqual("P07", buffer.GetStructureText(6));
			Assert.AreEqual("P08", buffer.GetStructureText(7));
			Assert.AreEqual("P09", buffer.GetStructureText(8));
		}
		[Test]
		public void DeleteLineRangeFrom00_03()
		{
			// Operation
			buffer.DeleteLines(0, 4);

			// Verification
			Assert.AreEqual("Appp1ppp", document.GetThumbprint());

			Assert.AreEqual("Section 1", buffer.GetStructureText(0));
			Assert.AreEqual("P04", buffer.GetStructureText(1));
			Assert.AreEqual("P05", buffer.GetStructureText(2));
			Assert.AreEqual("P06", buffer.GetStructureText(3));
			Assert.AreEqual("Section 2", buffer.GetStructureText(4));
			Assert.AreEqual("P07", buffer.GetStructureText(5));
			Assert.AreEqual("P08", buffer.GetStructureText(6));
			Assert.AreEqual("P09", buffer.GetStructureText(7));
		}
		[Test]
		public void DeleteLineRangeFrom00_04()
		{
			// Operation
			buffer.DeleteLines(0, 5);

			// Verification
			Assert.AreEqual("App1ppp", document.GetThumbprint());

			Assert.AreEqual("P04", buffer.GetStructureText(0));
			Assert.AreEqual("P05", buffer.GetStructureText(1));
			Assert.AreEqual("P06", buffer.GetStructureText(2));
			Assert.AreEqual("Section 2", buffer.GetStructureText(3));
			Assert.AreEqual("P07", buffer.GetStructureText(4));
			Assert.AreEqual("P08", buffer.GetStructureText(5));
			Assert.AreEqual("P09", buffer.GetStructureText(6));
		}
		[Test]
		public void DeleteLineRangeFrom00_05()
		{
			// Operation
			buffer.DeleteLines(0, 6);

			// Verification
			Assert.AreEqual("Ap1ppp", document.GetThumbprint());

			Assert.AreEqual("P05", buffer.GetStructureText(0));
			Assert.AreEqual("P06", buffer.GetStructureText(1));
			Assert.AreEqual("Section 2", buffer.GetStructureText(2));
			Assert.AreEqual("P07", buffer.GetStructureText(3));
			Assert.AreEqual("P08", buffer.GetStructureText(4));
			Assert.AreEqual("P09", buffer.GetStructureText(5));
		}
		[Test]
		public void DeleteLineRangeFrom00_06()
		{
			// Operation
			buffer.DeleteLines(0, 7);

			// Verification
			Assert.AreEqual("A1ppp", document.GetThumbprint());

			Assert.AreEqual("P06", buffer.GetStructureText(0));
			Assert.AreEqual("Section 2", buffer.GetStructureText(1));
			Assert.AreEqual("P07", buffer.GetStructureText(2));
			Assert.AreEqual("P08", buffer.GetStructureText(3));
			Assert.AreEqual("P09", buffer.GetStructureText(4));
		}
		[Test]
		public void DeleteLineRangeFrom00_07()
		{
			// Operation
			buffer.DeleteLines(0, 8);

			// Verification
			Assert.AreEqual("Appp", document.GetThumbprint());

			Assert.AreEqual("Section 2", buffer.GetStructureText(0));
			Assert.AreEqual("P07", buffer.GetStructureText(1));
			Assert.AreEqual("P08", buffer.GetStructureText(2));
			Assert.AreEqual("P09", buffer.GetStructureText(3));
		}
		[Test]
		public void DeleteLineRangeFrom00_08()
		{
			// Operation
			buffer.DeleteLines(0, 9);

			// Verification
			Assert.AreEqual("App", document.GetThumbprint());

			Assert.AreEqual("P07", buffer.GetStructureText(0));
			Assert.AreEqual("P08", buffer.GetStructureText(1));
			Assert.AreEqual("P09", buffer.GetStructureText(2));
		}
		[Test]
		public void DeleteLineRangeFrom00_09()
		{
			// Operation
			buffer.DeleteLines(0, 10);

			// Verification
			Assert.AreEqual("Ap", document.GetThumbprint());

			Assert.AreEqual("P08", buffer.GetStructureText(0));
			Assert.AreEqual("P09", buffer.GetStructureText(1));
		}
		[Test]
		public void DeleteLineRangeFrom00_10()
		{
			// Operation
			buffer.DeleteLines(0, 11);

			// Verification
			Assert.AreEqual("A", document.GetThumbprint());

			Assert.AreEqual("P09", buffer.GetStructureText(0));
		}
		[Test]
		public void DeleteLineRangeFrom01_01()
		{
			// Operation
			buffer.DeleteLines(1, 1);

			// Verification
			Assert.AreEqual("App1ppp1ppp", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
			Assert.AreEqual("P02", buffer.GetStructureText(1));
			Assert.AreEqual("P03", buffer.GetStructureText(2));
			Assert.AreEqual("Section 1", buffer.GetStructureText(3));
			Assert.AreEqual("P04", buffer.GetStructureText(4));
			Assert.AreEqual("P05", buffer.GetStructureText(5));
			Assert.AreEqual("P06", buffer.GetStructureText(6));
			Assert.AreEqual("Section 2", buffer.GetStructureText(7));
			Assert.AreEqual("P07", buffer.GetStructureText(8));
			Assert.AreEqual("P08", buffer.GetStructureText(9));
			Assert.AreEqual("P09", buffer.GetStructureText(10));
		}
		[Test]
		public void DeleteLineRangeFrom01_02()
		{
			// Operation
			buffer.DeleteLines(1, 2);

			// Verification
			Assert.AreEqual("Ap1ppp1ppp", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
			Assert.AreEqual("P03", buffer.GetStructureText(1));
			Assert.AreEqual("Section 1", buffer.GetStructureText(2));
			Assert.AreEqual("P04", buffer.GetStructureText(3));
			Assert.AreEqual("P05", buffer.GetStructureText(4));
			Assert.AreEqual("P06", buffer.GetStructureText(5));
			Assert.AreEqual("Section 2", buffer.GetStructureText(6));
			Assert.AreEqual("P07", buffer.GetStructureText(7));
			Assert.AreEqual("P08", buffer.GetStructureText(8));
			Assert.AreEqual("P09", buffer.GetStructureText(9));
		}
		[Test]
		public void DeleteLineRangeFrom01_03()
		{
			// Operation
			buffer.DeleteLines(1, 3);

			// Verification
			Assert.AreEqual("A1ppp1ppp", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
			Assert.AreEqual("Section 1", buffer.GetStructureText(1));
			Assert.AreEqual("P04", buffer.GetStructureText(2));
			Assert.AreEqual("P05", buffer.GetStructureText(3));
			Assert.AreEqual("P06", buffer.GetStructureText(4));
			Assert.AreEqual("Section 2", buffer.GetStructureText(5));
			Assert.AreEqual("P07", buffer.GetStructureText(6));
			Assert.AreEqual("P08", buffer.GetStructureText(7));
			Assert.AreEqual("P09", buffer.GetStructureText(8));
		}
		[Test]
		public void DeleteLineRangeFrom01_04()
		{
			// Operation
			buffer.DeleteLines(1, 4);

			// Verification
			Assert.AreEqual("Appp1ppp", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
			Assert.AreEqual("P04", buffer.GetStructureText(1));
			Assert.AreEqual("P05", buffer.GetStructureText(2));
			Assert.AreEqual("P06", buffer.GetStructureText(3));
			Assert.AreEqual("Section 2", buffer.GetStructureText(4));
			Assert.AreEqual("P07", buffer.GetStructureText(5));
			Assert.AreEqual("P08", buffer.GetStructureText(6));
			Assert.AreEqual("P09", buffer.GetStructureText(7));
		}
		[Test]
		public void DeleteLineRangeFrom01_05()
		{
			// Operation
			buffer.DeleteLines(1, 5);

			// Verification
			Assert.AreEqual("App1ppp", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
			Assert.AreEqual("P05", buffer.GetStructureText(1));
			Assert.AreEqual("P06", buffer.GetStructureText(2));
			Assert.AreEqual("Section 2", buffer.GetStructureText(3));
			Assert.AreEqual("P07", buffer.GetStructureText(4));
			Assert.AreEqual("P08", buffer.GetStructureText(5));
			Assert.AreEqual("P09", buffer.GetStructureText(6));
		}
		[Test]
		public void DeleteLineRangeFrom01_06()
		{
			// Operation
			buffer.DeleteLines(1, 6);

			// Verification
			Assert.AreEqual("Ap1ppp", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
			Assert.AreEqual("P06", buffer.GetStructureText(1));
			Assert.AreEqual("Section 2", buffer.GetStructureText(2));
			Assert.AreEqual("P07", buffer.GetStructureText(3));
			Assert.AreEqual("P08", buffer.GetStructureText(4));
			Assert.AreEqual("P09", buffer.GetStructureText(5));
		}
		[Test]
		public void DeleteLineRangeFrom01_07()
		{
			// Operation
			buffer.DeleteLines(1, 7);

			// Verification
			Assert.AreEqual("A1ppp", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
			Assert.AreEqual("Section 2", buffer.GetStructureText(1));
			Assert.AreEqual("P07", buffer.GetStructureText(2));
			Assert.AreEqual("P08", buffer.GetStructureText(3));
			Assert.AreEqual("P09", buffer.GetStructureText(4));
		}
		[Test]
		public void DeleteLineRangeFrom01_08()
		{
			// Operation
			buffer.DeleteLines(1, 8);

			// Verification
			Assert.AreEqual("Appp", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
			Assert.AreEqual("P07", buffer.GetStructureText(1));
			Assert.AreEqual("P08", buffer.GetStructureText(2));
			Assert.AreEqual("P09", buffer.GetStructureText(3));
		}
		[Test]
		public void DeleteLineRangeFrom01_09()
		{
			// Operation
			buffer.DeleteLines(1, 9);

			// Verification
			Assert.AreEqual("App", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
			Assert.AreEqual("P08", buffer.GetStructureText(1));
			Assert.AreEqual("P09", buffer.GetStructureText(2));
		}
		[Test]
		public void DeleteLineRangeFrom01_10()
		{
			// Operation
			buffer.DeleteLines(1, 10);

			// Verification
			Assert.AreEqual("Ap", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
			Assert.AreEqual("P09", buffer.GetStructureText(1));
		}
		[Test]
		public void DeleteLineRangeFrom01_11()
		{
			// Operation
			buffer.DeleteLines(1, 11);

			// Verification
			Assert.AreEqual("A", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
		}
		[Test]
		public void DeleteLineRangeFrom02_02()
		{
			// Operation
			buffer.DeleteLines(2, 1);

			// Verification
			Assert.AreEqual("App1ppp1ppp", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
			Assert.AreEqual("P01", buffer.GetStructureText(1));
			Assert.AreEqual("P03", buffer.GetStructureText(2));
			Assert.AreEqual("Section 1", buffer.GetStructureText(3));
			Assert.AreEqual("P04", buffer.GetStructureText(4));
			Assert.AreEqual("P05", buffer.GetStructureText(5));
			Assert.AreEqual("P06", buffer.GetStructureText(6));
			Assert.AreEqual("Section 2", buffer.GetStructureText(7));
			Assert.AreEqual("P07", buffer.GetStructureText(8));
			Assert.AreEqual("P08", buffer.GetStructureText(9));
			Assert.AreEqual("P09", buffer.GetStructureText(10));
		}
		[Test]
		public void DeleteLineRangeFrom02_03()
		{
			// Operation
			buffer.DeleteLines(2, 2);

			// Verification
			Assert.AreEqual("Ap1ppp1ppp", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
			Assert.AreEqual("P01", buffer.GetStructureText(1));
			Assert.AreEqual("Section 1", buffer.GetStructureText(2));
			Assert.AreEqual("P04", buffer.GetStructureText(3));
			Assert.AreEqual("P05", buffer.GetStructureText(4));
			Assert.AreEqual("P06", buffer.GetStructureText(5));
			Assert.AreEqual("Section 2", buffer.GetStructureText(6));
			Assert.AreEqual("P07", buffer.GetStructureText(7));
			Assert.AreEqual("P08", buffer.GetStructureText(8));
			Assert.AreEqual("P09", buffer.GetStructureText(9));
		}
		[Test]
		public void DeleteLineRangeFrom02_04()
		{
			// Operation
			buffer.DeleteLines(2, 3);

			// Verification
			Assert.AreEqual("Apppp1ppp", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
			Assert.AreEqual("P01", buffer.GetStructureText(1));
			Assert.AreEqual("P04", buffer.GetStructureText(2));
			Assert.AreEqual("P05", buffer.GetStructureText(3));
			Assert.AreEqual("P06", buffer.GetStructureText(4));
			Assert.AreEqual("Section 2", buffer.GetStructureText(5));
			Assert.AreEqual("P07", buffer.GetStructureText(6));
			Assert.AreEqual("P08", buffer.GetStructureText(7));
			Assert.AreEqual("P09", buffer.GetStructureText(8));
		}
		[Test]
		public void DeleteLineRangeFrom02_05()
		{
			// Operation
			buffer.DeleteLines(2, 4);

			// Verification
			Assert.AreEqual("Appp1ppp", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
			Assert.AreEqual("P01", buffer.GetStructureText(1));
			Assert.AreEqual("P05", buffer.GetStructureText(2));
			Assert.AreEqual("P06", buffer.GetStructureText(3));
			Assert.AreEqual("Section 2", buffer.GetStructureText(4));
			Assert.AreEqual("P07", buffer.GetStructureText(5));
			Assert.AreEqual("P08", buffer.GetStructureText(6));
			Assert.AreEqual("P09", buffer.GetStructureText(7));
		}
		[Test]
		public void DeleteLineRangeFrom02_06()
		{
			// Operation
			buffer.DeleteLines(2, 5);

			// Verification
			Assert.AreEqual("App1ppp", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
			Assert.AreEqual("P01", buffer.GetStructureText(1));
			Assert.AreEqual("P06", buffer.GetStructureText(2));
			Assert.AreEqual("Section 2", buffer.GetStructureText(3));
			Assert.AreEqual("P07", buffer.GetStructureText(4));
			Assert.AreEqual("P08", buffer.GetStructureText(5));
			Assert.AreEqual("P09", buffer.GetStructureText(6));
		}
		[Test]
		public void DeleteLineRangeFrom02_07()
		{
			// Operation
			buffer.DeleteLines(2, 6);

			// Verification
			Assert.AreEqual("Ap1ppp", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
			Assert.AreEqual("P01", buffer.GetStructureText(1));
			Assert.AreEqual("Section 2", buffer.GetStructureText(2));
			Assert.AreEqual("P07", buffer.GetStructureText(3));
			Assert.AreEqual("P08", buffer.GetStructureText(4));
			Assert.AreEqual("P09", buffer.GetStructureText(5));
		}
		[Test]
		public void DeleteLineRangeFrom02_08()
		{
			// Operation
			buffer.DeleteLines(2, 7);

			// Verification
			Assert.AreEqual("Apppp", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
			Assert.AreEqual("P01", buffer.GetStructureText(1));
			Assert.AreEqual("P07", buffer.GetStructureText(2));
			Assert.AreEqual("P08", buffer.GetStructureText(3));
			Assert.AreEqual("P09", buffer.GetStructureText(4));
		}
		[Test]
		public void DeleteLineRangeFrom02_09()
		{
			// Operation
			buffer.DeleteLines(2, 8);

			// Verification
			Assert.AreEqual("Appp", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
			Assert.AreEqual("P01", buffer.GetStructureText(1));
			Assert.AreEqual("P08", buffer.GetStructureText(2));
			Assert.AreEqual("P09", buffer.GetStructureText(3));
		}
		[Test]
		public void DeleteLineRangeFrom02_10()
		{
			// Operation
			buffer.DeleteLines(2, 9);

			// Verification
			Assert.AreEqual("App", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
			Assert.AreEqual("P01", buffer.GetStructureText(1));
			Assert.AreEqual("P09", buffer.GetStructureText(2));
		}
		[Test]
		public void DeleteLineRangeFrom02_11()
		{
			// Operation
			buffer.DeleteLines(2, 10);

			// Verification
			Assert.AreEqual("Ap", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
			Assert.AreEqual("P01", buffer.GetStructureText(1));
		}
		[Test]
		public void DeleteLineRangeFrom03_03()
		{
			// Operation
			buffer.DeleteLines(3, 1);

			// Verification
			Assert.AreEqual("App1ppp1ppp", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
			Assert.AreEqual("P01", buffer.GetStructureText(1));
			Assert.AreEqual("P02", buffer.GetStructureText(2));
			Assert.AreEqual("Section 1", buffer.GetStructureText(3));
			Assert.AreEqual("P04", buffer.GetStructureText(4));
			Assert.AreEqual("P05", buffer.GetStructureText(5));
			Assert.AreEqual("P06", buffer.GetStructureText(6));
			Assert.AreEqual("Section 2", buffer.GetStructureText(7));
			Assert.AreEqual("P07", buffer.GetStructureText(8));
			Assert.AreEqual("P08", buffer.GetStructureText(9));
			Assert.AreEqual("P09", buffer.GetStructureText(10));
		}
		[Test]
		public void DeleteLineRangeFrom03_04()
		{
			// Operation
			buffer.DeleteLines(3, 2);

			// Verification
			Assert.AreEqual("Appppp1ppp", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
			Assert.AreEqual("P01", buffer.GetStructureText(1));
			Assert.AreEqual("P02", buffer.GetStructureText(2));
			Assert.AreEqual("P04", buffer.GetStructureText(3));
			Assert.AreEqual("P05", buffer.GetStructureText(4));
			Assert.AreEqual("P06", buffer.GetStructureText(5));
			Assert.AreEqual("Section 2", buffer.GetStructureText(6));
			Assert.AreEqual("P07", buffer.GetStructureText(7));
			Assert.AreEqual("P08", buffer.GetStructureText(8));
			Assert.AreEqual("P09", buffer.GetStructureText(9));
		}
		[Test]
		public void DeleteLineRangeFrom03_05()
		{
			// Operation
			buffer.DeleteLines(3, 3);

			// Verification
			Assert.AreEqual("Apppp1ppp", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
			Assert.AreEqual("P01", buffer.GetStructureText(1));
			Assert.AreEqual("P02", buffer.GetStructureText(2));
			Assert.AreEqual("P05", buffer.GetStructureText(3));
			Assert.AreEqual("P06", buffer.GetStructureText(4));
			Assert.AreEqual("Section 2", buffer.GetStructureText(5));
			Assert.AreEqual("P07", buffer.GetStructureText(6));
			Assert.AreEqual("P08", buffer.GetStructureText(7));
			Assert.AreEqual("P09", buffer.GetStructureText(8));
		}
		[Test]
		public void DeleteLineRangeFrom03_06()
		{
			// Operation
			buffer.DeleteLines(3, 4);

			// Verification
			Assert.AreEqual("Appp1ppp", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
			Assert.AreEqual("P01", buffer.GetStructureText(1));
			Assert.AreEqual("P02", buffer.GetStructureText(2));
			Assert.AreEqual("P06", buffer.GetStructureText(3));
			Assert.AreEqual("Section 2", buffer.GetStructureText(4));
			Assert.AreEqual("P07", buffer.GetStructureText(5));
			Assert.AreEqual("P08", buffer.GetStructureText(6));
			Assert.AreEqual("P09", buffer.GetStructureText(7));
		}
		[Test]
		public void DeleteLineRangeFrom03_07()
		{
			// Operation
			buffer.DeleteLines(3, 5);

			// Verification
			Assert.AreEqual("App1ppp", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
			Assert.AreEqual("P01", buffer.GetStructureText(1));
			Assert.AreEqual("P02", buffer.GetStructureText(2));
			Assert.AreEqual("Section 2", buffer.GetStructureText(3));
			Assert.AreEqual("P07", buffer.GetStructureText(4));
			Assert.AreEqual("P08", buffer.GetStructureText(5));
			Assert.AreEqual("P09", buffer.GetStructureText(6));
		}
		[Test]
		public void DeleteLineRangeFrom03_08()
		{
			// Operation
			buffer.DeleteLines(3, 6);

			// Verification
			Assert.AreEqual("Appppp", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
			Assert.AreEqual("P01", buffer.GetStructureText(1));
			Assert.AreEqual("P02", buffer.GetStructureText(2));
			Assert.AreEqual("P07", buffer.GetStructureText(3));
			Assert.AreEqual("P08", buffer.GetStructureText(4));
			Assert.AreEqual("P09", buffer.GetStructureText(5));
		}
		[Test]
		public void DeleteLineRangeFrom03_09()
		{
			// Operation
			buffer.DeleteLines(3, 7);

			// Verification
			Assert.AreEqual("Apppp", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
			Assert.AreEqual("P01", buffer.GetStructureText(1));
			Assert.AreEqual("P02", buffer.GetStructureText(2));
			Assert.AreEqual("P08", buffer.GetStructureText(3));
			Assert.AreEqual("P09", buffer.GetStructureText(4));
		}
		[Test]
		public void DeleteLineRangeFrom03_10()
		{
			// Operation
			buffer.DeleteLines(3, 8);

			// Verification
			Assert.AreEqual("Appp", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
			Assert.AreEqual("P01", buffer.GetStructureText(1));
			Assert.AreEqual("P02", buffer.GetStructureText(2));
			Assert.AreEqual("P09", buffer.GetStructureText(3));
		}
		[Test]
		public void DeleteLineRangeFrom03_11()
		{
			// Operation
			buffer.DeleteLines(3, 9);

			// Verification
			Assert.AreEqual("App", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
			Assert.AreEqual("P01", buffer.GetStructureText(1));
			Assert.AreEqual("P02", buffer.GetStructureText(2));
		}
		[Test]
		public void DeleteLineRangeFrom04_04()
		{
			// Operation
			buffer.DeleteLines(4, 1);

			// Verification
			Assert.AreEqual("Apppppp1ppp", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
			Assert.AreEqual("P01", buffer.GetStructureText(1));
			Assert.AreEqual("P02", buffer.GetStructureText(2));
			Assert.AreEqual("P03", buffer.GetStructureText(3));
			Assert.AreEqual("P04", buffer.GetStructureText(4));
			Assert.AreEqual("P05", buffer.GetStructureText(5));
			Assert.AreEqual("P06", buffer.GetStructureText(6));
			Assert.AreEqual("Section 2", buffer.GetStructureText(7));
			Assert.AreEqual("P07", buffer.GetStructureText(8));
			Assert.AreEqual("P08", buffer.GetStructureText(9));
			Assert.AreEqual("P09", buffer.GetStructureText(10));
		}
		[Test]
		public void DeleteLineRangeFrom04_05()
		{
			// Operation
			buffer.DeleteLines(4, 2);

			// Verification
			Assert.AreEqual("Appppp1ppp", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
			Assert.AreEqual("P01", buffer.GetStructureText(1));
			Assert.AreEqual("P02", buffer.GetStructureText(2));
			Assert.AreEqual("P03", buffer.GetStructureText(3));
			Assert.AreEqual("P05", buffer.GetStructureText(4));
			Assert.AreEqual("P06", buffer.GetStructureText(5));
			Assert.AreEqual("Section 2", buffer.GetStructureText(6));
			Assert.AreEqual("P07", buffer.GetStructureText(7));
			Assert.AreEqual("P08", buffer.GetStructureText(8));
			Assert.AreEqual("P09", buffer.GetStructureText(9));
		}
		[Test]
		public void DeleteLineRangeFrom04_06()
		{
			// Operation
			buffer.DeleteLines(4, 3);

			// Verification
			Assert.AreEqual("Apppp1ppp", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
			Assert.AreEqual("P01", buffer.GetStructureText(1));
			Assert.AreEqual("P02", buffer.GetStructureText(2));
			Assert.AreEqual("P03", buffer.GetStructureText(3));
			Assert.AreEqual("P06", buffer.GetStructureText(4));
			Assert.AreEqual("Section 2", buffer.GetStructureText(5));
			Assert.AreEqual("P07", buffer.GetStructureText(6));
			Assert.AreEqual("P08", buffer.GetStructureText(7));
			Assert.AreEqual("P09", buffer.GetStructureText(8));
		}
		[Test]
		public void DeleteLineRangeFrom04_07()
		{
			// Operation
			buffer.DeleteLines(4, 4);

			// Verification
			Assert.AreEqual("Appp1ppp", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
			Assert.AreEqual("P01", buffer.GetStructureText(1));
			Assert.AreEqual("P02", buffer.GetStructureText(2));
			Assert.AreEqual("P03", buffer.GetStructureText(3));
			Assert.AreEqual("Section 2", buffer.GetStructureText(4));
			Assert.AreEqual("P07", buffer.GetStructureText(5));
			Assert.AreEqual("P08", buffer.GetStructureText(6));
			Assert.AreEqual("P09", buffer.GetStructureText(7));
		}
		[Test]
		public void DeleteLineRangeFrom04_08()
		{
			// Operation
			buffer.DeleteLines(4, 5);

			// Verification
			Assert.AreEqual("Apppppp", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
			Assert.AreEqual("P01", buffer.GetStructureText(1));
			Assert.AreEqual("P02", buffer.GetStructureText(2));
			Assert.AreEqual("P03", buffer.GetStructureText(3));
			Assert.AreEqual("P07", buffer.GetStructureText(4));
			Assert.AreEqual("P08", buffer.GetStructureText(5));
			Assert.AreEqual("P09", buffer.GetStructureText(6));
		}
		[Test]
		public void DeleteLineRangeFrom04_09()
		{
			// Operation
			buffer.DeleteLines(4, 6);

			// Verification
			Assert.AreEqual("Appppp", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
			Assert.AreEqual("P01", buffer.GetStructureText(1));
			Assert.AreEqual("P02", buffer.GetStructureText(2));
			Assert.AreEqual("P03", buffer.GetStructureText(3));
			Assert.AreEqual("P08", buffer.GetStructureText(4));
			Assert.AreEqual("P09", buffer.GetStructureText(5));
		}
		[Test]
		public void DeleteLineRangeFrom04_10()
		{
			// Operation
			buffer.DeleteLines(4, 7);

			// Verification
			Assert.AreEqual("Apppp", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
			Assert.AreEqual("P01", buffer.GetStructureText(1));
			Assert.AreEqual("P02", buffer.GetStructureText(2));
			Assert.AreEqual("P03", buffer.GetStructureText(3));
			Assert.AreEqual("P09", buffer.GetStructureText(4));
		}
		[Test]
		public void DeleteLineRangeFrom04_11()
		{
			// Operation
			buffer.DeleteLines(4, 8);

			// Verification
			Assert.AreEqual("Appp", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
			Assert.AreEqual("P01", buffer.GetStructureText(1));
			Assert.AreEqual("P02", buffer.GetStructureText(2));
			Assert.AreEqual("P03", buffer.GetStructureText(3));
		}
		[Test]
		public void DeleteLineRangeFrom05_05()
		{
			// Operation
			buffer.DeleteLines(5, 1);

			// Verification
			Assert.AreEqual("Appp1pp1ppp", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
			Assert.AreEqual("P01", buffer.GetStructureText(1));
			Assert.AreEqual("P02", buffer.GetStructureText(2));
			Assert.AreEqual("P03", buffer.GetStructureText(3));
			Assert.AreEqual("Section 1", buffer.GetStructureText(4));
			Assert.AreEqual("P05", buffer.GetStructureText(5));
			Assert.AreEqual("P06", buffer.GetStructureText(6));
			Assert.AreEqual("Section 2", buffer.GetStructureText(7));
			Assert.AreEqual("P07", buffer.GetStructureText(8));
			Assert.AreEqual("P08", buffer.GetStructureText(9));
			Assert.AreEqual("P09", buffer.GetStructureText(10));
		}
		[Test]
		public void DeleteLineRangeFrom05_06()
		{
			// Operation
			buffer.DeleteLines(5, 2);

			// Verification
			Assert.AreEqual("Appp1p1ppp", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
			Assert.AreEqual("P01", buffer.GetStructureText(1));
			Assert.AreEqual("P02", buffer.GetStructureText(2));
			Assert.AreEqual("P03", buffer.GetStructureText(3));
			Assert.AreEqual("Section 1", buffer.GetStructureText(4));
			Assert.AreEqual("P06", buffer.GetStructureText(5));
			Assert.AreEqual("Section 2", buffer.GetStructureText(6));
			Assert.AreEqual("P07", buffer.GetStructureText(7));
			Assert.AreEqual("P08", buffer.GetStructureText(8));
			Assert.AreEqual("P09", buffer.GetStructureText(9));
		}
		[Test]
		public void DeleteLineRangeFrom05_07()
		{
			// Operation
			buffer.DeleteLines(5, 3);

			// Verification
			Assert.AreEqual("Appp11ppp", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
			Assert.AreEqual("P01", buffer.GetStructureText(1));
			Assert.AreEqual("P02", buffer.GetStructureText(2));
			Assert.AreEqual("P03", buffer.GetStructureText(3));
			Assert.AreEqual("Section 1", buffer.GetStructureText(4));
			Assert.AreEqual("Section 2", buffer.GetStructureText(5));
			Assert.AreEqual("P07", buffer.GetStructureText(6));
			Assert.AreEqual("P08", buffer.GetStructureText(7));
			Assert.AreEqual("P09", buffer.GetStructureText(8));
		}
		[Test]
		public void DeleteLineRangeFrom05_08()
		{
			// Operation
			buffer.DeleteLines(5, 4);

			// Verification
			Assert.AreEqual("Appp1ppp", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
			Assert.AreEqual("P01", buffer.GetStructureText(1));
			Assert.AreEqual("P02", buffer.GetStructureText(2));
			Assert.AreEqual("P03", buffer.GetStructureText(3));
			Assert.AreEqual("Section 1", buffer.GetStructureText(4));
			Assert.AreEqual("P07", buffer.GetStructureText(5));
			Assert.AreEqual("P08", buffer.GetStructureText(6));
			Assert.AreEqual("P09", buffer.GetStructureText(7));
		}
		[Test]
		public void DeleteLineRangeFrom05_09()
		{
			// Operation
			buffer.DeleteLines(5, 5);

			// Verification
			Assert.AreEqual("Appp1pp", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
			Assert.AreEqual("P01", buffer.GetStructureText(1));
			Assert.AreEqual("P02", buffer.GetStructureText(2));
			Assert.AreEqual("P03", buffer.GetStructureText(3));
			Assert.AreEqual("Section 1", buffer.GetStructureText(4));
			Assert.AreEqual("P08", buffer.GetStructureText(5));
			Assert.AreEqual("P09", buffer.GetStructureText(6));
		}
		[Test]
		public void DeleteLineRangeFrom05_10()
		{
			// Operation
			buffer.DeleteLines(5, 6);

			// Verification
			Assert.AreEqual("Appp1p", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
			Assert.AreEqual("P01", buffer.GetStructureText(1));
			Assert.AreEqual("P02", buffer.GetStructureText(2));
			Assert.AreEqual("P03", buffer.GetStructureText(3));
			Assert.AreEqual("Section 1", buffer.GetStructureText(4));
			Assert.AreEqual("P09", buffer.GetStructureText(5));
		}
		[Test]
		public void DeleteLineRangeFrom05_11()
		{
			// Operation
			buffer.DeleteLines(5, 7);

			// Verification
			Assert.AreEqual("Appp1", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
			Assert.AreEqual("P01", buffer.GetStructureText(1));
			Assert.AreEqual("P02", buffer.GetStructureText(2));
			Assert.AreEqual("P03", buffer.GetStructureText(3));
			Assert.AreEqual("Section 1", buffer.GetStructureText(4));
		}
		[Test]
		public void DeleteLineRangeFrom06_06()
		{
			// Operation
			buffer.DeleteLines(6, 1);

			// Verification
			Assert.AreEqual("Appp1pp1ppp", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
			Assert.AreEqual("P01", buffer.GetStructureText(1));
			Assert.AreEqual("P02", buffer.GetStructureText(2));
			Assert.AreEqual("P03", buffer.GetStructureText(3));
			Assert.AreEqual("Section 1", buffer.GetStructureText(4));
			Assert.AreEqual("P04", buffer.GetStructureText(5));
			Assert.AreEqual("P06", buffer.GetStructureText(6));
			Assert.AreEqual("Section 2", buffer.GetStructureText(7));
			Assert.AreEqual("P07", buffer.GetStructureText(8));
			Assert.AreEqual("P08", buffer.GetStructureText(9));
			Assert.AreEqual("P09", buffer.GetStructureText(10));
		}
		[Test]
		public void DeleteLineRangeFrom06_07()
		{
			// Operation
			buffer.DeleteLines(6, 2);

			// Verification
			Assert.AreEqual("Appp1p1ppp", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
			Assert.AreEqual("P01", buffer.GetStructureText(1));
			Assert.AreEqual("P02", buffer.GetStructureText(2));
			Assert.AreEqual("P03", buffer.GetStructureText(3));
			Assert.AreEqual("Section 1", buffer.GetStructureText(4));
			Assert.AreEqual("P04", buffer.GetStructureText(5));
			Assert.AreEqual("Section 2", buffer.GetStructureText(6));
			Assert.AreEqual("P07", buffer.GetStructureText(7));
			Assert.AreEqual("P08", buffer.GetStructureText(8));
			Assert.AreEqual("P09", buffer.GetStructureText(9));
		}
		[Test]
		public void DeleteLineRangeFrom06_08()
		{
			// Operation
			buffer.DeleteLines(6, 3);

			// Verification
			Assert.AreEqual("Appp1pppp", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
			Assert.AreEqual("P01", buffer.GetStructureText(1));
			Assert.AreEqual("P02", buffer.GetStructureText(2));
			Assert.AreEqual("P03", buffer.GetStructureText(3));
			Assert.AreEqual("Section 1", buffer.GetStructureText(4));
			Assert.AreEqual("P04", buffer.GetStructureText(5));
			Assert.AreEqual("P07", buffer.GetStructureText(6));
			Assert.AreEqual("P08", buffer.GetStructureText(7));
			Assert.AreEqual("P09", buffer.GetStructureText(8));
		}
		[Test]
		public void DeleteLineRangeFrom06_09()
		{
			// Operation
			buffer.DeleteLines(6, 4);

			// Verification
			Assert.AreEqual("Appp1ppp", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
			Assert.AreEqual("P01", buffer.GetStructureText(1));
			Assert.AreEqual("P02", buffer.GetStructureText(2));
			Assert.AreEqual("P03", buffer.GetStructureText(3));
			Assert.AreEqual("Section 1", buffer.GetStructureText(4));
			Assert.AreEqual("P04", buffer.GetStructureText(5));
			Assert.AreEqual("P08", buffer.GetStructureText(6));
			Assert.AreEqual("P09", buffer.GetStructureText(7));
		}
		[Test]
		public void DeleteLineRangeFrom06_10()
		{
			// Operation
			buffer.DeleteLines(6, 5);

			// Verification
			Assert.AreEqual("Appp1pp", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
			Assert.AreEqual("P01", buffer.GetStructureText(1));
			Assert.AreEqual("P02", buffer.GetStructureText(2));
			Assert.AreEqual("P03", buffer.GetStructureText(3));
			Assert.AreEqual("Section 1", buffer.GetStructureText(4));
			Assert.AreEqual("P04", buffer.GetStructureText(5));
			Assert.AreEqual("P09", buffer.GetStructureText(6));
		}
		[Test]
		public void DeleteLineRangeFrom06_11()
		{
			// Operation
			buffer.DeleteLines(6, 6);

			// Verification
			Assert.AreEqual("Appp1p", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
			Assert.AreEqual("P01", buffer.GetStructureText(1));
			Assert.AreEqual("P02", buffer.GetStructureText(2));
			Assert.AreEqual("P03", buffer.GetStructureText(3));
			Assert.AreEqual("Section 1", buffer.GetStructureText(4));
			Assert.AreEqual("P04", buffer.GetStructureText(5));
		}
		[Test]
		public void DeleteLineRangeFrom07_07()
		{
			// Operation
			buffer.DeleteLines(7, 1);

			// Verification
			Assert.AreEqual("Appp1pp1ppp", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
			Assert.AreEqual("P01", buffer.GetStructureText(1));
			Assert.AreEqual("P02", buffer.GetStructureText(2));
			Assert.AreEqual("P03", buffer.GetStructureText(3));
			Assert.AreEqual("Section 1", buffer.GetStructureText(4));
			Assert.AreEqual("P04", buffer.GetStructureText(5));
			Assert.AreEqual("P05", buffer.GetStructureText(6));
			Assert.AreEqual("Section 2", buffer.GetStructureText(7));
			Assert.AreEqual("P07", buffer.GetStructureText(8));
			Assert.AreEqual("P08", buffer.GetStructureText(9));
			Assert.AreEqual("P09", buffer.GetStructureText(10));
		}
		[Test]
		public void DeleteLineRangeFrom07_08()
		{
			// Operation
			buffer.DeleteLines(7, 2);

			// Verification
			Assert.AreEqual("Appp1ppppp", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
			Assert.AreEqual("P01", buffer.GetStructureText(1));
			Assert.AreEqual("P02", buffer.GetStructureText(2));
			Assert.AreEqual("P03", buffer.GetStructureText(3));
			Assert.AreEqual("Section 1", buffer.GetStructureText(4));
			Assert.AreEqual("P04", buffer.GetStructureText(5));
			Assert.AreEqual("P05", buffer.GetStructureText(6));
			Assert.AreEqual("P07", buffer.GetStructureText(7));
			Assert.AreEqual("P08", buffer.GetStructureText(8));
			Assert.AreEqual("P09", buffer.GetStructureText(9));
		}
		[Test]
		public void DeleteLineRangeFrom07_09()
		{
			// Operation
			buffer.DeleteLines(7, 3);

			// Verification
			Assert.AreEqual("Appp1pppp", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
			Assert.AreEqual("P01", buffer.GetStructureText(1));
			Assert.AreEqual("P02", buffer.GetStructureText(2));
			Assert.AreEqual("P03", buffer.GetStructureText(3));
			Assert.AreEqual("Section 1", buffer.GetStructureText(4));
			Assert.AreEqual("P04", buffer.GetStructureText(5));
			Assert.AreEqual("P05", buffer.GetStructureText(6));
			Assert.AreEqual("P08", buffer.GetStructureText(7));
			Assert.AreEqual("P09", buffer.GetStructureText(8));
		}
		[Test]
		public void DeleteLineRangeFrom07_10()
		{
			// Operation
			buffer.DeleteLines(7, 4);

			// Verification
			Assert.AreEqual("Appp1ppp", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
			Assert.AreEqual("P01", buffer.GetStructureText(1));
			Assert.AreEqual("P02", buffer.GetStructureText(2));
			Assert.AreEqual("P03", buffer.GetStructureText(3));
			Assert.AreEqual("Section 1", buffer.GetStructureText(4));
			Assert.AreEqual("P04", buffer.GetStructureText(5));
			Assert.AreEqual("P05", buffer.GetStructureText(6));
			Assert.AreEqual("P09", buffer.GetStructureText(7));
		}
		[Test]
		public void DeleteLineRangeFrom07_11()
		{
			// Operation
			buffer.DeleteLines(7, 5);

			// Verification
			Assert.AreEqual("Appp1pp", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
			Assert.AreEqual("P01", buffer.GetStructureText(1));
			Assert.AreEqual("P02", buffer.GetStructureText(2));
			Assert.AreEqual("P03", buffer.GetStructureText(3));
			Assert.AreEqual("Section 1", buffer.GetStructureText(4));
			Assert.AreEqual("P04", buffer.GetStructureText(5));
			Assert.AreEqual("P05", buffer.GetStructureText(6));
		}
		[Test]
		public void DeleteLineRangeFrom08_08()
		{
			// Operation
			buffer.DeleteLines(8, 1);

			// Verification
			Assert.AreEqual("Appp1pppppp", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
			Assert.AreEqual("P01", buffer.GetStructureText(1));
			Assert.AreEqual("P02", buffer.GetStructureText(2));
			Assert.AreEqual("P03", buffer.GetStructureText(3));
			Assert.AreEqual("Section 1", buffer.GetStructureText(4));
			Assert.AreEqual("P04", buffer.GetStructureText(5));
			Assert.AreEqual("P05", buffer.GetStructureText(6));
			Assert.AreEqual("P06", buffer.GetStructureText(7));
			Assert.AreEqual("P07", buffer.GetStructureText(8));
			Assert.AreEqual("P08", buffer.GetStructureText(9));
			Assert.AreEqual("P09", buffer.GetStructureText(10));
		}
		[Test]
		public void DeleteLineRangeFrom08_09()
		{
			// Operation
			buffer.DeleteLines(8, 2);

			// Verification
			Assert.AreEqual("Appp1ppppp", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
			Assert.AreEqual("P01", buffer.GetStructureText(1));
			Assert.AreEqual("P02", buffer.GetStructureText(2));
			Assert.AreEqual("P03", buffer.GetStructureText(3));
			Assert.AreEqual("Section 1", buffer.GetStructureText(4));
			Assert.AreEqual("P04", buffer.GetStructureText(5));
			Assert.AreEqual("P05", buffer.GetStructureText(6));
			Assert.AreEqual("P06", buffer.GetStructureText(7));
			Assert.AreEqual("P08", buffer.GetStructureText(8));
			Assert.AreEqual("P09", buffer.GetStructureText(9));
		}
		[Test]
		public void DeleteLineRangeFrom08_10()
		{
			// Operation
			buffer.DeleteLines(8, 3);

			// Verification
			Assert.AreEqual("Appp1pppp", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
			Assert.AreEqual("P01", buffer.GetStructureText(1));
			Assert.AreEqual("P02", buffer.GetStructureText(2));
			Assert.AreEqual("P03", buffer.GetStructureText(3));
			Assert.AreEqual("Section 1", buffer.GetStructureText(4));
			Assert.AreEqual("P04", buffer.GetStructureText(5));
			Assert.AreEqual("P05", buffer.GetStructureText(6));
			Assert.AreEqual("P06", buffer.GetStructureText(7));
			Assert.AreEqual("P09", buffer.GetStructureText(8));
		}
		[Test]
		public void DeleteLineRangeFrom08_11()
		{
			// Operation
			buffer.DeleteLines(8, 4);

			// Verification
			Assert.AreEqual("Appp1ppp", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
			Assert.AreEqual("P01", buffer.GetStructureText(1));
			Assert.AreEqual("P02", buffer.GetStructureText(2));
			Assert.AreEqual("P03", buffer.GetStructureText(3));
			Assert.AreEqual("Section 1", buffer.GetStructureText(4));
			Assert.AreEqual("P04", buffer.GetStructureText(5));
			Assert.AreEqual("P05", buffer.GetStructureText(6));
			Assert.AreEqual("P06", buffer.GetStructureText(7));
		}
		[Test]
		public void DeleteLineRangeFrom09_09()
		{
			// Operation
			buffer.DeleteLines(9, 1);

			// Verification
			Assert.AreEqual("Appp1ppp1pp", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
			Assert.AreEqual("P01", buffer.GetStructureText(1));
			Assert.AreEqual("P02", buffer.GetStructureText(2));
			Assert.AreEqual("P03", buffer.GetStructureText(3));
			Assert.AreEqual("Section 1", buffer.GetStructureText(4));
			Assert.AreEqual("P04", buffer.GetStructureText(5));
			Assert.AreEqual("P05", buffer.GetStructureText(6));
			Assert.AreEqual("P06", buffer.GetStructureText(7));
			Assert.AreEqual("Section 2", buffer.GetStructureText(8));
			Assert.AreEqual("P08", buffer.GetStructureText(9));
			Assert.AreEqual("P09", buffer.GetStructureText(10));
		}
		[Test]
		public void DeleteLineRangeFrom09_10()
		{
			// Operation
			buffer.DeleteLines(9, 2);

			// Verification
			Assert.AreEqual("Appp1ppp1p", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
			Assert.AreEqual("P01", buffer.GetStructureText(1));
			Assert.AreEqual("P02", buffer.GetStructureText(2));
			Assert.AreEqual("P03", buffer.GetStructureText(3));
			Assert.AreEqual("Section 1", buffer.GetStructureText(4));
			Assert.AreEqual("P04", buffer.GetStructureText(5));
			Assert.AreEqual("P05", buffer.GetStructureText(6));
			Assert.AreEqual("P06", buffer.GetStructureText(7));
			Assert.AreEqual("Section 2", buffer.GetStructureText(8));
			Assert.AreEqual("P09", buffer.GetStructureText(9));
		}
		[Test]
		public void DeleteLineRangeFrom09_11()
		{
			// Operation
			buffer.DeleteLines(9, 3);

			// Verification
			Assert.AreEqual("Appp1ppp1", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
			Assert.AreEqual("P01", buffer.GetStructureText(1));
			Assert.AreEqual("P02", buffer.GetStructureText(2));
			Assert.AreEqual("P03", buffer.GetStructureText(3));
			Assert.AreEqual("Section 1", buffer.GetStructureText(4));
			Assert.AreEqual("P04", buffer.GetStructureText(5));
			Assert.AreEqual("P05", buffer.GetStructureText(6));
			Assert.AreEqual("P06", buffer.GetStructureText(7));
			Assert.AreEqual("Section 2", buffer.GetStructureText(8));
		}
		[Test]
		public void DeleteLineRangeFrom10_10()
		{
			// Operation
			buffer.DeleteLines(10, 1);

			// Verification
			Assert.AreEqual("Appp1ppp1pp", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
			Assert.AreEqual("P01", buffer.GetStructureText(1));
			Assert.AreEqual("P02", buffer.GetStructureText(2));
			Assert.AreEqual("P03", buffer.GetStructureText(3));
			Assert.AreEqual("Section 1", buffer.GetStructureText(4));
			Assert.AreEqual("P04", buffer.GetStructureText(5));
			Assert.AreEqual("P05", buffer.GetStructureText(6));
			Assert.AreEqual("P06", buffer.GetStructureText(7));
			Assert.AreEqual("Section 2", buffer.GetStructureText(8));
			Assert.AreEqual("P07", buffer.GetStructureText(9));
			Assert.AreEqual("P09", buffer.GetStructureText(10));
		}
		[Test]
		public void DeleteLineRangeFrom10_11()
		{
			// Operation
			buffer.DeleteLines(10, 2);

			// Verification
			Assert.AreEqual("Appp1ppp1p", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
			Assert.AreEqual("P01", buffer.GetStructureText(1));
			Assert.AreEqual("P02", buffer.GetStructureText(2));
			Assert.AreEqual("P03", buffer.GetStructureText(3));
			Assert.AreEqual("Section 1", buffer.GetStructureText(4));
			Assert.AreEqual("P04", buffer.GetStructureText(5));
			Assert.AreEqual("P05", buffer.GetStructureText(6));
			Assert.AreEqual("P06", buffer.GetStructureText(7));
			Assert.AreEqual("Section 2", buffer.GetStructureText(8));
			Assert.AreEqual("P07", buffer.GetStructureText(9));
		}
		[Test]
		public void DeleteLineRangeFrom11_11()
		{
			// Operation
			buffer.DeleteLines(11, 1);

			// Verification
			Assert.AreEqual("Appp1ppp1pp", document.GetThumbprint());

			Assert.AreEqual("Article", buffer.GetStructureText(0));
			Assert.AreEqual("P01", buffer.GetStructureText(1));
			Assert.AreEqual("P02", buffer.GetStructureText(2));
			Assert.AreEqual("P03", buffer.GetStructureText(3));
			Assert.AreEqual("Section 1", buffer.GetStructureText(4));
			Assert.AreEqual("P04", buffer.GetStructureText(5));
			Assert.AreEqual("P05", buffer.GetStructureText(6));
			Assert.AreEqual("P06", buffer.GetStructureText(7));
			Assert.AreEqual("Section 2", buffer.GetStructureText(8));
			Assert.AreEqual("P07", buffer.GetStructureText(9));
			Assert.AreEqual("P08", buffer.GetStructureText(10));
		}

		#endregion
	}
}