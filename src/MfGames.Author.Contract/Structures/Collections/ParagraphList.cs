namespace MfGames.Author.Contract.Structures.Collections
{
	/// <summary>
	/// Implements an ordered collection of paragraphs.
	/// </summary>
	public class ParagraphList : StructureBaseList<Paragraph>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="ParagraphList"/> class.
		/// </summary>
		/// <param name="parent">The parent.</param>
		public ParagraphList(StructureBase parent)
			: base(parent)
		{
		}

		#endregion
	}
}