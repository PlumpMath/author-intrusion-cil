#region Namespaces

using System;
using System.Collections.Generic;
using System.IO;

using AuthorIntrusion.Contracts.Collections;
using AuthorIntrusion.Contracts.Contents;
using AuthorIntrusion.Contracts.Enumerations;
using AuthorIntrusion.Contracts.Interfaces;
using AuthorIntrusion.Contracts.Languages;
using AuthorIntrusion.English;
using AuthorIntrusion.English.Enumerations;
using AuthorIntrusion.English.Tags;

using MfGames.Extensions.IO;
using MfGames.Logging;

using OpenNLP.Tools.Parser;
using OpenNLP.Tools.SentenceDetect;

#endregion

namespace AuthorIntrusion.EnglishSharpNlp
{
	/// <summary>
	/// Defines a content parser that used SharpNlp to break apart the
	/// sentence and create the parsing tree for the contents.
	/// </summary>
	public class EnglishSharpNlpParser : EnglishSpecificBase, IContentParser
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="EnglishSharpNlpParser"/> class.
		/// </summary>
		/// <param name="logger">The logger.</param>
		public EnglishSharpNlpParser(ILogger logger)
		{
			this.logger = logger;
			modelDirectory = new DirectoryInfo("models");
		}

		#endregion

		#region SharpNLP

		private ILogger logger;
		private DirectoryInfo modelDirectory;
		private ISentenceDetector sentenceDetector;
		private EnglishTreebankParser englishTreebankParser;

		/// <summary>
		/// Lazy loads the english treebank parser.
		/// </summary>
		/// <value>The english treebank parser.</value>
		private EnglishTreebankParser EnglishTreebankParser
		{
			get
			{
				if (englishTreebankParser == null)
				{
					logger.Info(this, "Creating English treebank parser");
					englishTreebankParser = new EnglishTreebankParser(modelDirectory.FullName);
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
					logger.Info(this, "Creating sentence detector from EnglishSD.nbin");
					sentenceDetector =
						new EnglishMaximumEntropySentenceDetector(
							modelDirectory.GetDirectoryInfo("EnglishSD.nbin").FullName);
				}

				return sentenceDetector;
			}
		}

		#endregion

		#region Parsing

		/// <summary>
		/// Parses the content of the content container and replaces the contents
		/// with parsed data.
		/// </summary>
		/// <param name="contents">The content container.</param>
		/// <returns>The status result from the parse.</returns>
		public ParserStatus Parse(ContentList contents)
		{
			// The SharpNlp works off a string, so we just getting the content
			// string from the contents and we'll parse that. Once we're done,
			// we'll merge the results back in to retain the additional encoding.
			string contentString = contents.ContentString;
			string[] sentenceStrings = SentenceDetector.SentenceDetect(contentString);

			// Loop through and create a sentence object for each one.
			List<Sentence> sentences = new List<Sentence>();

			foreach (string sentenceString in sentenceStrings)
			{
				// Create the sentence for this object.
				Sentence sentence = new Sentence();
				sentences.Add(sentence);

				// Parse the sentence using the English treebank parser.
				Parse parse = EnglishTreebankParser.DoParse(sentenceString);

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

			// Merge/replace the contents of the content list with the new
			// contents.
			contents.Clear();

			foreach (Sentence sentence in sentences)
			{
				contents.Add(sentence);
			}

			// Return a successful parse.
			return ParserStatus.Succeeded;
		}

		/// <summary>
		/// Recursively creates the contents by processing through the parse
		/// object and creating the content tree to it.
		/// </summary>
		/// <param name="contents">The contents.</param>
		/// <param name="parent">The parse.</param>
		private void CreateContents(IContentContainer contents, Parse parent)
		{
			// Go through the children of this parent object.
			foreach (var child in parent.GetChildren())
			{
				// Figure out what is this component.
				bool isPartOfSpeech = PartsOfSpeechUtility.IsPartOfSpeech(child.Type);
				bool isPhrase = PartsOfSpeechUtility.IsPhraseType(child.Type);

				if (!isPartOfSpeech && !isPhrase)
				{
					throw new Exception("Cannot parse type: " + child.Type);
				}

				if (isPhrase)
				{
					// Create a phrase object and assign it a phrase tag.
					Phrase phrase = new Phrase();
					contents.Contents.Add(phrase);

					// Tag the phrase with the type.
					PhraseType phraseType = PartsOfSpeechUtility.GetPhraseType(child.Type);
					phrase.Tags.Add(new EnglishPhraseTag(phraseType));

					// Recurse into the phrase.
					CreateContents(phrase, child);
				}
				else
				{
					// Create a word to represent this item.
					Word word = new Word(child.Text);
					contents.Contents.Add(word);

					// Tag the word with the part of speech.
					PartOfSpeech partOfSpeech =
						PartsOfSpeechUtility.GetPartOfSpeech(child.Type);
					word.Tags.Add(new EnglishPartOfSpeechTag(partOfSpeech));
				}
			}
		}

		#endregion
	}
}