#region Namespaces

using MfGames.Author.Contract.Structures.Collections;

#endregion

namespace MfGames.Author.Contract.Structures.Interfaces
{
	/// <summary>
	/// Describes a structure element that contain zero or more sections.
	/// </summary>
	public interface ISectionContainer
	{
		/// <summary>
		/// Gets the sections inside the container.
		/// </summary>
		/// <value>The sections.</value>
		SectionList Sections { get; }
	}
}