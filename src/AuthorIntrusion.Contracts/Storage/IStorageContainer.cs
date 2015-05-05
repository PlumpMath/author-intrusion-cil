// <copyright file="IStorageContainer.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System.Collections.Generic;

namespace AuthorIntrusion.Contracts.Storage
{
	/// <summary>
	/// Represents a category or directory inside a storage provider.
	/// </summary>
	public interface IStorageContainer
	{
		#region Public Properties

		IStorageContainer Parent { get; }

		#endregion

		#region Public Methods and Operators

		IEnumerable<IStorageContainer> GetContainers();

		IEnumerable<IStorage> GetStorageMetdata();

		#endregion
	}
}
