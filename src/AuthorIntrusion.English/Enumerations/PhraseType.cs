#region Namespaces

using System.ComponentModel;

using AuthorIntrusion.English.Attributes;

#endregion

namespace AuthorIntrusion.English.Enumerations
{
	/// <summary>
	/// Defines the various types of English phrases.
	///
	/// http://bulba.sdsu.edu/jeanette/thesis/PennTags.html
	/// </summary>
	public enum PhraseType
	{
		/// <summary>
		/// 
		/// </summary>
		[Description("Adjective Phrase")]
		[TreebankCode("ADJP")]
		AdjectivePhrase,

		/// <summary>
		/// 
		/// </summary>
		[Description("Prepositional Phrase")]
		[TreebankCode("PP")]
		PrepositionalPhrase,

		/// <summary>
		/// 
		/// </summary>
		[Description("Adverb Phrase")]
		[TreebankCode("ADVP")]
		AdverbPhrase,

		/// <summary>
		/// 
		/// </summary>
		[Description("Particle")]
		[TreebankCode("PRT")]
		Particle,

		/// <summary>
		/// 
		/// </summary>
		[Description("Conjunction Phrase")]
		[TreebankCode("CONJP")]
		ConjunctionPhrase,

		/// <summary>
		/// 
		/// </summary>
		[Description("Subordinating Conjunction Clause")]
		[TreebankCode("SBAR")]
		SubordinatingConjunctionClause,

		/// <summary>
		/// 
		/// </summary>
		[Description("Interjection")]
		[TreebankCode("INTJ")]
		Interjection,

		/// <summary>
		/// 
		/// </summary>
		[Description("Unlike Coordinated Phrase")]
		[TreebankCode("UCP")]
		UnlikeCoordinatedPhrase,

		/// <summary>
		/// 
		/// </summary>
		[Description("List Marker")]
		[TreebankCode("LST")]
		ListMarker,

		/// <summary>
		/// 
		/// </summary>
		[Description("Verb Phrase")]
		[TreebankCode("VP")]
		VerbPhrase,

		/// <summary>
		/// 
		/// </summary>
		[Description("Noun Phrase")]
		[TreebankCode("NP")]
		NounPhrase,

		[Description("WH-Noun Phrase")]
		[TreebankCode("WHNP")]
		WhNounPhrase,

		[Description("WH-Adverb Phrase")]
		[TreebankCode("WHADVP")]
		WhAdverbPhrase,

		[Description("Simple Declarative Phrase")]
		[TreebankCode("S")]
		SimpleDeclarativePhrase,

		[Description("Quantified Phrase")]
		[TreebankCode("QP")]
		QuantifiedPhrase,

		[Description("Parenthetical")]
		[TreebankCode("PRN")]
		Parenthetical,

		[Description("Inverted Question")]
		[TreebankCode("SQ")]
		InvertedQuestion,

		[Description("Wh-Adjective Phrase")]
		[TreebankCode("WHADJP")]
		WhAdjectivePhrase,
	}
}