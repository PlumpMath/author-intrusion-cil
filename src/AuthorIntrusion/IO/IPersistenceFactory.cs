// <copyright file="IPersistenceFactory.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System;

namespace AuthorIntrusion.IO
{
	/// <summary>
	/// A persistence factory is a factory class that hooks up a protocol, such as "file://"
	/// or "ai://" to a specific instance of an IPersistence class.
	/// </summary>
	public interface IPersistenceFactory
	{
		#region Public Properties

		/// <summary>
		/// Gets the name of the URI protocol that this factory handles, without the
		/// trailing "://". For example, "file" or "ai" instead of "file://" or "ai://".
		/// </summary>
		string Scheme { get; }

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Constructs an IPersistence object based on the given URI.
		/// </summary>
		/// <param name="uri">
		/// The URI to load.
		/// </param>
		/// <returns>
		/// An IPersistence object representing the path.
		/// </returns>
		IPersistence CreatePersistence(Uri uri);

		#endregion
	}
}
