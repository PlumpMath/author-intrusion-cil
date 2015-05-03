// <copyright file="MetadataValue.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.Contracts;
using System.Linq;

namespace AuthorIntrusion.Metadata
{
	/// <summary>
	/// Represents metadata values for a single metadata key. This is implemented
	/// as a sequence of strings, but provides first element access when available.
	/// </summary>
	public class MetadataValue
	{
		#region Fields

		/// <summary>
		/// Contains a list of all the string values.
		/// </summary>
		private string[] values;

		#endregion

		#region Constructors and Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="MetadataValue"/> class.
		/// </summary>
		public MetadataValue()
		{
			values = new string[0];
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets the value (or first one in the case of a sequence) and returns it. If there
		/// is no such value, then this contains null.
		/// </summary>
		/// <value>
		/// The value.
		/// </value>
		public string Value
		{
			get
			{
				if (values == null || values.Length == 0)
				{
					return null;
				}

				return values[0];
			}
		}

		/// <summary>
		/// Gets the values of the metadata key.
		/// </summary>
		/// <value>
		/// The values.
		/// </value>
		public ImmutableArray<string> Values
		{
			get { return values.ToImmutableArray(); }
		}

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Adds a metadata value to the collection.
		/// </summary>
		/// <param name="value">
		/// The value to add to the list.
		/// </param>
		public void Add(string value)
		{
			// Establish our controls.
			Contract.Requires(value != null);
			Contract.Requires(value.Length > 0);

			// If we already have it, then skip it.
			if (values.Contains(value))
			{
				return;
			}

			// Add the item to the array and rebuild it into an array.
			List<string> list = values.ToList();
			list.Add(value);

			values = list.ToArray();
		}

		/// <summary>
		/// Resets the entire metadata value to the given single element value.
		/// </summary>
		/// <param name="value">
		/// The new value to set.
		/// </param>
		public void Set(string value)
		{
			// Establish our controls.
			Contract.Requires(value != null);
			Contract.Requires(value.Length > 0);

			// Set the new value.
			values = new[] { value };
		}

		#endregion
	}
}
