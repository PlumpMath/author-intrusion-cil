// <copyright file="MemoryStorageProviderFactory.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System;

using AuthorIntrusion.Contracts.Storage;

namespace AuthorIntrusion.Tests.Storage
{
	public class MemoryStorageProviderFactory : IStorageProviderFactory
	{
		#region Public Properties

		public string StorageProviderFactoryId { get { return "memory"; } }

		#endregion

		#region Public Methods and Operators

		public IStorageProvider CreateProvider(StorageProviderParameters parameters)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
