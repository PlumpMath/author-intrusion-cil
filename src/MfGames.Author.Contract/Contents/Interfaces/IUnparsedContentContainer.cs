#region Namespaces

using MfGames.Author.Contract.Contents.Collections;

#endregion

namespace MfGames.Author.Contract.Contents.Interfaces
{
	/// <summary>
	/// Defines an item that can contain unparsed contents.
	/// </summary>
	public interface IUnparsedContentContainer
	{
		#region Contents

		/// <summary>
		/// Gets the unparsed contents in the container.
		/// </summary>
		/// <value>The unparsed contents.</value>
		ContentList UnparsedContents { get; }

		#endregion
	}
}