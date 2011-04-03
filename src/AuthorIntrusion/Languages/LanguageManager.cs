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

using AuthorIntrusion.Contracts.Enumerations;
using AuthorIntrusion.Contracts.Events;
using AuthorIntrusion.Contracts.Languages;
using AuthorIntrusion.Contracts.Matters;

using MfGames.Reporting;

#endregion

namespace AuthorIntrusion.Languages
{
	/// <summary>
	/// Defines the basic language manager which handles parsing and processing
	/// of the various structural elements.
	/// </summary>
	public class LanguageManager : ILanguageManager
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="LanguageManager"/> class.
		/// </summary>
		/// <param name="contentParsers">The content parsers.</param>
		public LanguageManager(
			IContentParser[] contentParsers)
		{
			// Save the various lists in member variables.
			this.contentParsers = contentParsers;

			// Report the wiring in the logger.
			log = new Logger("LanguageManager");
			log.Info("ContentParsers: {0:N0}", contentParsers.Length);
		}

		#endregion

		#region Events

		/// <summary>
		/// Fires the parse progress event.
		/// </summary>
		/// <param name="args">The <see cref="AuthorIntrusion.Contracts.Events.ParseProgressEventArgs"/> instance containing the event data.</param>
		private void FireParseProgress(ParseProgressEventArgs args)
		{
			if (ParseProgress != null)
			{
				ParseProgress(this, args);
			}
		}

		/// <summary>
		/// Occurs when the parsing progresses forward. Used for showing process
		/// dialogs.
		/// </summary>
		public event EventHandler<ParseProgressEventArgs> ParseProgress;

		#endregion

		#region Parsing

		private readonly IContentParser[] contentParsers;
		private readonly Logger log;

		/// <summary>
		/// Parses a single paragraph before returning.
		/// </summary>
		/// <param name="state">The state.</param>
		private void DoParseParagraph(object state)
		{
			// Get the paragraph we are dealing with.
			var paragraph = (Paragraph) state;

			// Create a list of parsers we currently have in this manager
			// and strip out the ones that don't apply to the language.
			var parsers = new List<IContentParser>();
			parsers.AddRange(contentParsers);

			while (true)
			{
				// Keep track of the parsers that need to be removed from the
				// list. We also keep track if we have at least one successful
				// parse since that will let us try the deferred parsers again.
				var removedParsers = new List<IContentParser>();
				bool hadSuccessfulParse = false;

				// Go through all the parsers on the list.
				foreach (IContentParser parser in parsers)
				{
					// Attempt to parse the contents with this one.
					ProcessStatus results = parser.Parse(paragraph.Contents);

					switch (results)
					{
						case ProcessStatus.Succeeded:
							// Mark that we are successful to loop again and
							// add the parser to the remove list.
							hadSuccessfulParse = true;
							removedParsers.Add(parser);
							break;

						case ProcessStatus.Failed:
							// Add the parser to the remove list so we don't
							// try it again.
							removedParsers.Add(parser);
							break;
					}
				}

				// Remove any parsers on the remove list.
				foreach (IContentParser parser in removedParsers)
				{
					parsers.Remove(parser);
				}

				// If we don't have any parsers left or if we didn't have
				// at least one successful one, we break out of the loop.
				if (!hadSuccessfulParse || parsers.Count == 0)
				{
					break;
				}
			}
		}

		/// <summary>
		/// Parses the contents of the given structure.
		/// </summary>
		/// <param name="structure">The structure.</param>
		public void Parse(Matter structure)
		{
			//// Check the inputs.
			//if (structure == null)
			//{
			//    throw new ArgumentNullException("structure");
			//}

			//// Get a list of all the paragraphs we'll be parsing. We'll use that
			//// to build up the worker threads for parsing.
			//IList<Paragraph> paragraphs = structure.ParagraphList;
			//int paragraphCount = paragraphs.Count;
			//int paragraphsProcessed = 0;

			//// Create a background worker thread to process each paragraph in
			//// turn. This allows for multi-core machines to process more efficiently
			//// and to get the results faster.
			//foreach (Paragraph paragraph in paragraphs)
			//{
			//    //ThreadPool.QueueUserWorkItem(DoParseParagraph, paragraph);
			//    DoParseParagraph(paragraph);

			//    // We are done processing this paragraph.
			//    paragraphsProcessed++;
			//    FireParseProgress(
			//        new ParseProgressEventArgs(paragraphsProcessed, paragraphCount));
			//}
			throw new NotImplementedException();
		}

		#endregion
	}
}