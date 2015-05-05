// <copyright file="StorageProviderManager.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System.Collections.Generic;

using AuthorIntrusion.Contracts.Storage;

namespace AuthorIntrusion.Storage
{
	public class StorageProviderManager
	{
		#region Fields

		private readonly IEnumerable<IStorageProviderFactory> factories;

		#endregion

		#region Constructors and Destructors

		public StorageProviderManager(IEnumerable<IStorageProviderFactory> factories)
		{
			this.factories = factories;
		}

		#endregion
	}
}
