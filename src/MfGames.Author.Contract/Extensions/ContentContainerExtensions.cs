#region Namespaces

using MfGames.Author.Contract.Contents;
using MfGames.Author.Contract.Enumerations;
using MfGames.Author.Contract.Interfaces;

#endregion

namespace MfGames.Author.Contract.Extensions
{
	/// <summary>
	/// Defines the various extensions that operate on the IContentContainer
	/// classes.
	/// </summary>
	public static class ContentContainerExtensions
	{
		/// <summary>
		/// Returns true if the last content in the container is a terminating
		/// puncuation.
		/// </summary>
		/// <param name="contentContainer">The content container.</param>
		/// <returns></returns>
		public static bool GetEndsWithTerminator(
			this IContentContainer contentContainer)
		{
			return contentContainer.Contents.EndsWith(
				delegate(Content content)
				{
					if (content is IContentContainer)
					{
						return ((IContentContainer) content).GetEndsWithTerminator();
					}

					return content.ContentType == ContentType.Terminator;
				});
		}

		/// <summary>
		/// Gets the count of unparsed content.
		/// </summary>
		/// <param name="contentContainer">The content container.</param>
		/// <returns></returns>
		public static int GetUnparsedCount(this IContentContainer contentContainer)
		{
			return
				contentContainer.Contents.GetCount(
					content => content.ContentType == ContentType.Unparsed);
		}
	}
}