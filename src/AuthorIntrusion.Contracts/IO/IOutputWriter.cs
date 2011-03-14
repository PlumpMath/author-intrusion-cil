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
	/// Defines the signature for something that can translate the internal
	/// structure into a specific format.
	/// </summary>
	public interface IOutputWriter
	{
		#region Identification

		/// <summary>
		/// Gets the file extensions that are commonly associated with this
		/// writer.
		/// </summary>
		/// <value>The file mask.</value>
		string[] FileExtensions { get; }

		/// <summary>
		/// Gets the MIME types associated with this writer.
		/// </summary>
		/// <value>The MIME types.</value>
		string[] MimeTypes { get; }

		/// <summary>
		/// Gets the name of the output format.
		/// </summary>
		/// <value>The name.</value>
		string Name { get; }

		#endregion

		#region Writing

		/// <summary>
		/// Writes out the root structure to the given output stream.
		/// </summary>
		/// <param name="outputStream">The output stream.</param>
		/// <param name="document">The document to write out.</param>
		void Write(
			Stream outputStream,
			Document document);

		#endregion
	}
}