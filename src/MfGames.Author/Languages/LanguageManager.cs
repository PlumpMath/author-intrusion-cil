#region Namespaces

using System;

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
		/// <param name="paragraphParser">The paragraph parser.</param>
		public LanguageManager(IParagraphParser paragraphParser)
		{
			this.paragraphParser = paragraphParser;
		}

		#endregion

		#region Parsing

		private readonly IParagraphParser paragraphParser;

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

			// If we are a section/paragraph container, simplify processing.
			if (structure is SectionParagraphContainerBase)
			{
				SectionParagraphContainerBase container = (SectionParagraphContainerBase) structure;

				foreach (ContentContainerStructure paragraph in container.Paragraphs)
				{
					Parse(paragraph);
				}

				foreach (Section section in container.Sections)
				{
					Parse(section);
				}

				return;
			}

			// For the remaining items, figure out what to do.
			switch (structure.GetType().Name)
			{
				case "Book":
					foreach (Chapter chapter in ((Book) structure).Chapters)
					{
						Parse(chapter);
					}

					break;

				case "Paragraph":
					ContentContainerStructure paragraph = (ContentContainerStructure) structure;

					if (!paragraph.IsParsed)
					{
						paragraphParser.Parse(paragraph);
						paragraph.UnparsedContents.Clear();
					}

					break;
			}
		}

		#endregion
	}
}