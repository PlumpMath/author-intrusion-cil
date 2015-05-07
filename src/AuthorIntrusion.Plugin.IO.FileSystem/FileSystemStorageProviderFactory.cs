// <copyright file="FileSystemStorageProviderFactory.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System;

using AuthorIntrusion.Contracts.Storage;

namespace AuthorIntrusion.Plugin.IO.FileSystem
{
	public class FileSystemStorageProviderFactory : IStorageProviderFactory
	{
		#region Public Properties

		public string StorageProviderFactoryId { get { return "filesystem"; } }

		#endregion

		#region Public Methods and Operators

		public IStorageProvider CreateProvider(StorageProviderParameters parameters)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
