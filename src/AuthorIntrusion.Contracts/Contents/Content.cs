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

using AuthorIntrusion.Contracts.Enumerations;

#endregion

namespace AuthorIntrusion.Contracts.Contents
{
	/// <summary>
	/// Base class for all content elements.
	/// </summary>
	public abstract class Content : Element
	{
		#region Contents

		/// <summary>
		/// Contains a flattened representation of the content.
		/// </summary>
		/// <value>The content string.</value>
		public abstract string ContentString { get; }

		/// <summary>
		/// Gets the type of content this object represents.
		/// </summary>
		/// <value>The type of the content.</value>
		public abstract ContentType ContentType { get; }

		/// <summary>
		/// Gets a value indicating whether this content is normally formatted
		/// with a leading space.
		/// </summary>
		/// <value><c>true</c> if [needs leading space]; otherwise, <c>false</c>.</value>
		public virtual bool NeedsLeadingSpace
		{
			get { return true; }
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
			// Show the content type plus a small section of the content itself.
			string contentString = ContentString;

			if (contentString.Length > 25)
			{
				contentString = contentString.Substring(0, 22) + "...";
			}

			return string.Format("{0} {1}", ContentType, contentString);
		}

		#endregion
	}
}