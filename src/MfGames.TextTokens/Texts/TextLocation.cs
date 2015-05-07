// <copyright file="TextLocation.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System;
using System.Diagnostics.Contracts;

using MfGames.TextTokens.Lines;
using MfGames.TextTokens.Tokens;

namespace MfGames.TextTokens.Texts
{
	/// <summary>
	/// Encapsulates a line, token, and text index into a single structure.
	/// </summary>
	public struct TextLocation : IEquatable<TextLocation>
	{
		#region Fields

		/// <summary>
		/// The line index for the location.
		/// </summary>
		public readonly LineIndex LineIndex;

		/// <summary>
		/// The text index for the location.
		/// </summary>
		public readonly TextIndex TextIndex;

		/// <summary>
		/// The token index for the location.
		/// </summary>
		public readonly TokenIndex TokenIndex;

		#endregion

		#region Constructors and Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="TextLocation"/> struct.
		/// </summary>
		/// <param name="lineIndex">
		/// Index of the line.
		/// </param>
		/// <param name="tokenIndex">
		/// Index of the token.
		/// </param>
		/// <param name="textIndex">
		/// Index of the text.
		/// </param>
		public TextLocation(
			LineIndex lineIndex,
			TokenIndex tokenIndex,
			TextIndex textIndex)
		{
			LineIndex = lineIndex;
			TokenIndex = tokenIndex;
			TextIndex = textIndex;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TextLocation"/> struct.
		/// </summary>
		/// <param name="lineIndex">
		/// Index of the line.
		/// </param>
		/// <param name="tokenIndex">
		/// Index of the token.
		/// </param>
		/// <param name="textIndex">
		/// Index of the text.
		/// </param>
		public TextLocation(
			int lineIndex,
			int tokenIndex,
			int textIndex)
			: this(new LineIndex(lineIndex),
				new TokenIndex(tokenIndex),
				new TextIndex(textIndex))
		{
			Contract.Requires(lineIndex >= 0);
			Contract.Requires(tokenIndex >= 0);
			Contract.Requires(textIndex >= 0);
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
		public static bool operator ==(TextLocation left,
			TextLocation right)
		{
			return left.Equals(right);
		}

		/// <summary>
		/// Implements the operator &gt;.
		/// </summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>
		/// The result of the operator.
		/// </returns>
		public static bool operator >(TextLocation left,
			TextLocation right)
		{
			if (left.LineIndex.Index != right.LineIndex.Index)
			{
				return left.LineIndex.Index > right.LineIndex.Index;
			}

			if (left.TokenIndex.Index != right.TokenIndex.Index)
			{
				return left.TokenIndex.Index > right.TokenIndex.Index;
			}

			return left.TextIndex.Index > right.TextIndex.Index;
		}

		/// <summary>
		/// Implements the operator &gt;=.
		/// </summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>
		/// The result of the operator.
		/// </returns>
		public static bool operator >=(TextLocation left,
			TextLocation right)
		{
			return left > right || left == right;
		}

		/// <summary>
		/// Implements the operator !=.
		/// </summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>
		/// The result of the operator.
		/// </returns>
		public static bool operator !=(TextLocation left,
			TextLocation right)
		{
			return !left.Equals(right);
		}

		/// <summary>
		/// Implements the operator &lt;.
		/// </summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>
		/// The result of the operator.
		/// </returns>
		public static bool operator <(TextLocation left,
			TextLocation right)
		{
			return right > left;
		}

		/// <summary>
		/// Implements the operator &lt;=.
		/// </summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>
		/// The result of the operator.
		/// </returns>
		public static bool operator <=(TextLocation left,
			TextLocation right)
		{
			return left < right || left == right;
		}

		/// <summary>
		/// Moves the text index within the location.
		/// </summary>
		/// <param name="textIndexDelta">
		/// The text index delta.
		/// </param>
		/// <returns>
		/// The adjusted text index.
		/// </returns>
		public TextLocation AddTextIndex(int textIndexDelta)
		{
			var location = new TextLocation(
				LineIndex.Index,
				TokenIndex.Index,
				TextIndex.Index + textIndexDelta);
			return location;
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
		public bool Equals(TextLocation other)
		{
			return LineIndex.Equals(other.LineIndex)
				&& TextIndex.Equals(other.TextIndex)
				&& TokenIndex.Equals(other.TokenIndex);
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

			return obj is TextLocation && Equals((TextLocation)obj);
		}

		/// <summary>
		/// Returns a hash code for this instance.
		/// </summary>
		/// <returns>
		/// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
		/// </returns>
		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = LineIndex.GetHashCode();
				hashCode = (hashCode * 397) ^ TextIndex.GetHashCode();
				hashCode = (hashCode * 397) ^ TokenIndex.GetHashCode();
				return hashCode;
			}
		}

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return string.Format(
				"TextLocation({0}, {1}, {2})",
				LineIndex.Index,
				TokenIndex.Index,
				TextIndex.Index);
		}

		#endregion
	}
}
