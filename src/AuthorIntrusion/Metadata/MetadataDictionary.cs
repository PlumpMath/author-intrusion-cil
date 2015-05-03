// <copyright file="MetadataDictionary.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System.Collections.Generic;

namespace AuthorIntrusion.Metadata
{
	/// <summary>
	/// Implements a dictionary for managing metadata keys and values.
	/// </summary>
	public class MetadataDictionary : Dictionary<MetadataKey, MetadataValue>
	{
		#region Public Methods and Operators

		/// <summary>
		/// Retrieves an existing value from the dictionary or creates a new one.
		/// </summary>
		/// <param name="key">
		/// The key to retrieve.
		/// </param>
		/// <returns>
		/// The value, either a new one or an old one.
		/// </returns>
		public MetadataValue GetOrCreate(MetadataKey key)
		{
			MetadataValue value;

			if (!TryGetValue(
				key,
				out value))
			{
				value = new MetadataValue();
				this[key] = value;
			}

			return value;
		}

		/// <summary>
		/// Sets the current instance values with the values from the given
		/// metadata.
		/// </summary>
		/// <param name="metadata">
		/// The metadata.
		/// </param>
		public void Set(MetadataDictionary metadata)
		{
			// First clear out the entry.
			Clear();

			// Go through the new metdata and copy it.
			foreach (KeyValuePair<MetadataKey, MetadataValue> entry in metadata)
			{
				this[entry.Key] = entry.Value;
			}
		}

		#endregion
	}
}
