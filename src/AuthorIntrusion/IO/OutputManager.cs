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
	/// A singleton class that manages the output and writing of documents and
	/// converting them from the internal structure.
	/// </summary>
	public class OutputManager : IOutputManager
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="OutputManager"/> class.
		/// </summary>
		/// <param name="outputWriters">The output writers.</param>
		public OutputManager(IOutputWriter[] outputWriters)
		{
			writers = outputWriters;
		}

		#endregion

		#region OutputWriter Management

		private readonly IOutputWriter[] writers;

		/// <summary>
		/// Gets the writers associated with the manager.
		/// </summary>
		/// <value>The writers.</value>
		public IOutputWriter[] Writers
		{
			get { return writers; }
		}

		#endregion

		#region Writeing

		/// <summary>
		/// Writes the specified file and returns a structure elements.
		/// If there is any problems with writing the output, this should throw
		/// an exception and never return a null root structure.
		/// </summary>
		/// <param name="outputFile">The output file.</param>
		/// <param name="document">The document.</param>
		public void Write(
			FileInfo outputFile,
			Document document)
		{
			// Make sure the file exists for writing.
			if (outputFile == null)
			{
				throw new ArgumentNullException("outputFile");
			}

			// Write the file and return the structure.
			using (
				FileStream fileStream = outputFile.Open(
					FileMode.Create, FileAccess.Write, FileShare.None))
			{
				Write(fileStream, document, outputFile.Name);
			}
		}

		/// <summary>
		/// Uses the given filename and output stream to write the file using one
		/// of the registered output writers.
		/// </summary>
		/// <param name="outputStream">The output stream.</param>
		/// <param name="document">The document.</param>
		/// <param name="filename">The filename.</param>
		private void Write(
			Stream outputStream,
			Document document,
			string filename)
		{
			// Build up a list of possible writers based on filename.
			var writers = new List<IOutputWriter>();

			if (String.IsNullOrEmpty(filename))
			{
				// We don't have a filename, so we need to test all the output
				// writers to figure out which one we can use.
				writers.AddRange(this.writers);
			}
			else
			{
				// Use the filename to par down the list of writers.
				string fileExtension = Path.GetExtension(filename);

				foreach (IOutputWriter outputWriter in this.writers)
				{
					foreach (string writerExtension in outputWriter.FileExtensions)
					{
						if (fileExtension == writerExtension)
						{
							writers.Add(outputWriter);
						}
					}
				}
			}

			// At this point, we have either 0, 1, or more writers. If we have
			// 0, then nothing can parse it. If we have one, we just use that
			// writer. Otherwise, we need to use an event to determine which one
			// to use and then use the selected writer.
			if (writers.Count == 0)
			{
				throw new IOException("Cannot find an output writer for stream");
			}

			// TODO Add the processing for multiple writers.

			// Use the first writer in the list, regardless of count.
			IOutputWriter writer = writers[0];
			writer.Write(outputStream, document);
		}

		#endregion
	}
}