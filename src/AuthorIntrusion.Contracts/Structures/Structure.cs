namespace AuthorIntrusion.Contracts.Structures
{
	/// <summary>
	/// The common root for all the structural elements.
	/// </summary>
	public abstract class Structure : Element
	{
		#region Structural Relationships

		/// <summary>
		/// Gets a count of content container content (i.e. paragraphs) in this
		/// object or child objects.
		/// </summary>
		public abstract int ContentContainerStructureCount { get; }

		/// <summary>
		/// Gets or sets the parent structure element.
		/// </summary>
		/// <value>The parent.</value>
		public Structure Parent { get; set; }

		#endregion
	}
}