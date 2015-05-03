// <copyright file="MemoryPersistence.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using AuthorIntrusion.IO;

using MfGames.HierarchicalPaths;
using MfGames.IO;

namespace AuthorIntrusion.Tests
{
	/// <summary>
	/// Implements a persistence class that has all of the data stored in memory.
	/// </summary>
	public class MemoryPersistence : IPersistence
	{
		#region Fields

		/// <summary>
		/// Contains the registered data keyed by the relative file.
		/// </summary>
		private readonly Dictionary<HierarchicalPath, byte[]> data;

		#endregion

		#region Constructors and Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="MemoryPersistence"/> class.
		/// </summary>
		public MemoryPersistence()
		{
			data = new Dictionary<HierarchicalPath, byte[]>();
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets the number of data elements in memory.
		/// </summary>
		/// <value>
		/// The data count.
		/// </value>
		public int DataCount { get { return data.Count; } }

		/// <summary>
		/// Gets or sets the format of the project file.
		/// </summary>
		public IFileBufferFormat ProjectFormat { get; set; }

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Retrieves the lines from a given data element.
		/// </summary>
		/// <param name="path">
		/// The path.
		/// </param>
		/// <returns>
		/// An array of lines.
		/// </returns>
		public List<string> GetDataLines(string path)
		{
			return GetDataLines(new HierarchicalPath(path));
		}

		/// <summary>
		/// Retrieves the lines from a given data element.
		/// </summary>
		/// <param name="path">
		/// The path.
		/// </param>
		/// <returns>
		/// An array of lines.
		/// </returns>
		public List<string> GetDataLines(HierarchicalPath path)
		{
			// Get the element from the persistence.
			byte[] bytes = data[path];

			// Scan it as a UTF-8 lines.
			var lines = new List<string>();

			using (var stream = new MemoryStream(bytes))
			using (var reader = new StreamReader(stream))
			{
				string line;

				while ((line = reader.ReadLine()) != null)
				{
					lines.Add(line);
				}
			}

			// Return the resulting lines.
			return lines;
		}

		/// <summary>
		/// Gets a read stream for the project file.
		/// </summary>
		/// <returns>
		/// A stream to the project file.
		/// </returns>
		/// <remarks>
		/// It is the responsibility of the calling class to close the stream.
		/// </remarks>
		public Stream GetProjectReadStream()
		{
			return GetReadStream(new HierarchicalPath("/"));
		}

		/// <summary>
		/// Gets the write stream for the project file.
		/// </summary>
		/// <returns>
		/// A stream to the project file.
		/// </returns>
		public Stream GetProjectWriteStream()
		{
			return GetWriteStream(new HierarchicalPath("/"));
		}

		/// <summary>
		/// Retrieves a read stream for a given path. The calling method is responsible for
		/// disposing of the stream.
		/// </summary>
		/// <param name="path">
		/// The absolute path into the project root.
		/// </param>
		/// <returns>
		/// A read stream to the path.
		/// </returns>
		/// <remarks>
		/// It is the responsibility of the calling class to close the stream.
		/// </remarks>
		public Stream GetReadStream(HierarchicalPath path)
		{
			if (!data.ContainsKey(path))
			{
				throw new Exception(
					"Cannot get read stream for path: " + path + ".");
			}

			byte[] bytes = data[path];
			return new MemoryStream(
				bytes,
				false);
		}

		/// <summary>
		/// Gets the write stream for a given path relative to the project. The calling
		/// method is responsible for disposing of the stream.
		/// </summary>
		/// <param name="path">
		/// The path.
		/// </param>
		/// <returns>
		/// A stream to the persistence object.
		/// </returns>
		public Stream GetWriteStream(HierarchicalPath path)
		{
			// Create a memory stream and wrap it into a callback so we can record the
			// results once the stream is closed.
			var stream = new MemoryStream();
			var callback = new CallbackStream<MemoryStream>(stream);

			callback.Closed += (sender,
				args) => SetData(
					path,
					args.UnderlyingStream);

			// Return the resulting callback stream.
			return callback;
		}

		/// <summary>
		/// Sets the data for a given path.
		/// </summary>
		/// <param name="path">
		/// The path.
		/// </param>
		/// <param name="lines">
		/// The lines.
		/// </param>
		public void SetData(
			HierarchicalPath path,
			params string[] lines)
		{
			// Convert the lines to an UTF-8 byte array.
			using (var stream = new MemoryStream())
			using (var writer = new StreamWriter(
				stream,
				Encoding.UTF8))
			{
				// Go through the lines and write them out.
				foreach (string line in lines)
				{
					writer.WriteLine(line);
				}

				// Set the byte array.
				writer.Flush();
				stream.Flush();
				stream.Position = 0L;

				byte[] buffer = stream.ToArray();

				data[path] = buffer;
			}
		}

		/// <summary>
		/// Sets the data for a given path.
		/// </summary>
		/// <param name="path">
		/// The path.
		/// </param>
		/// <param name="lines">
		/// The lines.
		/// </param>
		public void SetData(
			string path,
			params string[] lines)
		{
			var newPath = new HierarchicalPath(path);
			SetData(
				newPath,
				lines);
		}

		#endregion

		#region Methods

		/// <summary>
		/// Sets the data from a stream.
		/// </summary>
		/// <param name="path">
		/// The path.
		/// </param>
		/// <param name="stream">
		/// The stream.
		/// </param>
		private void SetData(
			HierarchicalPath path,
			MemoryStream stream)
		{
			// Figure out the buffer without the trailing nulls.
			byte[] bytes = stream.GetBuffer();
			int index = bytes.Length - 1;

			while (index >= 0 && bytes[index] == 0)
			{
				index--;
			}

			// Copy the truncated byte array out.
			var truncated = new byte[index + 1];

			Array.Copy(
				bytes,
				0,
				truncated,
				0,
				index + 1);

			// Set the data.
			data[path] = truncated;
		}

		#endregion
	}
}
