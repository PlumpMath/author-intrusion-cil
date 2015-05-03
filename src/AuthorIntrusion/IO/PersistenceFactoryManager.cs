// <copyright file="PersistenceFactoryManager.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System;
using System.Linq;

namespace AuthorIntrusion.IO
{
	/// <summary>
	/// Implements the singleton instance for managing all persistence frameworks.
	/// </summary>
	public class PersistenceFactoryManager
	{
		#region Fields

		/// <summary>
		/// Contains a list of factories the manager is aware of.
		/// </summary>
		private readonly IPersistenceFactory[] factories;

		#endregion

		#region Constructors and Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="PersistenceFactoryManager"/> class.
		/// </summary>
		/// <param name="factories">
		/// The factories.
		/// </param>
		public PersistenceFactoryManager(IPersistenceFactory[] factories)
		{
			this.factories = factories;
		}

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Creates and returns an IPersistence that represents a given URI. This is
		/// the same as calling GetFactory().CreatePersistence().
		/// </summary>
		/// <param name="uri">
		/// The URI to load the persistence.
		/// </param>
		/// <returns>
		/// A persistence object representing the URI.
		/// </returns>
		public IPersistence CreatePersistence(Uri uri)
		{
			IPersistenceFactory factory = GetFactory(uri);
			IPersistence persistence = factory.CreatePersistence(uri);
			return persistence;
		}

		/// <summary>
		/// Retrieves the factory for a given URI scheme.
		/// </summary>
		/// <param name="uri">
		/// The URI.
		/// </param>
		/// <returns>
		/// A factory representing the given URI.
		/// </returns>
		public IPersistenceFactory GetFactory(Uri uri)
		{
			IPersistenceFactory factory =
				factories.First(f => f.Scheme == uri.Scheme);
			return factory;
		}

		#endregion
	}
}
