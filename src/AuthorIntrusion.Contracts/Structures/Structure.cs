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

using AuthorIntrusion.Contracts.Enumerations;

#endregion

namespace AuthorIntrusion.Contracts.Structures
{
	/// <summary>
	/// The common root for all the structural elements.
	/// </summary>
	public abstract class Structure : Element
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Structure"/> class.
		/// </summary>
		protected Structure()
		{
			dataDictionary = new Dictionary<object, object>();
		}

		#endregion

		#region Properties

		private readonly Dictionary<object, object> dataDictionary;

		/// <summary>
		/// Contains a data dictionary which can be used to associate data
		/// with a given structure.
		/// </summary>
		public IDictionary<object, object> DataDictionary
		{
			get { return dataDictionary; }
		}

		/// <summary>
		/// Gets the type of the structure.
		/// </summary>
		/// <value>The type of the structure.</value>
		public abstract StructureType StructureType { get; set; }

		#endregion

		#region Relationships

		/// <summary>
		/// Gets a count of content container content (i.e. paragraphs) in this
		/// object or child objects.
		/// </summary>
		public abstract int ParagraphCount { get; }

		/// <summary>
		/// Gets a flattened list of all paragraphs inside the structure.
		/// </summary>
		/// <value>The paragraph list.</value>
		public abstract IList<Paragraph> ParagraphList { get; }

		#endregion
	}
}