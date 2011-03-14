#region Copyright and License

// Copyright (c) 2005-2011, Moonfire Games
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

#endregion

#region Namespaces

using System;
using System.Collections.Generic;
using System.Text;

using AuthorIntrusion.Contracts.Contents;
using AuthorIntrusion.Contracts.Delegates;
using AuthorIntrusion.Contracts.Enumerations;
using AuthorIntrusion.Contracts.Interfaces;

#endregion

namespace AuthorIntrusion.Contracts.Collections
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