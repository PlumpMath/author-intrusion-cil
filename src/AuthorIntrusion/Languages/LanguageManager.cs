#region Namespaces

using System;
using System.Collections.Generic;

using MfGames.Author.Contract.Enumerations;
using MfGames.Author.Contract.Interfaces;
using MfGames.Author.Contract.Languages;
using MfGames.Author.Contract.Structures;

#endregion

namespace MfGames.Author.Languages
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
		public LanguageManager(IContentParser[] contentParsers)
		{
			this.contentParsers = contentParsers;
		}

		#endregion

		#region Parsing

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

			// If we have content, we need to parse those first.
			if (structure is IContentContainer)
			{
				// Create a list of parsers we currently have in this manager
				// and strip out the ones that don't apply to the language.
				List<IContentParser> parsers = new List<IContentParser>();
				parsers.AddRange(contentParsers);

				var contentContainer = (IContentContainer) structure;

				while (true)
				{
					// Keep track of the parsers that need to be removed from the
					// list. We also keep track if we have at least one successful
					// parse since that will let us try the deferred parsers again.
					List<IContentParser> removedParsers = new List<IContentParser>();
					bool hadSuccessfulParse = false;

					// Go through all the parsers on the list.
					foreach (var parser in parsers)
					{
						// Attempt to parse the contents with this one.
						var results = parser.Parse(contentContainer.Contents);

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
					foreach (var parser in removedParsers)
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

			// For structure containers, pass the parsing into the child
			// structures.
			if (structure is IStructureContainer)
			{
				var structureContainer = (IStructureContainer) structure;
				
				foreach (var childStructure in structureContainer.Structures)
				{
					Parse(childStructure);
				}
			}
		}

		#endregion
	}
}