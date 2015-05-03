// <copyright file="MetadataManager.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System.Collections.Generic;

namespace AuthorIntrusion.Metadata
{
	/// <summary>
	/// Management class to entire a singleton-style access to metadata keys.
	/// </summary>
	public class MetadataManager
	{
		#region Fields

		/// <summary>
		/// Contains a list of all known metadata keys.
		/// </summary>
		private readonly Dictionary<string, MetadataKey> keys;

		#endregion

		#region Constructors and Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="MetadataManager"/> class.
		/// </summary>
		public MetadataManager()
		{
			keys = new Dictionary<string, MetadataKey>();
		}

		#endregion

		#region Public Indexers

		/// <summary>
		/// Gets the singleton metadata key, creating it if needed.
		/// </summary>
		/// <value>
		/// The <see cref="MetadataKey"/>.
		/// </value>
		/// <param name="keyName">
		/// Name of the key.
		/// </param>
		/// <returns>
		/// A metadata key.
		/// </returns>
		public MetadataKey this[string keyName]
		{
			get
			{
				MetadataKey key;

				if (!keys.TryGetValue(
					keyName,
					out key))
				{
					key = new MetadataKey(keyName);
					keys[keyName] = key;
				}

				return key;
			}
		}

		#endregion
	}
}
