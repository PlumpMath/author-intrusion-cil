// <copyright file="IStorageProviderFactory.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

namespace AuthorIntrusion.Contracts.Storage
{
	public interface IStorageProviderFactory
	{
		#region Public Properties

		string StorageProviderFactoryId { get; }

		#endregion

		#region Public Methods and Operators

		IStorageProvider CreateProvider(StorageProviderParameters parameters);

		#endregion
	}
}
