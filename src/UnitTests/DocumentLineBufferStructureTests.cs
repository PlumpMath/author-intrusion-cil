using System.Text;

using AuthorIntrusion.Contracts;
using AuthorIntrusion.Contracts.Matters;
using AuthorIntrusion.Contracts.Structures;

using AuthorIntrusionGtk.Editors;

using NUnit.Framework;

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

		private DocumentLineBuffer largeBuffer;
		private Document largeDocument;
		private Document smallDocument;
		private DocumentLineBuffer smallBuffer;

		/// <summary>
		/// Sets up the test and creates the initial line buffer.
		/// </summary>
		[SetUp]
		public void Setup()
		{
			// Create a standard large document layout.
			var article = new Region(MatterType.Article, "A1");
			article.Structures.Add(new Paragraph("P01"));
			article.Structures.Add(new Paragraph("P02"));
			article.Structures.Add(new Paragraph("P03"));

			var sect1 = new Region(MatterType.Section, "1");
			sect1.Structures.Add(new Paragraph("P04"));
			sect1.Structures.Add(new Paragraph("P05"));
			sect1.Structures.Add(new Paragraph("P06"));
			article.Structures.Add(sect1);

			sect1 = new Region(MatterType.Section, "2");
			sect1.Structures.Add(new Paragraph("P07"));
			sect1.Structures.Add(new Paragraph("P08"));
			sect1.Structures.Add(new Paragraph("P09"));
			article.Structures.Add(sect1);

			var sect2 = new Region(MatterType.SubSection, "2.1");
			sect2.Structures.Add(new Paragraph("P10"));
			sect2.Structures.Add(new Paragraph("P11"));
			sect2.Structures.Add(new Paragraph("P12"));
			sect1.Structures.Add(sect2);

			var sect3 = new Region(MatterType.SubSubSection, "2.1.1");
			sect3.Structures.Add(new Paragraph("P13"));
			sect3.Structures.Add(new Paragraph("P14"));
			sect3.Structures.Add(new Paragraph("P15"));
			sect2.Structures.Add(sect3);

			sect2 = new Region(MatterType.SubSection, "2.2");
			sect2.Structures.Add(new Paragraph("P16"));
			sect2.Structures.Add(new Paragraph("P17"));
			sect2.Structures.Add(new Paragraph("P18"));
			sect1.Structures.Add(sect2);

			sect3 = new Region(MatterType.SubSubSection, "2.2.1");
			sect3.Structures.Add(new Paragraph("P19"));
			sect3.Structures.Add(new Paragraph("P20"));
			sect3.Structures.Add(new Paragraph("P21"));
			sect2.Structures.Add(sect3);

			sect3 = new Region(MatterType.SubSubSection, "2.2.2");
			sect3.Structures.Add(new Paragraph("P22"));
			sect3.Structures.Add(new Paragraph("P23"));
			sect3.Structures.Add(new Paragraph("P24"));
			sect2.Structures.Add(sect3);

			largeDocument = new Document();
			largeDocument.Structure = article;

			// Set up the standard small document.
			article = new Region(MatterType.Article, "A1");
			article.Structures.Add(new Paragraph("P01"));
			article.Structures.Add(new Paragraph("P02"));
			article.Structures.Add(new Paragraph("P03"));

			sect1 = new Region(MatterType.Section, "1");
			sect1.Structures.Add(new Paragraph("P04"));
			sect1.Structures.Add(new Paragraph("P05"));
			sect1.Structures.Add(new Paragraph("P06"));
			article.Structures.Add(sect1);

			sect1 = new Region(MatterType.Section, "2");
			sect1.Structures.Add(new Paragraph("P07"));
			sect1.Structures.Add(new Paragraph("P08"));
			sect1.Structures.Add(new Paragraph("P09"));
			article.Structures.Add(sect1);

			smallDocument = new Document();
			smallDocument.Structure = article;

			// Create the buffer from the document.
			largeBuffer = new DocumentLineBuffer(largeDocument);
			smallBuffer = new DocumentLineBuffer(smallDocument);
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
			string thumbprint = GetLargeThumbprint();

			// Verification
			Assert.AreEqual("Appp1ppp1ppp2ppp3ppp2ppp3ppp3ppp", thumbprint);
		}

		#endregion

		#region Delete Lines

		/// <summary>
		/// Deletes the first paragraph in the article.
		/// </summary>
		[Test] public void DeleteFirstParagraph()
		{
			// Operation
			largeBuffer.DeleteLines(1, 1);

			// Verification
			string thumbprint = GetLargeThumbprint();

			Assert.AreEqual("App1ppp1ppp2ppp3ppp2ppp3ppp3ppp", thumbprint);
		}

		/// <summary>
		/// Deletes the second paragraph in the article.
		/// </summary>
		[Test] public void DeleteSecondParagraph()
		{
			// Operation
			largeBuffer.DeleteLines(2, 1);

			// Verification
			string thumbprint = GetLargeThumbprint();

			Assert.AreEqual("App1ppp1ppp2ppp3ppp2ppp3ppp3ppp", thumbprint);
		}

		/// <summary>
		/// Deletes the third paragraph in the article.
		/// </summary>
		[Test] public void DeleteThirdParagraph()
		{
			// Operation
			largeBuffer.DeleteLines(3, 1);

			// Verification
			string thumbprint = GetLargeThumbprint();

			Assert.AreEqual("App1ppp1ppp2ppp3ppp2ppp3ppp3ppp", thumbprint);
		}

		/// <summary>
		/// Deletes all three paragraphs in the article.
		/// </summary>
		[Test] public void DeleteArticleParagraphs()
		{
			// Operation
			largeBuffer.DeleteLines(1, 3);

			// Verification
			string thumbprint = GetLargeThumbprint();

			Assert.AreEqual("A1ppp1ppp2ppp3ppp2ppp3ppp3ppp", thumbprint);
		}

		/// <summary>
		/// Deletes the first two article paragraphs.
		/// </summary>
		[Test] public void DeleteFirstTwoArticleParagraphs()
		{
			// Operation
			largeBuffer.DeleteLines(1, 2);

			// Verification
			string thumbprint = GetLargeThumbprint();

			Assert.AreEqual("Ap1ppp1ppp2ppp3ppp2ppp3ppp3ppp", thumbprint);
		}

		/// <summary>
		/// Deletes the second two article paragraphs.
		/// </summary>
		[Test] public void DeleteSecondTwoArticleParagraphs()
		{
			// Operation
			largeBuffer.DeleteLines(2, 2);

			// Verification
			string thumbprint = GetLargeThumbprint();

			Assert.AreEqual("Ap1ppp1ppp2ppp3ppp2ppp3ppp3ppp", thumbprint);
		}

		/// <summary>
		/// Deletes a section and all its paragraphs.
		/// </summary>
		[Test] public void DeleteSectionAndParagraphs()
		{
			// Operation
			largeBuffer.DeleteLines(4, 4);

			// Verification
			string thumbprint = GetLargeThumbprint();

			Assert.AreEqual("Appp1ppp2ppp3ppp2ppp3ppp3ppp", thumbprint);
		}

		/// <summary>
		/// Deletes the section and its first paragraph.
		/// </summary>
		[Test] public void DeleteSectionAndFirstParagraph()
		{
			// Operation
			largeBuffer.DeleteLines(4, 2);

			// Verification
			string thumbprint = GetLargeThumbprint();

			Assert.AreEqual("Appppp1ppp2ppp3ppp2ppp3ppp3ppp", thumbprint);
		}

		/// <summary>
		/// Deletes a section and its paragraphs plus the next section, but
		/// leaving the second section's paragraphs in place.
		/// </summary>
		[Test] public void DeleteTwoSectionsAndFirstParagraphs()
		{
			// Operation
			largeBuffer.DeleteLines(4, 5);

			// Verification
			string thumbprint = GetLargeThumbprint();

			Assert.AreEqual("Apppppp2ppp3ppp2ppp3ppp3ppp", thumbprint);
		}

		/// <summary>
		/// Deletes a subsection which should promote the third level inside it.
		/// </summary>
		[Test] public void DeleteSubSection()
		{
			// Operation
			largeBuffer.DeleteLines(12, 1);

			// Verification
			string thumbprint = GetLargeThumbprint();

			Assert.AreEqual("Appp1ppp1pppppp2ppp2ppp3ppp3ppp", thumbprint);
		}

		/// <summary>
		/// Deletes the sub section with children next to another section with
		/// children, so the item should not be promoted.
		/// </summary>
		[Test] public void DeleteSubSectionWithChildren()
		{
			// Operation
			largeBuffer.DeleteLines(20, 1);

			// Verification
			string thumbprint = GetLargeThumbprint();

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
			largeBuffer.DeleteLines(20, 2);

			// Verification
			string thumbprint = GetLargeThumbprint();

			Assert.AreEqual("Appp1ppp1ppp2ppp3ppppp3ppp3ppp", thumbprint);
		}

		/// <summary>
		/// Deletes the sub section with children next to another section with
		/// children, so the item should not be promoted.
		/// </summary>
		[Test]
		public void DeleteSubSectionWithChildren2Small()
		{
			// Operation
			smallBuffer.DeleteLines(4, 2);

			// Verification
			string thumbprint = GetSmallThumbprint();

			Assert.AreEqual("Appp1ppp1ppp", thumbprint);
		}

		/// <summary>
		/// Deletes a paragraph, a child and the first of its paragraph.
		/// </summary>
		[Test] public void DeleteLeadingParagraphOnChild()
		{
			// Operation
			largeBuffer.DeleteLines(23, 3);

			// Verification
			string thumbprint = GetLargeThumbprint();

			Assert.AreEqual("Appp1ppp1ppp2ppp3ppp2pppp3ppp", thumbprint);
		}

		#endregion

		#endregion

		#region Utilities

		/// <summary>
		/// Creates a thumbprint of the buffer by creating a string that has
		/// the first initial of most of the structure elements. This gives a
		/// general layout of the entire document and will make it easier to
		/// check for style movement with line operations. For example,
		/// a document with a chapter and three paragraphs would be "Cppp".
		/// 
		/// Sections, SubSections, and SubSubSections are converted into 1, 2,
		/// 3, respectively.
		/// </summary>
		/// <returns></returns>
		private string GetLargeThumbprint()
		{
			var builder = new StringBuilder();

			GetStructureThumbprint(builder, largeBuffer.Document.Structure);

			return builder.ToString();
		}

		private string GetSmallThumbprint()
		{
			var builder = new StringBuilder();

			GetStructureThumbprint(builder, smallBuffer.Document.Structure);

			return builder.ToString();
		}

		/// <summary>
		/// Gets the thumbprint of the structure then goes through the structure's
		/// child structures.
		/// </summary>
		/// <param name="builder">The builder.</param>
		/// <param name="structure">The structure.</param>
		private static void GetStructureThumbprint(StringBuilder builder, Matter structure)
		{
			// If we are null, then just return.
			if (structure == null)
			{
				return;
			}

			// Add the structure's thumbprint to the builder.
			string thumbprint = structure.MatterType.ToString()[0].ToString();

			switch (structure.MatterType)
			{
				case MatterType.Paragraph:
					// We want paragraphs to be lowercase P.
					thumbprint = "p";
					break;

				case MatterType.Section:
					// We convert sections into numerical levels.
					thumbprint = "1";
					break;

				case MatterType.SubSection:
					// We convert sections into numerical levels.
					thumbprint = "2";
					break;

				case MatterType.SubSubSection:
					// We convert sections into numerical levels.
					thumbprint = "3";
					break;
			}

			builder.Append(thumbprint);

			// If we are a section object, then recursively go into it.
			if (structure is Region)
			{
				var section = (Region) structure;

				foreach (Matter child in section.Structures)
				{
					GetStructureThumbprint(builder, child);
				}
			}
		}

		#endregion
	}
}