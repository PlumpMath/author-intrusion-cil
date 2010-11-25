#region Namespaces

using MfGames.Author.Contract.Contents;

#endregion

namespace MfGames.Author.Contract.Delegates
{
	/// <summary>
	/// Defines the delegate which takes content and returns if the content
	/// fulfills the condition.
	/// </summary>
	public delegate bool ContentFilter(Content content);
}