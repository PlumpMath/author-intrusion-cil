// <copyright file="NameDictionary.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System.Collections.Generic;
using System.Linq;

namespace AuthorIntrusion.IO
{
	/// <summary>
	/// Represents a collection of names, which have different usages or levels. There are
	/// a number of constants within the collection to represent common names.
	/// </summary>
	public class NameDictionary : Dictionary<string, string>
	{
		#region Constants

		/// <summary>
		/// The constant key into the dictionary for first names.
		/// </summary>
		public const string FirstNameKey = "First Name";

		/// <summary>
		/// The constant key into the dictionary for last names.
		/// </summary>
		public const string LastNameKey = "LastName";

		/// <summary>
		/// The constant key into the dictionary for the preferred name.
		/// </summary>
		public const string PreferredNameKey = "Preferred Name";

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets the first name of the dictionary. If it doesn't exist, this will
		/// return null.
		/// </summary>
		public string FirstName
		{
			get
			{
				return ContainsKey(FirstNameKey)
					? this[FirstNameKey]
					: null;
			}

			set
			{
				if (value == null)
				{
					Remove(FirstNameKey);
				}
				else
				{
					this[FirstNameKey] = value;
				}
			}
		}

		/// <summary>
		/// Gets or sets the last name of the dictionary. If it doesn't exist, this will
		/// return null.
		/// </summary>
		public string LastName
		{
			get { return ContainsKey(LastNameKey) ? this[LastNameKey] : null; }

			set
			{
				if (value == null)
				{
					Remove(LastNameKey);
				}
				else
				{
					this[LastNameKey] = value;
				}
			}
		}

		/// <summary>
		/// Gets or sets the preferred name of the dictionary. If it doesn't exist, this will
		/// return null.
		/// </summary>
		public string PreferredName
		{
			get
			{
				return ContainsKey(PreferredNameKey)
					? this[PreferredNameKey]
					: null;
			}

			set
			{
				if (value == null)
				{
					Remove(PreferredNameKey);
				}
				else
				{
					this[PreferredNameKey] = value;
				}
			}
		}

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			// Try to identify how to format the character's name.
			if (ContainsKey(PreferredNameKey))
			{
				return PreferredName;
			}

			// See if we have a first and/or a last name.
			if (ContainsKey(FirstNameKey) || ContainsKey(LastNameKey))
			{
				return string.Format(
					"{0} {1}",
					FirstName,
					LastName)
					.Trim();
			}

			// If we still don't have a name, pick a random one.
			if (Count > 0)
			{
				return this[Keys.OrderBy(k => k)
					.First()];
			}

			// There are no names within the container, so just give a placeholder.
			return "<Unnamed>";
		}

		#endregion
	}
}
