#region Namespaces

using AuthorIntrusion.Contracts.Interfaces;
using AuthorIntrusion.English.Enumerations;

#endregion

namespace AuthorIntrusion.English.Tags
{
	/// <summary>
	/// Implements a tag that defines an English phrase.
	/// </summary>
	public class EnglishPhraseTag : IElementTag
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="EnglishPhraseTag"/> class.
		/// </summary>
		/// <param name="phraseType">Type of the phrase.</param>
		public EnglishPhraseTag(PhraseType phraseType)
		{
			this.phraseType = phraseType;
		}

		#endregion

		#region English

		private readonly PhraseType phraseType;

		/// <summary>
		/// Gets the type of the phrase.
		/// </summary>
		/// <value>The type of the phrase.</value>
		public PhraseType PhraseType
		{
			get { return phraseType; }
		}

		#endregion
	}
}