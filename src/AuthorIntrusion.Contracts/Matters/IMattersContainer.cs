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

namespace AuthorIntrusion.Contracts.Matters
{
	/// <summary>
	/// Identifies the objects that have a list of matters underneath them,
	/// such as a Region or a Document.
	/// </summary>
	public interface IMattersContainer
	{
		/// <summary>
		/// Gets the zero-based depth of the object in the container.
		/// </summary>
		int Depth { get; }

		/// <summary>
		/// Contains a list of matter objects underneath the container.
		/// </summary>
		MatterCollection Matters { get; }

		/// <summary>
		/// Gets the parent container object.
		/// </summary>
		IMattersContainer ParentContainer { get; }

		/// <summary>
		/// Gets the document associated with this container.
		/// </summary>
		/// <value>The document.</value>
		Document ParentDocument { get; }

		/// <summary>
		/// Gets the index of this instance in its parent container.
		/// </summary>
		/// <value>
		/// The index of the item in the parent.
		/// </value>
		int ParentIndex { get; }
	}
}