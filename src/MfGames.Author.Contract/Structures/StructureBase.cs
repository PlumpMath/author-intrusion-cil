namespace MfGames.Author.Contract.Structures
{
	/// <summary>
	/// The common root for all the structural elements.
	/// </summary>
	public abstract class StructureBase
	{
		#region Structural Relationships

		/// <summary>
		/// Gets or sets the parent structure element.
		/// </summary>
		/// <value>The parent.</value>
		public StructureBase Parent { get; set; }

		#endregion
	}
}