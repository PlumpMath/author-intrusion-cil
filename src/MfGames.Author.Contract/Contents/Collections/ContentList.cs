#region Namespaces

using System.Collections.Generic;

#endregion

namespace MfGames.Author.Contract.Contents.Collections
{
	/// <summary>
	/// Implements an ordered list of content elements.
	/// </summary>
	public class ContentList : List<ContentBase>
	{
		#region List Operations
		
		/// <summary>
		/// Adds an unparsed string to the contents list.
		/// </summary>
		/// <param name="parsedContent">
		/// A <see cref="System.String"/>
		/// </param>
		public void Add(string parsedContent)
		{
			Add(new UnparsedString(parsedContent));
		}
		
		#endregion
	}
}