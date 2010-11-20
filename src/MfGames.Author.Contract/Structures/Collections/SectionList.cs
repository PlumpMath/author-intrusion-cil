namespace MfGames.Author.Contract.Structures.Collections
{
	/// <summary>
	/// Implements a specialized list that handles parent mapping as part of
	/// the collection operations.
	/// </summary>
	public class SectionList : StructureBaseList<Section>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="SectionList"/> class.
		/// </summary>
		/// <param name="parent">The parent.</param>
		public SectionList(StructureBase parent)
			: base(parent)
		{
		}

		#endregion
	}
}