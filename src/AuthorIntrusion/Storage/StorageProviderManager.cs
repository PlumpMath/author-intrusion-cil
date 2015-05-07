// <copyright file="StorageProviderManager.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System.Collections.Generic;
using System.Linq;

using AuthorIntrusion.Contracts.Settings;
using AuthorIntrusion.Contracts.Storage;

namespace AuthorIntrusion.Storage
{
	public class StorageProviderManager
	{
		#region Fields

		private readonly List<IStorageProviderFactory> factories;

		private readonly ISettings settings;

		#endregion

		#region Constructors and Destructors

		public StorageProviderManager(
			ISettings settings,
			IEnumerable<IStorageProviderFactory> factories)
		{
			this.settings = settings;
			this.factories = factories.ToList();
		}

		#endregion

		#region Public Methods and Operators

		public IStorageProviderFactory GetStorageProviderFactory(
			string storageProviderFactoryId)
		{
			IStorageProviderFactory results = factories
				.Single(f => f.StorageProviderFactoryId == storageProviderFactoryId);
			return results;
		}

		#endregion
	}
}
