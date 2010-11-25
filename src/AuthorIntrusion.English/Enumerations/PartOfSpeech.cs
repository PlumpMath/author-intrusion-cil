#region Namespaces

using System.ComponentModel;

using AuthorIntrusion.English.Attributes;

#endregion

namespace AuthorIntrusion.English.Enumerations
{
	/// <summary>
	/// Defines the English parts of speech.
	/// </summary>
	public enum PartOfSpeech
	{
		/// <summary>
		/// 
		/// </summary>
		[Description("Coordinating Conjunction")]
		[TreebankCode("CC")]
		CoordinatingConjunction,

		/// <summary>
		/// 
		/// </summary>
		[Description("Particle")]
		[TreebankCode("RP")]
		Particle,

		/// <summary>
		/// 
		/// </summary>
		[Description("Cardinal Number")]
		[TreebankCode("CD")]
		CardinalNumber,

		/// <summary>
		/// 
		/// </summary>
		[Description("Symbol")]
		[TreebankCode("SYM")]
		Symbol,

		/// <summary>
		/// 
		/// </summary>
		[Description("Determiner")]
		[TreebankCode("DT")]
		Determiner,

		/// <summary>
		/// 
		/// </summary>
		[Description("To")]
		[TreebankCode("TO")]
		To,

		/// <summary>
		/// 
		/// </summary>
		[Description("Existential There")]
		[TreebankCode("EX")]
		ExistentialThere,

		/// <summary>
		/// 
		/// </summary>
		[Description("Interjection")]
		[TreebankCode("UH")]
		Interjection,

		/// <summary>
		/// 
		/// </summary>
		[Description("Foreign word")]
		[TreebankCode("FW")]
		Foreignword,

		/// <summary>
		/// 
		/// </summary>
		[Description("Verb Base")]
		[TreebankCode("VB")]
		VerbBase,

		/// <summary>
		/// 
		/// </summary>
		[Description("Preposition Conjunction")]
		[TreebankCode("IN")]
		PrepositionConjunction,

		/// <summary>
		/// 
		/// </summary>
		[Description("Pase Tense Verb")]
		[TreebankCode("VBD")]
		PaseTenseVerb,

		/// <summary>
		/// 
		/// </summary>
		[Description("Adjective")]
		[TreebankCode("JJ")]
		Adjective,

		/// <summary>
		/// 
		/// </summary>
		[Description("Present Participle Verb")]
		[TreebankCode("VBG")]
		PresentParticipleVerb,

		/// <summary>
		/// 
		/// </summary>
		[Description("Comparative Adjective")]
		[TreebankCode("JJR")]
		ComparativeAdjective,

		/// <summary>
		/// 
		/// </summary>
		[Description("Past Participle Verb")]
		[TreebankCode("VBN")]
		PastParticipleVerb,

		/// <summary>
		/// 
		/// </summary>
		[Description("Superlative Adjective")]
		[TreebankCode("JJS")]
		SuperlativeAdjective,

		/// <summary>
		/// 
		/// </summary>
		[Description("Singular Present Verb")]
		[TreebankCode("VBP")]
		SingularPresentVerb,

		/// <summary>
		/// 
		/// </summary>
		[Description("List Item Marker")]
		[TreebankCode("LS")]
		ListItemMarker,

		/// <summary>
		/// 
		/// </summary>
		[Description("Singular Present 3rd Person Verb")]
		[TreebankCode("VBZ")]
		SingularPresent3rdPersonVerb,

		/// <summary>
		/// 
		/// </summary>
		[Description("Modal")]
		[TreebankCode("MD")]
		Modal,

		/// <summary>
		/// 
		/// </summary>
		[Description("WH-Determiner")]
		[TreebankCode("WDT")]
		WHDeterminer,

		/// <summary>
		/// 
		/// </summary>
		[Description("Noun")]
		[TreebankCode("NN")]
		Noun,

		/// <summary>
		/// 
		/// </summary>
		[Description("WH-Pronoun")]
		[TreebankCode("WP")]
		WHPronoun,

		/// <summary>
		/// 
		/// </summary>
		[Description("Singular Proper Noun")]
		[TreebankCode("NNP")]
		SingularProperNoun,

		/// <summary>
		/// 
		/// </summary>
		[Description("Possessive WH-Pronoun")]
		[TreebankCode("WP$")]
		PossessiveWHPronoun,

		/// <summary>
		/// 
		/// </summary>
		[Description("Plural Proper Noun")]
		[TreebankCode("NNPS")]
		PluralProperNoun,

		/// <summary>
		/// 
		/// </summary>
		[Description("WH-Adverb")]
		[TreebankCode("WRB")]
		WHAdverb,

		/// <summary>
		/// 
		/// </summary>
		[Description("Plural Noun")]
		[TreebankCode("NNS")]
		PluralNoun,

		/// <summary>
		/// 
		/// </summary>
		[Description("Open Double Quote")]
		[TreebankCode("``")]
		OpenDoubleQuote,

		/// <summary>
		/// 
		/// </summary>
		[Description("Predeterminer")]
		[TreebankCode("PDT")]
		Predeterminer,

		/// <summary>
		/// 
		/// </summary>
		[Description("Comma")]
		[TreebankCode(",")]
		Comma,

		/// <summary>
		/// 
		/// </summary>
		[Description("Possessive Ending")]
		[TreebankCode("POS")]
		PossessiveEnding,

		/// <summary>
		/// 
		/// </summary>
		[Description("Close Double Quote")]
		[TreebankCode("''")]
		CloseDoubleQuote,

		/// <summary>
		/// 
		/// </summary>
		[Description("Personal Pronoun")]
		[TreebankCode("PRP")]
		PersonalPronoun,

		/// <summary>
		/// 
		/// </summary>
		[Description("Sentence-Final Punctuation")]
		[TreebankCode(".")]
		SentenceFinalPunctuation,

		/// <summary>
		/// 
		/// </summary>
		[Description("Possessive Pronoun")]
		[TreebankCode("PRP$")]
		PossessivePronoun,

		/// <summary>
		/// 
		/// </summary>
		[Description("Colon")]
		[TreebankCode(":")]
		Colon,

		/// <summary>
		/// 
		/// </summary>
		[Description("Adverb")]
		[TreebankCode("RB")]
		Adverb,

		/// <summary>
		/// 
		/// </summary>
		[Description("Dollar sign")]
		[TreebankCode("$")]
		Dollarsign,

		/// <summary>
		/// 
		/// </summary>
		[Description("Comparative Adverb")]
		[TreebankCode("RBR")]
		ComparativeAdverb,

		/// <summary>
		/// 
		/// </summary>
		[Description("Pound sign")]
		[TreebankCode("#")]
		Poundsign,

		/// <summary>
		/// 
		/// </summary>
		[Description("Superlative Adverb")]
		[TreebankCode("RBS")]
		SuperlativeAdverb,

		/// <summary>
		/// 
		/// </summary>
		[Description("Left Parenthesis")]
		[TreebankCode("(")]
		LeftParenthesis,

		/// <summary>
		/// 
		/// </summary>
		[Description("Right Parenthesis")]
		[TreebankCode(")")]
		RightParenthesis,
	}
}