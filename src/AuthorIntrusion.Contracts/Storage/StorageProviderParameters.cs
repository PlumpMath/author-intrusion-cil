// <copyright file="StorageProviderParameters.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System.Collections.Generic;

namespace AuthorIntrusion.Contracts.Storage
{
	public class StorageProviderParameters : Dictionary<string, object>
	{
		#region Constants

		public const string StorageProviderFactoryIdKey =
			"Storage Provider Factory ID";

		public const string StorageProviderIdKey = "Storage Provider ID";

		#endregion

		#region Public Properties

		public string StorageProviderFactoryId
		{
			get
			{
				var id = (string)this[StorageProviderFactoryIdKey];
				return id;
			}
		}

		public string StorageProviderId
		{
			get
			{
				var id = (string)this[StorageProviderIdKey];
				return id;
			}
		}

		#endregion
	}
}
