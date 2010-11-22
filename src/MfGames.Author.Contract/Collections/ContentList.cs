#region Namespaces

using System.Collections.Generic;
using System.Text;

using MfGames.Author.Contract.Contents;

#endregion

namespace MfGames.Author.Contract.Collections
{
	/// <summary>
	/// Implements an ordered list of content elements.
	/// </summary>
	public class ContentList : List<Content>
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
			Add(new Unparsed(parsedContent));
		}
		
		#endregion

		#region Conversion

		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </returns>
		public override string ToString()
		{
			StringBuilder buffer = new StringBuilder();
			bool first = true;

			foreach (Content content in this)
			{
				if (first)
				{
					first = false;
				}
				else
				{
					if (content.ContentType == ContentType.Terminator)
					{
						buffer.Append(" ");
					}
				}

				buffer.Append(content.ToString());
			}

			return buffer.ToString();
		}

		#endregion
	}
}