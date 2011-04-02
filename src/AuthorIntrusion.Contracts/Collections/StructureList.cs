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

using AuthorIntrusion.Contracts.Structures;

#endregion

namespace AuthorIntrusion.Contracts.Collections
{
	/// <summary>
	/// Implements a list that manages structure elements.
	/// </summary>
	public class StructureList : List<Structure>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="StructureList"/> class.
		/// </summary>
		/// <param name="parent">The parent.</param>
		public StructureList(Structure parent)
		{
			this.parent = parent;
		}

		#endregion

		#region Structures

		private readonly Structure parent;

		/// <summary>
		/// Gets the parent structural element.
		/// </summary>
		/// <value>The parent.</value>
		public Structure Parent
		{
			get { return parent; }
		}

		#endregion

		#region List Operations

		/// <summary>
		/// Adds the specified structure to the list.
		/// </summary>
		/// <param name="structure">The structure.</param>
		public new void Add(Structure structure)
		{
			if (structure == null)
			{
				throw new ArgumentNullException("structure");
			}

			structure.Parent = parent;

			base.Add(structure);
		}

		/// <summary>
		/// Inserts a range of structures into the list.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="list">The list.</param>
		public new void InsertRange(int index, IEnumerable<Structure> list)
		{
			// Call the base to add the items.
			base.InsertRange(index, list);

			// Go through the list and reparent them.
			foreach (Structure structure in list)
			{
				structure.Parent = parent;
			}
		}

		#endregion
	}
}