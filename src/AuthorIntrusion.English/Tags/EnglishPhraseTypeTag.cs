#region Namespaces

using AuthorIntrusion.Contracts.Interfaces;
using AuthorIntrusion.English.Enumerations;

#endregion

namespace AuthorIntrusion.English.Tags
{
	/// <summary>
	/// Implements a tag that defines an English phrase.
	/// </summary>
	public class EnglishPhraseTypeTag : IElementTag
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="EnglishPhraseTag"/> class.
		/// </summary>
		/// <param name="phraseType">Type of the phrase.</param>
		public EnglishPhraseTypeTag(PhraseType phraseType)
		{
			this.partOfSpeech = phraseType;
		}

		#endregion

		#region English

		private readonly PhraseType partOfSpeech;

		/// <summary>
		/// Gets the type of the phrase.
		/// </summary>
		/// <value>The type of the phrase.</value>
		public PhraseType PhraseType
		{
			get { return partOfSpeech; }
		}

		#endregion

		#region Conversion

		public override string ToString()
		{
			return partOfSpeech.ToString();
		}

		#endregion
	}
}