#region Namespaces

using AuthorIntrusion.Contracts.Collections;

#endregion

namespace AuthorIntrusion.Contracts.Interfaces
{
	/// <summary>
	/// Defines an item that can contain unparsed contents.
	/// </summary>
	public interface IContentContainer
	{
		#region Contents

		/// <summary>
		/// Contains an ordered list of contents within the container.
		/// </summary>
		/// <value>The contents.</value>
		ContentList Contents { get; }

		/// <summary>
		/// Gets a flattened string that represents the entire container.
		/// </summary>
		/// <value>The content string.</value>
		string ContentString { get; }

		#endregion
	}
}