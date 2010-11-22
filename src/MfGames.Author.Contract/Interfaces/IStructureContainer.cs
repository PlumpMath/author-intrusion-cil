#region Namespaces

using MfGames.Author.Contract.Collections;

#endregion

namespace MfGames.Author.Contract.Interfaces
{
	/// <summary>
	/// Describes a structure element that contain zero or more sections.
	/// </summary>
	public interface IStructureContainer
	{
		/// <summary>
		/// Contains a list of structures inside the container.
		/// </summary>
		/// <value>The child structures or an empty list.</value>
		StructureList Structures { get; }
	}
}