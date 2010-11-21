#region Namespaces

using MfGames.Author.Contract.Contents.Collections;

#endregion

namespace MfGames.Author.Contract.Languages
{
	/// <summary>
	/// Defines the signature for a class that splits contents.
	/// </summary>
	public interface IContentSplitter
	{
		#region Splitting

		/// <summary>
		/// Splits the specified contents into parsed components.
		/// </summary>
		/// <param name="contents">The contents.</param>
		/// <returns></returns>
		ContentList SplitContents(ContentList contents);

		#endregion
	}
}