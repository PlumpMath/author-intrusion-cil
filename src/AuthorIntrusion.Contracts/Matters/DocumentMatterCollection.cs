#region Copyright and License

// Copyright (c) 2011, Moonfire Games
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

#endregion

namespace AuthorIntrusion.Contracts.Matters
{
	/// <summary>
	/// Provides a flattened list and access to the the document's matter
	/// collection.
	/// </summary>
	public class DocumentMatterCollection
	{
		#region Fields

		private readonly MatterCollection matters;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="DocumentMatterCollection"/> class.
		/// </summary>
		/// <param name="matters">The matters.</param>
		public DocumentMatterCollection(MatterCollection matters)
		{
			if (matters == null)
			{
				throw new ArgumentNullException("matters");
			}

			this.matters = matters;
		}

		#endregion

		#region Access

		/// <summary>
		/// Gets the total number of Matter objects.
		/// </summary>
		public int Count
		{
			get { return matters.FlattenedCount; }
		}

		/// <summary>
		/// Gets the <see cref="AuthorIntrusion.Contracts.Matters.Matter"/> at the
		/// specified index.
		/// </summary>
		public Matter this[int index]
		{
			get { throw new NotImplementedException(); }
		}

		#endregion
	}
}