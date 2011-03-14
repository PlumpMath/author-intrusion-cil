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
	/// Defines the signature for the input manager.
	/// </summary>
	public interface IInputManager
	{
		#region Readers

		/// <summary>
		/// Gets all the input readers associated with this manager.
		/// </summary>
		/// <value>The input readers.</value>
		IInputReader[] Readers { get; }

		#endregion

		#region Reading

		/// <summary>
		/// Reads the specified file and returns a structure elements.
		/// If there is any problems with reading the input, this should throw
		/// an exception and never return a null root structure.
		/// </summary>
		/// <param name="inputFile">The input file.</param>
		/// <returns></returns>
		Document Read(FileInfo inputFile);

		#endregion
	}
}