// <copyright file="LineIndex.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System;
using System.Diagnostics.Contracts;

namespace MfGames.TextTokens.Lines
{
	/// <summary>
	/// A simple, low-overhead sequence for tokens.
	/// </summary>
	public struct LineIndex : IEquatable<LineIndex>
	{
		#region Static Fields

		/// <summary>
		/// A common index for 0 for the beginning.
		/// </summary>
		public static readonly LineIndex First = new LineIndex(0);

		#endregion

		#region Fields

		/// <summary>
		/// Contains the zero-based index of a line inside the buffer.
		/// </summary>
		public int Index;

		#endregion

		#region Constructors and Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="LineIndex"/> struct.
		/// </summary>
		/// <param name="index">
		/// The index.
		/// </param>
		public LineIndex(int index)
		{
			Contract.Requires(index >= 0);

			Index = index;
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
		public static bool operator ==(LineIndex left,
			LineIndex right)
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
		public static bool operator !=(LineIndex left,
			LineIndex right)
		{
			return !left.Equals(right);
		}

		/// <summary>
		/// Adds the specified offset to the index and returns the results.
		/// </summary>
		/// <param name="offset">
		/// The offset.
		/// </param>
		/// <returns>
		/// An adjusted index.
		/// </returns>
		[Pure]
		public LineIndex Add(int offset)
		{
			return new LineIndex(Index + offset);
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
		public bool Equals(LineIndex other)
		{
			return Index == other.Index;
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

			return obj is LineIndex && Equals((LineIndex)obj);
		}

		/// <summary>
		/// Returns a hash code for this instance.
		/// </summary>
		/// <returns>
		/// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
		/// </returns>
		public override int GetHashCode()
		{
			return Index;
		}

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return "LineIndex(" + Index + ")";
		}

		#endregion
	}
}
