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

using AuthorIntrusion.Contracts.Collections;
using AuthorIntrusion.Contracts.Interfaces;

#endregion

namespace AuthorIntrusion.Contracts.Structures
{
	/// <summary>
	/// Defines the common functionality of a structure that contains other
	/// structures. For example, a DocBook 5's &lt;book /&gt; element can
	/// contain other structural elements.
	/// </summary>
	public class Section : Structure, IStructureContainer
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Section"/> class.
		/// </summary>
		public Section()
		{
			structures = new StructureList(this);
		}

		#endregion

		#region Structures

		private readonly StructureList structures;

		/// <summary>
		/// Gets a count of content container content (i.e. paragraphs) in this
		/// object or child objects.
		/// </summary>
		public override int ParagraphCount
		{
			get
			{
				int count = 0;

				foreach (Structure structure in structures)
				{
					count += structure.ParagraphCount;
				}

				return count;
			}
		}

		/// <summary>
		/// Gets a flattened list of all paragraphs inside the structure.
		/// </summary>
		/// <value>The paragraph list.</value>
		public override IList<Paragraph> ParagraphList
		{
			get
			{
				var paragraphs = new List<Paragraph>();

				foreach (Structure structure in structures)
				{
					paragraphs.AddRange(structure.ParagraphList);
				}

				return paragraphs;
			}
		}

		/// <summary>
		/// Contains a list of structures inside the container.
		/// </summary>
		/// <value>The child structures or an empty list.</value>
		public StructureList Structures
		{
			get { return structures; }
		}

		#endregion
	}
}