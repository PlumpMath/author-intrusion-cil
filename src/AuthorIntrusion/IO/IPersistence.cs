// <copyright file="IPersistence.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System.IO;

using MfGames.HierarchicalPaths;

namespace AuthorIntrusion.IO
{
	/// <summary>
	/// Defines the signature for a class that manages persistence. This is effectively
	/// an abstract layer over the file system but can also represent the interface to
	/// a server or pulling contents from a zip archive.
	/// </summary>
	public interface IPersistence
	{
		#region Public Properties

		/// <summary>
		/// Gets the format of the project file.
		/// </summary>
		IFileBufferFormat ProjectFormat { get; }

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Gets a read stream for the project file.
		/// </summary>
		/// <remarks>
		/// It is the responsibility of the calling class to close the stream.
		/// </remarks>
		/// <returns>A stream to the project file.</returns>
		Stream GetProjectReadStream();

		/// <summary>
		/// Gets the write stream for the project file.
		/// </summary>
		/// <returns>
		/// A stream to the project file.
		/// </returns>
		Stream GetProjectWriteStream();

		/// <summary>
		/// Retrieves a read stream for a given path. The calling method is responsible for
		/// disposing of the stream.
		/// </summary>
		/// <remarks>
		/// It is the responsibility of the calling class to close the stream.
		/// </remarks>
		/// <param name="path">
		/// The absolute path into the project root.
		/// </param>
		/// <returns>
		/// A read stream to the path.
		/// </returns>
		Stream GetReadStream(HierarchicalPath path);

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
		Stream GetWriteStream(HierarchicalPath path);

		#endregion
	}
}
