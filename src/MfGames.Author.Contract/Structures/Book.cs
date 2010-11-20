#region Namespaces

using MfGames.Author.Contract.Structures.Collections;
using MfGames.Author.Contract.Structures.Interfaces;

#endregion

namespace MfGames.Author.Contract.Structures
{
	/// <summary>
	/// Defines a top-level structural element for the document.
	/// </summary>
	public class Book : StructureBase, IRootStructure
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Book"/> class.
		/// </summary>
		public Book()
		{
			chapters = new ChapterList(this);
		}

		#endregion

		#region Structural Relationships

		private readonly ChapterList chapters;

		/// <summary>
		/// Gets the chapters associated with this book.
		/// </summary>
		/// <value>The chapters.</value>
		public ChapterList Chapters
		{
			get { return chapters; }
		}

		#endregion
	}
}