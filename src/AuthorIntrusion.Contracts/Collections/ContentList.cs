#region Namespaces

using System;
using System.Collections.Generic;
using System.Text;

using MfGames.Author.Contract.Contents;
using MfGames.Author.Contract.Delegates;
using MfGames.Author.Contract.Enumerations;
using MfGames.Author.Contract.Interfaces;

#endregion

namespace MfGames.Author.Contract.Collections
{
	/// <summary>
	/// Implements an ordered list of content elements.
	/// </summary>
	public class ContentList : List<Content>
	{
		#region Contents

		/// <summary>
		/// Gets a flattened string representing the entire contents.
		/// </summary>
		/// <value>The content string.</value>
		public string ContentString
		{
			get
			{
				// Go through the contents and add the the child contents's
				// flattened string.
				var buffer = new StringBuilder();
				bool isFirst = true;

				foreach (Content content in this)
				{
					// Check to see if we need a space before this content.
					if (isFirst)
					{
						// No leading space in the buffer.
						isFirst = false;
					}
					else if (content.NeedsLeadingSpace)
					{
						buffer.Append(" ");
					}

					// Add the flattened view the contents.
					buffer.Append(content.ContentString);
				}

				// Return the resulting string.
				return buffer.ToString();
			}
		}

		/// <summary>
		/// Gets the count of content based on the delegate filter.
		/// </summary>
		/// <param name="contentFilter">The content filter.</param>
		/// <returns></returns>
		public bool EndsWith(ContentFilter contentFilter)
		{
			// If we don't have anything, this is always false.
			if (Count == 0)
			{
				return false;
			}

			// Grab the last item on the list.
			Content last = this[Count - 1];
			return contentFilter(last);
		}

		/// <summary>
		/// Gets the count of content based on the delegate filter.
		/// </summary>
		/// <param name="contentFilter">The counter.</param>
		/// <returns></returns>
		public int GetCount(ContentFilter contentFilter)
		{
			// Go through all the contents in this list.
			int count = 0;

			foreach (Content content in this)
			{
				// First test this content.
				if (contentFilter(content))
				{
					count++;
				}

				// If the content is a container, then go into the child content.
				if (content is IContentContainer)
				{
					var contentContainer = (IContentContainer) content;
					count += contentContainer.Contents.GetCount(contentFilter);
				}
			}

			// Return the resulting count.
			return count;
		}

		/// <summary>
		/// Returns true if the last content in the container is a terminating
		/// puncuation.
		/// </summary>
		/// <returns></returns>
		public bool GetEndsWithTerminator()
		{
			return EndsWith(
				delegate(Content content)
				{
					if (content is IContentContainer)
					{
						return ((IContentContainer) content).Contents.GetEndsWithTerminator();
					}

					return content.ContentType == ContentType.Terminator;
				});
		}

		/// <summary>
		/// Gets the count of unparsed content.
		/// </summary>
		/// <returns></returns>
		public int GetUnparsedCount()
		{
			return GetCount(content => content.ContentType == ContentType.Unparsed);
		}

		#endregion

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

		/// <summary>
		/// Replaces the specified contents with the new list.
		/// </summary>
		/// <param name="contents">The contents.</param>
		public void Replace(ContentList contents)
		{
			if (contents == null)
			{
				throw new ArgumentNullException("contents");
			}

			Clear();
			AddRange(contents);
		}

		#endregion
	}
}