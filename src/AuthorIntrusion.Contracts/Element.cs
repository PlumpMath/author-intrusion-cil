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

using System.Collections.Generic;

using AuthorIntrusion.Contracts.Interfaces;

#endregion

namespace AuthorIntrusion.Contracts
{
	/// <summary>
	/// Top-level class for all elements inside the internal model. The
	/// primary extending classes are Structure and Content.
	/// </summary>
	public abstract class Element
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Element"/> class.
		/// </summary>
		protected Element()
		{
			tags = new HashSet<IElementTag>();
		}

		#endregion

		#region Relationships

		/// <summary>
		/// Gets or sets the parent structure element.
		/// </summary>
		/// <value>The parent.</value>
		public Element Parent { get; set; }

		#endregion

		#region Tags

		private readonly HashSet<IElementTag> tags;

		/// <summary>
		/// Gets the tags associated with this element.
		/// </summary>
		/// <value>The tags.</value>
		public HashSet<IElementTag> Tags
		{
			get { return tags; }
		}

		#endregion
	}
}