// <copyright file="IFileBufferFormatFactory.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System.IO;

namespace AuthorIntrusion.IO
{
	/// <summary>
	/// Indicates an IBufferFormat that can read and write files from a file system.
	/// </summary>
	public interface IFileBufferFormatFactory : IBufferFormatFactory
	{
		#region Public Methods and Operators

		/// <summary>
		/// Determines whether this instance can read the specified file. This may
		/// open the file to perform a simple scan if needed (typically for XML files).
		/// </summary>
		/// <param name="file">
		/// The file.
		/// </param>
		/// <returns>
		/// True if the format can handle it, otherwise false.
		/// </returns>
		bool CanHandle(FileInfo file);

		/// <summary>
		/// Creates an instance of a file buffer format and returns it.
		/// </summary>
		/// <returns>A constructed file buffer format.</returns>
		IFileBufferFormat Create();

		#endregion
	}
}
