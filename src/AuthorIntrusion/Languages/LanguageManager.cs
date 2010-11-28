#region Namespaces

using System;
using System.Collections.Generic;

using AuthorIntrusion.Contracts.Enumerations;
using AuthorIntrusion.Contracts.Events;
using AuthorIntrusion.Contracts.Interfaces;
using AuthorIntrusion.Contracts.Languages;
using AuthorIntrusion.Contracts.Structures;

using MfGames.Logging;

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
		public LanguageManager(ILogger logger, IContentParser[] contentParsers)
		{
			// Save the various lists in member variables.
			this.contentParsers = contentParsers;

			// Report the wiring in the logger.
			log = new Log("LanguageManager", logger);
			log.Info("ContentParsers: {0:N0}", contentParsers.Length);
		}

		#endregion

		#region Events

		public event EventHandler<ParseProgressEventArgs> ParseProgress;

		private void FireParseProgress(ParseProgressEventArgs args)
		{
			if (ParseProgress != null)
			{
				ParseProgress(this, args);
			}
		}

		#endregion

		#region Parsing

		private readonly Log log;
		private readonly IContentParser[] contentParsers;

		/// <summary>
		/// Parses the contents of the given structure.
		/// </summary>
		/// <param name="structure">The structure.</param>
		public void Parse(Structure structure)
		{
			// Check the inputs.
			if (structure == null)
			{
				throw new ArgumentNullException("structure");
			}

			// Keep track of the paragraphs we'll be parsing so we can give a good progress
			// status.
			int paragraphCount = structure.ContentContainerStructureCount;
			int paragraphProcessed = 0;

			// Process the structure and paragraphs.
			Parse(structure, paragraphCount, ref paragraphProcessed);
		}

		private void Parse(Structure structure, int paragraphCount, ref int paragraphsProcessed)
		{
			// If we have content, we need to parse those first.
			if (structure is IContentContainer)
			{
				// Create a list of parsers we currently have in this manager
				// and strip out the ones that don't apply to the language.
				var parsers = new List<IContentParser>();
				parsers.AddRange(contentParsers);

				var contentContainer = (IContentContainer) structure;

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
						ParserStatus results = parser.Parse(contentContainer.Contents);

						switch (results)
						{
							case ParserStatus.Succeeded:
								// Mark that we are successful to loop again and
								// add the parser to the remove list.
								hadSuccessfulParse = true;
								removedParsers.Add(parser);
								break;

							case ParserStatus.Failed:
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

				// We are done processing this paragraph.
				paragraphsProcessed++;
				FireParseProgress(new ParseProgressEventArgs(paragraphsProcessed, paragraphCount));
			}

			// For structure containers, pass the parsing into the child
			// structures.
			if (structure is IStructureContainer)
			{
				var structureContainer = (IStructureContainer) structure;

				foreach (Structure childStructure in structureContainer.Structures)
				{
					Parse(childStructure, paragraphCount, ref paragraphsProcessed);
				}
			}
		}

		#endregion
	}
}