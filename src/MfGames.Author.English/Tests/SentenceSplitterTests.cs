#region Namespaces

using System;
using System.Collections.Generic;

using MfGames.Author.Contract.Contents;
using MfGames.Author.Contract.Contents.Collections;
using MfGames.Author.Contract.Structures;

using NUnit.Framework;

#endregion

namespace MfGames.Author.English
{
	/// <summary>
	/// Contains various sentence splitter tests.
	/// </summary>
	[TestFixture]
	public class SentenceSplitterTests
	{
		#region Tests
		
		[Test]
		public void SimpleSentence()
		{
			TestSingleSentence(
				new UnparsedString("This is a simple sentence."));
		}
		
		[Test]
		public void SentenceWithHonorific()
		{
			TestSingleSentence(
				new UnparsedString("I saw Mr. Smith."));
		}
		
		#endregion
		
		#region Utility
		
		/// <summary>
		/// Takes the list of content elements and splits the resulting sentences. Then
		/// the resulting sentence is verify that it is parsed correctly.
		/// </summary>
		/// <param name="contents">
		/// A <see cref="ContentBase[]"/>
		/// </param>
		private List<ContentList> TestSingleSentence(params ContentBase[] contents)
		{
			// Create the paragraph and add the sentence to the unparsed content.
			Paragraph paragraph = new Paragraph();
			paragraph.UnparsedContents.Add(new UnparsedString ("This is the first sentence."));
			
			foreach (ContentBase content in contents)
			{
				paragraph.UnparsedContents.Add(content);
			}
			
			paragraph.UnparsedContents.Add(new UnparsedString("This is the third sentence."));
			
			// Split the sentences out and compare the results.
			List<ContentList> sentences = SentenceSplitter.Split(paragraph.UnparsedContents);
			
			Assert.AreEqual(3, sentences.Count, "Could not parse isolate the sentence with the splitter.");
			
			// Return the resulting list so additional tests again.
			return sentences;
		}
		
		#endregion
	}
}
