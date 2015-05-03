// <copyright file="FilePersistenceFactory.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System;
using System.IO;

namespace AuthorIntrusion.IO
{
	/// <summary>
	/// Defines the persistence factory for the "file" protocol which interacts
	/// with the local file system.
	/// </summary>
	public class FilePersistenceFactory : IPersistenceFactory
	{
		#region Fields

		/// <summary>
		/// Contains a list of formats factories that the file system persistency
		/// is aware of.
		/// </summary>
		private readonly IFileBufferFormatFactory[] formatFactories;

		#endregion

		#region Constructors and Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="FilePersistenceFactory"/> class.
		/// </summary>
		/// <param name="formatsFactories">
		/// The formats factories.
		/// </param>
		public FilePersistenceFactory(
			IFileBufferFormatFactory[] formatsFactories)
		{
			formatFactories = formatsFactories;
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets the name of the URI protocol that this factory handles, without the
		/// trailing "://". For example, "file" or "ai" instead of "file://" or "ai://".
		/// </summary>
		public string Scheme { get { return "file"; } }

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Constructs an IPersistence object based on the given path. This will be the
		/// component of the path after the "://". For example, this method will get "a/b.xml"
		/// from "file://a/b.xml".
		/// </summary>
		/// <param name="uri">
		/// </param>
		/// <returns>
		/// An IPersistence object representing the path.
		/// </returns>
		/// <exception cref="System.NotImplementedException">
		/// </exception>
		public IPersistence CreatePersistence(Uri uri)
		{
			// For the filesystem, we need to keep track of the "project" or root file
			// so we can write it out again. If the URI represents a directory, we have
			// to identify the project file.
			string path = uri.LocalPath;

			if (Directory.Exists(path))
			{
				throw new ArgumentException(
					"The URI must point to a filename, not a directory.",
					"uri");
			}

			// Save the project file.
			var projectFile = new FileInfo(path);

			// Figure out which format can handle the project file.
			IFileBufferFormatFactory projectFormatFactory = null;

			foreach (
				IFileBufferFormatFactory formatFactory in formatFactories)
			{
				if (formatFactory.CanHandle(projectFile))
				{
					projectFormatFactory = formatFactory;
				}
			}

			if (projectFormatFactory == null)
			{
				throw new InvalidOperationException(
					"Cannot identify the format for the project file: "
						+ projectFile.Name + ".");
			}

			// The file exists, so create the file persistence for it.
			IFileBufferFormat format = projectFormatFactory.Create();
			var persistence = new FilePersistence(
				projectFile,
				format);
			return persistence;
		}

		#endregion
	}
}
