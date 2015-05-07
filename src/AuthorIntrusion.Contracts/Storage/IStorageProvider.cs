// <copyright file="IStorageProvider.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

namespace AuthorIntrusion.Contracts.Storage
{
	/// <summary>
	/// Represents a source for <c>IStorage</c> including information of their
	/// contextual organization (directories, categories). A single provider
	/// may have login or instance-specific variables and there can be more
	/// than one of a given type in the system at the same time.
	/// </summary>
	public interface IStorageProvider
	{
		#region Public Properties

		StorageProviderParameters Parameters { get; }
		string StorageProviderId { get; }

		#endregion

		#region Public Methods and Operators

		IStorageContainer GetDefaultContainer();

		IStorage GetStorage(string storageId);

		#endregion
	}
}
