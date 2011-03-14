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

using System.IO;

#endregion

namespace AuthorIntrusion.Contracts.IO
{
	/// <summary>
	/// Identifies the classes used for identifying and reading input streams
	/// and generating the internal structure tree.
	/// </summary>
	public interface IInputReader
	{
		#region Identification

		/// <summary>
		/// Gets the file extensions that are commonly associated with this
		/// input reader.
		/// </summary>
		/// <value>The file mask.</value>
		string[] FileExtensions { get; }

		/// <summary>
		/// Gets the MIME types associated with this writer.
		/// </summary>
		/// <value>The MIME types.</value>
		string[] MimeTypes { get; }

		/// <summary>
		/// Gets the name of the input file.
		/// </summary>
		/// <value>The name.</value>
		string Name { get; }

		/// <summary>
		/// Determines whether this reader can read the specified input stream.
		/// </summary>
		/// <param name="inputStream">The input stream.</param>
		/// <returns>
		/// 	<c>true</c> if this instance can read the specified input stream; otherwise, <c>false</c>.
		/// </returns>
		bool CanRead(Stream inputStream);

		#endregion

		#region Reading

		/// <summary>
		/// Reads the specified input stream and returns a document representing
		/// the document.
		/// </summary>
		/// <param name="inputStream">The input stream.</param>
		/// <returns>A document representing the stream.</returns>
		Document Read(Stream inputStream);

		#endregion
	}
}