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
using System.IO;

using AuthorIntrusion.Contracts;
using AuthorIntrusion.Contracts.IO;

#endregion

namespace AuthorIntrusion.IO
{
	/// <summary>
	/// A singleton class that manages the input and reading of documents and
	/// converting them into the internal structure.
	/// </summary>
	public class InputManager : IInputManager
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="InputManager"/> class.
		/// </summary>
		/// <param name="inputReaders">The input readers.</param>
		public InputManager(IInputReader[] inputReaders)
		{
			readers = inputReaders;
		}

		#endregion

		#region InputReader Management

		private readonly IInputReader[] readers;

		/// <summary>
		/// Gets all the input readers associated with this manager.
		/// </summary>
		/// <value>The input readers.</value>
		public IInputReader[] Readers
		{
			get { return readers; }
		}

		#endregion

		#region Reading

		/// <summary>
		/// Reads the specified file and returns a structure elements.
		/// If there is any problems with reading the input, this should throw
		/// an exception and never return a null root structure.
		/// </summary>
		/// <param name="inputFile">The input file.</param>
		/// <returns></returns>
		public Document Read(FileInfo inputFile)
		{
			// Make sure the file exists for reading.
			if (inputFile == null)
			{
				throw new ArgumentNullException("inputFile");
			}

			if (!inputFile.Exists)
			{
				throw new FileNotFoundException(inputFile.FullName);
			}

			// Read the file and return the structure.
			using (
				FileStream fileStream = inputFile.Open(
					FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				return Read(fileStream, inputFile.Name);
			}
		}

		/// <summary>
		/// Uses the given filename and input stream to read the file using one
		/// of the registered input readers.
		/// </summary>
		/// <param name="inputStream">The input stream.</param>
		/// <param name="filename">The filename.</param>
		/// <returns></returns>
		private Document Read(
			Stream inputStream,
			string filename)
		{
			// Build up a list of possible readers based on filename.
			var readers = new List<IInputReader>();

			if (String.IsNullOrEmpty(filename))
			{
				// We don't have a filename, so we need to test all the input
				// readers to figure out which one we can use.
				readers.AddRange(this.readers);
			}
			else
			{
				// Use the filename to par down the list of readers.
				string fileExtension = Path.GetExtension(filename);

				foreach (IInputReader inputReader in this.readers)
				{
					foreach (string readerExtension in inputReader.FileExtensions)
					{
						if (fileExtension == readerExtension)
						{
							readers.Add(inputReader);
						}
					}
				}
			}

			// If the stream is seekable, then we then filter down by parsing
			// the files with the remaining readers.
			if (inputStream.CanSeek)
			{
				var filteredReaders = new List<IInputReader>();

				foreach (IInputReader inputReader in readers)
				{
					// See if this input reader thinks it is capable of handling
					// the given file.
					inputStream.Seek(0, SeekOrigin.Begin);

					if (inputReader.CanRead(inputStream))
					{
						filteredReaders.Add(inputReader);
					}
				}

				// Move the readers back into the main list.
				readers = filteredReaders;

				// Reposition the input stream back to the beginning.
				inputStream.Seek(0, SeekOrigin.Begin);
			}

			// At this point, we have either 0, 1, or more readers. If we have
			// 0, then nothing can parse it. If we have one, we just use that
			// reader. Otherwise, we need to use an event to determine which one
			// to use and then use the selected reader.
			if (readers.Count == 0)
			{
				throw new IOException("Cannot find an input reader for stream");
			}

			// TODO Add the processing for multiple readers.

			// Use the first reader in the list, regardless of count.
			IInputReader reader = readers[0];
			return reader.Read(inputStream);
		}

		#endregion
	}
}