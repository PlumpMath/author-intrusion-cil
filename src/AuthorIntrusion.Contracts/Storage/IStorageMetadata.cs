// <copyright file="IStorageMetadata.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

namespace AuthorIntrusion.Contracts.Storage
{
	/// <summary>
	/// A high-level scanner of an <c>IStorage</c> that includes pertient
	/// information about the project such as title and creator.
	/// </summary>
	public interface IStorageMetadata
	{
		#region Public Properties

		string StorageId { get; }
		string Title { get; }

		#endregion

		#region Public Methods and Operators

		IStorage GetStorage();

		#endregion
	}
}
