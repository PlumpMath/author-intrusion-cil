﻿// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System;

namespace AuthorIntrusion.Common.Blocks
{
	/// <summary>
	/// Identifies a location within a specific block.
	/// </summary>
	public struct BlockPosition: IEquatable<BlockPosition>
	{
		#region Properties

		public BlockKey BlockKey { get; private set; }

		public int TextIndex { get; private set; }

		#endregion

		#region Methods

		public bool Equals(BlockPosition other)
		{
			return BlockKey.Equals(other.BlockKey) && TextIndex == other.TextIndex;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}

			return obj is BlockPosition && Equals((BlockPosition) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (BlockKey.GetHashCode() * 397) ^ TextIndex;
			}
		}

		public override string ToString()
		{
			return string.Format(
				"BlockPosition ({0}, {1})", BlockKey.Id.ToString("X8"), TextIndex);
		}

		#endregion

		#region Operators

		public static bool operator ==(BlockPosition left,
			BlockPosition right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(BlockPosition left,
			BlockPosition right)
		{
			return !left.Equals(right);
		}

		#endregion

		#region Constructors

		static BlockPosition()
		{
			Empty = new BlockPosition();
		}

		public BlockPosition(
			BlockKey blockKey,
			int textIndex)
			: this()
		{
			BlockKey = blockKey;
			TextIndex = textIndex;
		}

		public BlockPosition(
			Block block,
			int textIndex)
			: this(block.BlockKey, textIndex)
		{
		}

		#endregion

		#region Fields

		/// <summary>
		/// Gets the empty block position which points to an all zero key and position.
		/// </summary>
		public static readonly BlockPosition Empty;

		#endregion
	}
}
