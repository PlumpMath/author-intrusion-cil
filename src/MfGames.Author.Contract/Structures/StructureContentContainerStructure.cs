#region Namespaces

using System.Diagnostics;

using MfGames.Author.Contract.Collections;
using MfGames.Author.Contract.Interfaces;

#endregion

namespace MfGames.Author.Contract.Structures
{
	/// <summary>
	/// Defines the common functionality where an object can contain both
	/// inner sections and contents.
	/// </summary>
	public class StructureContentContainerStructure
		: StructureContainerStructure, IContentContainer
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Chapter"/> class.
		/// </summary>
		public StructureContentContainerStructure()
		{
			contents = new ContentList();
		}

		#endregion

		#region Structural Relationships
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly ContentList contents;

		public ContentList Contents
		{
			get { return contents; }
		}


		#endregion
	}
}