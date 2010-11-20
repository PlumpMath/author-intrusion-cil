#region Namespaces

using System.Collections.Generic;

#endregion

namespace MfGames.Author.Contract.Structures.Collections
{
	/// <summary>
	/// Contains an ordered list of chapters that also manages parent
	/// relationships.
	/// </summary>
	public class ChapterList : StructureBaseList<Chapter>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="ChapterList"/> class.
		/// </summary>
		/// <param name="parent">The parent.</param>
		public ChapterList(StructureBase parent)
			: base(parent)
		{
		}

		#endregion
	}
}