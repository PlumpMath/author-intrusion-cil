namespace MfGames.Author.Contract.Structures
{
	/// <summary>
	/// The common root for all the structural elements.
	/// </summary>
	public abstract class Structure
	{
		#region Structural Relationships

		/// <summary>
		/// Gets or sets the parent structure element.
		/// </summary>
		/// <value>The parent.</value>
		public Structure Parent { get; set; }

		#endregion
	}
}