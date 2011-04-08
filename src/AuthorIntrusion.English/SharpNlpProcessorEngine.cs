#region Copyright and License

// Copyright (c) 2005-2011, Moonfire Games
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

using AuthorIntrusion.Contracts.Collections;
using AuthorIntrusion.Contracts.Contents;
using AuthorIntrusion.Contracts.Enumerations;
using AuthorIntrusion.Contracts.Interfaces;
using AuthorIntrusion.Contracts.Languages;
using AuthorIntrusion.Contracts.Matters;
using AuthorIntrusion.Contracts.Processors;
using AuthorIntrusion.English.Enumerations;
using AuthorIntrusion.English.Tags;

using MfGames.Extensions.System.IO;
using MfGames.Locking;
using MfGames.Reporting;

using OpenNLP.Tools.Parser;
using OpenNLP.Tools.SentenceDetect;
using OpenNLP.Tools.Util;

#endregion

namespace AuthorIntrusion.English
{
	/// <summary>
	/// Defines a content parser that used SharpNlp to break apart the
	/// sentence and create the parsing tree for the contents.
	/// </summary>
	public class SharpNlpProcessorEngine : EnglishSpecificBase, IProcessorEngine
	{
		#region Fields

		private readonly Logger log;
		private readonly DirectoryInfo modelDirectory;
		private EnglishTreebankParser englishTreebankParser;
		private ISentenceDetector sentenceDetector;
		private readonly ReaderWriterLockSlim processLock;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="SharpNlpProcessorEngine"/> class.
		/// </summary>
		public SharpNlpProcessorEngine()
		{
			modelDirectory = new DirectoryInfo("models");
			log = new Logger(this);
			processLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
		}

		#endregion

		#region SharpNLP

		/// <summary>
		/// Lazy loads the English treebank parser.
		/// </summary>
		/// <value>The english treebank parser.</value>
		private EnglishTreebankParser EnglishTreebankParser
		{
			get
			{
				if (englishTreebankParser == null)
				{
					log.Info("Creating English treebank parser");
					englishTreebankParser = new EnglishTreebankParser(
						modelDirectory.FullName, true, false);
				}

				return englishTreebankParser;
			}
		}

		/// <summary>
		/// Gets the SharpNLP sentence detector, loading it into memory if needed.
		/// </summary>
		/// <value>The sentence detector.</value>
		private ISentenceDetector SentenceDetector
		{
			get
			{
				if (sentenceDetector == null)
				{
					log.Info("Creating sentence detector from EnglishSD.nbin");
					sentenceDetector =
						new EnglishMaximumEntropySentenceDetector(
							modelDirectory.GetDirectoryInfo("EnglishSD.nbin").FullName);
				}

				return sentenceDetector;
			}
		}

		#endregion

		#region Processing

		/// <summary>
		/// Gets a value indicating whether this processor is a singleton, which
		/// means there is only one instance for a given processor or if the user
		/// can create multiple instances and customize each one.
		/// </summary>
		public bool IsSingleton
		{
			get { return true; }
		}

		/// <summary>
		/// Creates a new processor info associated with this processor. For
		/// singleton processes, this should require the same information object
		/// for every call, but for non-singleton, this will create a new
		/// object so allow the user to create multiple instances.
		/// </summary>
		/// <returns></returns>
		public Processor CreateProcessor()
		{
			return new SharpNlpProcessor(this);
		}

		#endregion

		#region Parsing

		/// <summary>
		/// Recursively creates the contents by processing through the parse
		/// object and creating the content tree to it.
		/// </summary>
		/// <param name="contents">The contents.</param>
		/// <param name="parent">The parse.</param>
		private void CreateContents(
			IContentContainer contents,
			Parse parent)
		{
			// Go through the children of this parent object.
			foreach (Parse child in parent.GetChildren())
			{
				// Create a token to represent this item.
				Span span = child.Span;
				string token = child.Text.Substring(span.Start, (span.End) - (span.Start));

				// Figure out the type of speech this is.
				var treebankTag = new EnglishTreebankTag(child.Type, GetType().ToString());
				EnglishTreebankClassification classification =
					EnglishTreebankUtility.GetClassification(child.Type);
				Content content;

				switch (classification)
				{
					case EnglishTreebankClassification.Phrase:
						// Create the phrase.
						var phrase = new Phrase();
						content = phrase;

						// Recurse into the phrase.
						CreateContents(phrase, child);
						break;

					case EnglishTreebankClassification.Puncuation:
						content = new Puncuation(token, false);
						break;

					case EnglishTreebankClassification.Terminating:
						content = new Puncuation(token, true);
						break;

					case EnglishTreebankClassification.Word:
						content = new Word(token);
						break;

					default:
						throw new Exception("Unknown classification type: " + classification);
				}

				// Add the content to the list.
				content.Tags.Add(treebankTag);
				contents.Contents.Add(content);
			}
		}

		/// <summary>
		/// Processes through a single paragraph and breaks it into the 
		/// various English phrases.
		/// </summary>
		/// <param name="context">The context.</param>
		public void Process(ProcessorContext context)
		{
			// At the moment, SharpNlp is not thread-safe.
			Paragraph paragraph = context.Paragraph;
			var sentences = new List<Sentence>();

			using (new WriteLock(processLock))
			{
				// The SharpNlp works off a string, so we just getting the content
				// string from the contents and we'll parse that. Once we're done,
				// we'll merge the results back in to retain the additional encoding.
				string contentString = paragraph.ContentString;
				string[] sentenceStrings = SentenceDetector.SentenceDetect(contentString);

				// Loop through and create a sentence object for each one.
				foreach (string sentenceString in sentenceStrings)
				{
					// Check to see if we are canceled.
					if (context.IsCanceled)
					{
						return;
					}

					// Create the sentence for this object.
					var sentence = new Sentence();
					sentences.Add(sentence);

					// Parse the sentence using the English treebank parser.
					Parse parse;

					try
					{
						parse = EnglishTreebankParser.DoParse(sentenceString);
					}
					catch (Exception exception)
					{
						// Can't parse this line, so report it and skip it.
						log.Error("Cannot parse: {0}: {1}", sentenceString, exception);
						continue;
					}

					// Check to see if we are canceled.
					if (context.IsCanceled)
					{
						return;
					}

					// Move into the top node.
					if (parse.Type == MaximumEntropyParser.TopNode)
					{
						// There is only one child in the top node.
						parse = parse.GetChildren()[0];
					}

					// Recursively add the various phrases into the sentence while
					// tagging them with the English parts of speech and phrase types.
					CreateContents(sentence, parse);
				}
			}

			// Check to see if we are canceled.
			if (context.IsCanceled)
			{
				return;
			}

			// Merge/replace the contents of the content list with the new
			// contents.
			paragraph.Contents.Clear();

			foreach (Sentence sentence in sentences)
			{
				paragraph.Contents.Add(sentence);
			}
		}

		#endregion
	}
}