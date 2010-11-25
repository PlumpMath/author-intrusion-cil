#region Namespaces

using AuthorIntrusion.Contracts.Contents;

#endregion

namespace AuthorIntrusion.Contracts.Delegates
{
	/// <summary>
	/// Defines the delegate which takes content and returns if the content
	/// fulfills the condition.
	/// </summary>
	public delegate bool ContentFilter(Content content);
}