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

using AuthorIntrusionGtk.Editors;

using NUnit.Framework;

#endregion

namespace UnitTests
{
	/// <summary>
	/// Tests the functionality of the <see cref="DocumentLineBuffer"/> class when
	/// dealing with structural changes, such as adding and deleting lines.
	/// </summary>
	[TestFixture]
	public class DocumentLineBufferStructureTests
	{
		#region Setup

		private DocumentLineBuffer buffer;
		private Document document;

		/// <summary>
		/// Sets up the test and creates the initial line buffer.
		/// </summary>
		[SetUp]
		public void Setup()
		{
			// Create a standard large document layout.
			document = new Document();

			var article = new Region(RegionType.Article, "A1");
			document.Matters.Add(article);
			article.Matters.Add(new Paragraph("P01"));
			article.Matters.Add(new Paragraph("P02"));
			article.Matters.Add(new Paragraph("P03"));

			var sect1 = new Region(RegionType.Section1, "1");
			article.Matters.Add(sect1);
			sect1.Matters.Add(new Paragraph("P04"));
			sect1.Matters.Add(new Paragraph("P05"));
			sect1.Matters.Add(new Paragraph("P06"));

			sect1 = new Region(RegionType.Section1, "2");
			article.Matters.Add(sect1);
			sect1.Matters.Add(new Paragraph("P07"));
			sect1.Matters.Add(new Paragraph("P08"));
			sect1.Matters.Add(new Paragraph("P09"));

			var sect2 = new Region(RegionType.Section2, "2.1");
			sect1.Matters.Add(sect2);
			sect2.Matters.Add(new Paragraph("P10"));
			sect2.Matters.Add(new Paragraph("P11"));
			sect2.Matters.Add(new Paragraph("P12"));

			var sect3 = new Region(RegionType.Section3, "2.1.1");
			sect2.Matters.Add(sect3);
			sect3.Matters.Add(new Paragraph("P13"));
			sect3.Matters.Add(new Paragraph("P14"));
			sect3.Matters.Add(new Paragraph("P15"));

			sect2 = new Region(RegionType.Section2, "2.2");
			sect1.Matters.Add(sect2);
			sect2.Matters.Add(new Paragraph("P16"));
			sect2.Matters.Add(new Paragraph("P17"));
			sect2.Matters.Add(new Paragraph("P18"));

			sect3 = new Region(RegionType.Section3, "2.2.1");
			sect2.Matters.Add(sect3);
			sect3.Matters.Add(new Paragraph("P19"));
			sect3.Matters.Add(new Paragraph("P20"));
			sect3.Matters.Add(new Paragraph("P21"));

			sect3 = new Region(RegionType.Section3, "2.2.2");
			sect2.Matters.Add(sect3);
			sect3.Matters.Add(new Paragraph("P22"));
			sect3.Matters.Add(new Paragraph("P23"));
			sect3.Matters.Add(new Paragraph("P24"));

			// Create the buffer from the document.
			buffer = new DocumentLineBuffer(document);
		}

		#endregion

		#region Tests

		#region Operation

		/// <summary>
		/// Tests that the initial document structure is correct.
		/// </summary>
		[Test]
		public void InitialDocumentStructure()
		{
			// Operation
			string thumbprint = document.GetThumbprint();

			// Verification
			Assert.AreEqual("Appp1ppp1ppp2ppp3ppp2ppp3ppp3ppp", thumbprint);
		}

		#endregion

		#region Delete Lines

		/// <summary>
		/// Deletes all three paragraphs in the article.
		/// </summary>
		[Test]
		public void DeleteArticleParagraphs()
		{
			// Operation
			buffer.DeleteLines(1, 3);

			// Verification
			string thumbprint = document.GetThumbprint();

			Assert.AreEqual("A1ppp1ppp2ppp3ppp2ppp3ppp3ppp", thumbprint);
		}

		/// <summary>
		/// Deletes the first paragraph in the article.
		/// </summary>
		[Test]
		public void DeleteFirstParagraph()
		{
			// Operation
			buffer.DeleteLines(1, 1);

			// Verification
			string thumbprint = document.GetThumbprint();

			Assert.AreEqual("App1ppp1ppp2ppp3ppp2ppp3ppp3ppp", thumbprint);
		}

		/// <summary>
		/// Deletes the first two article paragraphs.
		/// </summary>
		[Test]
		public void DeleteFirstTwoArticleParagraphs()
		{
			// Operation
			buffer.DeleteLines(1, 2);

			// Verification
			string thumbprint = document.GetThumbprint();

			Assert.AreEqual("Ap1ppp1ppp2ppp3ppp2ppp3ppp3ppp", thumbprint);
		}

		/// <summary>
		/// Deletes a paragraph, a child and the first of its paragraph.
		/// </summary>
		[Test]
		public void DeleteLeadingParagraphOnChild()
		{
			// Operation
			buffer.DeleteLines(23, 3);

			// Verification
			string thumbprint = document.GetThumbprint();

			Assert.AreEqual("Appp1ppp1ppp2ppp3ppp2pppp3ppp", thumbprint);
		}

		/// <summary>
		/// Deletes the second paragraph in the article.
		/// </summary>
		[Test]
		public void DeleteSecondParagraph()
		{
			// Operation
			buffer.DeleteLines(2, 1);

			// Verification
			string thumbprint = document.GetThumbprint();

			Assert.AreEqual("App1ppp1ppp2ppp3ppp2ppp3ppp3ppp", thumbprint);
		}

		/// <summary>
		/// Deletes the second two article paragraphs.
		/// </summary>
		[Test]
		public void DeleteSecondTwoArticleParagraphs()
		{
			// Operation
			buffer.DeleteLines(2, 2);

			// Verification
			string thumbprint = document.GetThumbprint();

			Assert.AreEqual("Ap1ppp1ppp2ppp3ppp2ppp3ppp3ppp", thumbprint);
		}

		/// <summary>
		/// Deletes the section and its first paragraph.
		/// </summary>
		[Test]
		public void DeleteSectionAndFirstParagraph()
		{
			// Operation
			buffer.DeleteLines(4, 2);

			// Verification
			string thumbprint = document.GetThumbprint();

			Assert.AreEqual("Appppp1ppp2ppp3ppp2ppp3ppp3ppp", thumbprint);
		}

		/// <summary>
		/// Deletes a section and all its paragraphs.
		/// </summary>
		[Test]
		public void DeleteSectionAndParagraphs()
		{
			// Operation
			buffer.DeleteLines(4, 4);

			// Verification
			string thumbprint = document.GetThumbprint();

			Assert.AreEqual("Appp1ppp2ppp3ppp2ppp3ppp3ppp", thumbprint);
		}

		/// <summary>
		/// Deletes a subsection which should promote the third level inside it.
		/// </summary>
		[Test]
		public void DeleteSubSection()
		{
			// Operation
			buffer.DeleteLines(12, 1);

			// Verification
			string thumbprint = document.GetThumbprint();

			Assert.AreEqual("Appp1ppp1pppppp3ppp2ppp3ppp3ppp", thumbprint);
		}

		/// <summary>
		/// Deletes the sub section with children next to another section with
		/// children, so the item should not be promoted.
		/// </summary>
		[Test]
		public void DeleteSubSectionWithChildren()
		{
			// Operation
			buffer.DeleteLines(20, 1);

			// Verification
			string thumbprint = document.GetThumbprint();

			Assert.AreEqual("Appp1ppp1ppp2ppp3pppppp3ppp3ppp", thumbprint);
		}

		/// <summary>
		/// Deletes the sub section with children next to another section with
		/// children, so the item should not be promoted.
		/// </summary>
		[Test]
		public void DeleteSubSectionWithChildren2()
		{
			// Operation
			buffer.DeleteLines(20, 2);

			// Verification
			string thumbprint = document.GetThumbprint();

			Assert.AreEqual("Appp1ppp1ppp2ppp3ppppp3ppp3ppp", thumbprint);
		}

		/// <summary>
		/// Deletes the third paragraph in the article.
		/// </summary>
		[Test]
		public void DeleteThirdParagraph()
		{
			// Operation
			buffer.DeleteLines(3, 1);

			// Verification
			string thumbprint = document.GetThumbprint();

			Assert.AreEqual("App1ppp1ppp2ppp3ppp2ppp3ppp3ppp", thumbprint);
		}

		/// <summary>
		/// Deletes a section and its paragraphs plus the next section, but
		/// leaving the second section's paragraphs in place.
		/// </summary>
		[Test]
		public void DeleteTwoSectionsAndFirstParagraphs()
		{
			// Operation
			buffer.DeleteLines(4, 5);

			// Verification
			string thumbprint = document.GetThumbprint();

			Assert.AreEqual("Apppppp2ppp3ppp2ppp3ppp3ppp", thumbprint);
		}

		#endregion

		#endregion
	}
}