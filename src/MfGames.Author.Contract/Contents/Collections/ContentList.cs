#region Namespaces

using System.Collections.Generic;
using System.Text;

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

			foreach (ContentBase content in this)
			{
				if (first)
				{
					first = false;
				}
				else
				{
					if (!(content is Terminator))
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