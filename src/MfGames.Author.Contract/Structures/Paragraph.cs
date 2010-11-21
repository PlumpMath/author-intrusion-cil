#region Namespaces

using MfGames.Author.Contract.Contents.Collections;
using MfGames.Author.Contract.Contents.Interfaces;

#endregion

namespace MfGames.Author.Contract.Structures
{
	/// <summary>
	/// Represents a single paragraph within the document.
	/// </summary>
	public class Paragraph : StructureBase, IUnparsedContentContainer
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Paragraph"/> class.
		/// </summary>
		public Paragraph()
		{
			unparsedContents = new ContentList();
		}

		#endregion

		#region Contents

		private readonly ContentList unparsedContents;

		/// <summary>
		/// Gets the unparsed contents which is a list of partially formatted
		/// content elements along with UnparsedString.
		/// </summary>
		/// <value>The unparsed contents.</value>
		public ContentList UnparsedContents { get { return unparsedContents; } }

		#endregion
	}
}