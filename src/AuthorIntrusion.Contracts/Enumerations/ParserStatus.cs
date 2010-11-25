namespace AuthorIntrusion.Contracts.Enumerations
{
	/// <summary>
	/// Indicates the results of a parse attempt.
	/// </summary>
	public enum ParserStatus
	{
		/// <summary>
		/// Indicates that the parse has succeeded.
		/// </summary>
		Succeeded,

		/// <summary>
		/// Indicates that the parse failed and not to try again.
		/// </summary>
		Failed,

		/// <summary>
		/// Indicates that the parse could not complete but to try again if
		/// something changed, such as another parser successfully running.
		/// </summary>
		Deferred,
	}
}