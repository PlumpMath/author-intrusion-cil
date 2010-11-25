namespace AuthorIntrusion.Contracts.Enumerations
{
	/// <summary>
	/// Defines the type of contents that the system uses.
	/// </summary>
	public enum ContentType
	{
		/// <summary>
		/// Indicates that the content is unparsed text.
		/// </summary>
		Unparsed,

		/// <summary>
		/// Indicates that the content is a single or effectively single
		/// word.
		/// </summary>
		Word,

		/// <summary>
		/// Indicates that the content represents non-sentence-terminating
		/// puncuation.
		/// </summary>
		Punctuation,

		/// <summary>
		/// Indicates that the content is sentence-terminating puncuation.
		/// </summary>
		Terminator,

		/// <summary>
		/// Indicates that the content is a quoted string.
		/// </summary>
		Quote,

		/// <summary>
		/// Indicates that the content is a sentence.
		/// </summary>
		Sentence,
	}
}