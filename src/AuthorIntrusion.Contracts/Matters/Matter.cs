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

using System.Collections.Generic;

#endregion

namespace AuthorIntrusion.Contracts.Matters
{
	/// <summary>
	/// Represents document matter (i.e., front, body, and back) for a document.
	/// These represent either regions in the document (chapter, section, etc.),
	/// paragraphs (text contents), or breaks of various types.
	/// </summary>
	public abstract class Matter : Element
	{
		#region Fields

		private readonly Dictionary<object, object> dataDictionary;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Matter"/> class.
		/// </summary>
		protected Matter()
		{
			dataDictionary = new Dictionary<object, object>();
		}

		#endregion

		#region Properties

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
		public abstract MatterType MatterType { get; }

		#endregion

		#region Document Editing

		/// <summary>
		/// Creates an version of itself, but with no text or contents. This is
		/// used to duplicate lines in the text editor.
		/// </summary>
		/// <returns></returns>
		public abstract Matter CreateEmptyClone();

		#endregion

		#region Contents

		/// <summary>
		/// Retrieves the string for the structural context. This will be the
		/// title for sections and content for paragraphs.
		/// </summary>
		/// <returns>An unformatted string representing the contents.</returns>
		public abstract string GetContents();

		/// <summary>
		/// Sets the text of the structure. For sections, this will be the title
		/// and for paragraphs, it will be the unparsed contents.
		/// </summary>
		/// <param name="text">The contexts text to set.</param>
		public abstract void SetContents(string text);

		#endregion
	}
}