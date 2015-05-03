// <copyright file="CssClassKey.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System;
using System.Diagnostics.Contracts;

namespace AuthorIntrusion.Css
{
	/// <summary>
	/// Defines a CSS class name which is used for identifying classes of lines
	/// and tokens.
	/// </summary>
	public struct CssClassKey : IEquatable<CssClassKey>
	{
		#region Fields

		/// <summary>
		/// The name of the CSS class.
		/// </summary>
		public readonly string Name;

		#endregion

		#region Constructors and Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="CssClassKey"/> struct.
		/// </summary>
		/// <param name="name">
		/// The name.
		/// </param>
		public CssClassKey(string name)
		{
			// Establish our contracts.
			Contract.Requires(name != null);
			Contract.Requires(name.Length > 0);

			// Save the member variable.
			Name = name;
		}

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Implements the operator ==.
		/// </summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>
		/// The result of the operator.
		/// </returns>
		public static bool operator ==(CssClassKey left,
			CssClassKey right)
		{
			return left.Equals(right);
		}

		/// <summary>
		/// Implements the operator !=.
		/// </summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>
		/// The result of the operator.
		/// </returns>
		public static bool operator !=(CssClassKey left,
			CssClassKey right)
		{
			return !left.Equals(right);
		}

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <param name="other">
		/// An object to compare with this object.
		/// </param>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		public bool Equals(CssClassKey other)
		{
			return string.Equals(
				Name,
				other.Name);
		}

		/// <summary>
		/// Determines whether the specified <see cref="System.Object"/>, is equal to this instance.
		/// </summary>
		/// <param name="obj">
		/// The <see cref="System.Object"/> to compare with this instance.
		/// </param>
		/// <returns>
		/// <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
		/// </returns>
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(
				null,
				obj))
			{
				return false;
			}

			return obj is CssClassKey && Equals((CssClassKey)obj);
		}

		/// <summary>
		/// Returns a hash code for this instance.
		/// </summary>
		/// <returns>
		/// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
		/// </returns>
		public override int GetHashCode()
		{
			return Name != null ? Name.GetHashCode() : 0;
		}

		#endregion
	}
}
