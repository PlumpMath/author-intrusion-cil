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
			get
			{
				// Go through the top-level collections.
				return GetMatterAt(matters, index, index);
			}
		}

		/// <summary>
		/// Gets the matter at a given index.
		/// </summary>
		/// <param name="currentMatters">The matters.</param>
		/// <param name="originalIndex">Index of the original.</param>
		/// <param name="relativeIndex">The index.</param>
		/// <returns></returns>
		private static Matter GetMatterAt(
			MatterCollection currentMatters,
			int originalIndex,
			int relativeIndex)
		{
			// Go through the matters and see if one matches. We decrement
			// the index as we go so we can always work with relative indexes.
			foreach (Matter matter in currentMatters)
			{
				// If we are at index 0, then this is the matter.
				if (relativeIndex == 0)
				{
					return matter;
				}

				// Decrement a relative index.
				relativeIndex--;

				// Check to see if this matter is a container.
				var container = matter as IMattersContainer;

				if (container != null)
				{
					// This is inside the container, so recurse into it.
					if (relativeIndex < container.Matters.FlattenedCount)
					{
						return GetMatterAt(container.Matters, originalIndex, relativeIndex);
					}

					// We weren't in the container, so skip over it.
					relativeIndex -= container.Matters.FlattenedCount;
				}
			}

			// If we got this far, we can't find it.
			throw new IndexOutOfRangeException(
				"Cannot find Matter at index " + originalIndex + ".");
		}

		#endregion
	}
}