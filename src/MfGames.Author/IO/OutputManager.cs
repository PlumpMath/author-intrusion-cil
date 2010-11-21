#region Namespaces

using System;
using System.Collections.Generic;
using System.IO;

using Castle.Core;

using MfGames.Author.Contract.IO;
using MfGames.Author.Contract.Structures;

#endregion

namespace MfGames.Author.IO
{
	/// <summary>
	/// A singleton class that manages the output and writing of documents and
	/// converting them from the internal structure.
	/// </summary>
	[Singleton]
	public class OutputManager : IOutputManager
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="OutputManager"/> class.
		/// </summary>
		/// <param name="outputWriters">The output writers.</param>
		public OutputManager(IOutputWriter[] outputWriters)
		{
			this.outputWriters = outputWriters;
		}

		#endregion

		#region OutputWriter Management

		private readonly IOutputWriter[] outputWriters;

		#endregion

		#region Writeing

		/// <summary>
		/// Writes the specified file and returns a structure elements.
		/// If there is any problems with writing the output, this should throw
		/// an exception and never return a null root structure.
		/// </summary>
		/// <param name="outputFile">The output file.</param>
		/// <param name="structure">The structure.</param>
		/// <returns></returns>
		public void Write(
			FileInfo outputFile,
			StructureBase structure)
		{
			// Make sure the file exists for writing.
			if (outputFile == null)
			{
				throw new ArgumentNullException("outputFile");
			}

			// Write the file and return the structure.
			using (
				FileStream fileStream = outputFile.Open(FileMode.Create,
				                                        FileAccess.Write,
				                                        FileShare.None))
			{
				Write(fileStream, structure, outputFile.Name);
			}
		}

		/// <summary>
		/// Uses the given filename and output stream to write the file using one
		/// of the registered output writers.
		/// </summary>
		/// <param name="outputStream">The output stream.</param>
		/// <param name="structure">The structure.</param>
		/// <param name="filename">The filename.</param>
		/// <returns></returns>
		private void Write(
			Stream outputStream,
			StructureBase structure,
			string filename)
		{
			// Build up a list of possible writers based on filename.
			var writers = new List<IOutputWriter>();

			if (String.IsNullOrEmpty(filename))
			{
				// We don't have a filename, so we need to test all the output
				// writers to figure out which one we can use.
				writers.AddRange(outputWriters);
			}
			else
			{
				// Use the filename to par down the list of writers.
				string fileExtension = Path.GetExtension(filename);

				foreach (IOutputWriter outputWriter in outputWriters)
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
			writer.Write(outputStream, structure);
		}

		#endregion
	}
}