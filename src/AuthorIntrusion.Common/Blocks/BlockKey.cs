// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System;

namespace AuthorIntrusion.Common.Blocks
{
	/// <summary>
	/// Defines the unique key to identify a block.
	/// </summary>
	public struct BlockKey: IEquatable<BlockKey>
	{
		#region Properties

		/// <summary>
		/// Gets the block ID.
		/// </summary>
		public uint Id
		{
			get { return id; }
		}

		#endregion

		#region Methods

		public bool Equals(BlockKey other)
		{
			return id == other.id;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}

			return obj is BlockKey && Equals((BlockKey) obj);
		}

		public override int GetHashCode()
		{
			return (int) id;
		}

		/// <summary>
		/// Gets the next BlockKey. This is a universal ID across the system, but the
		/// ID will not be used outside of that process.
		/// </summary>
		/// <returns></returns>
		public static BlockKey GetNext()
		{
			unchecked
			{
				var key = new BlockKey(nextId++);
				return key;
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
			return string.Format("BlockKey({0})", id.ToString("X8"));
		}

		#endregion

		#region Operators

		public static bool operator ==(BlockKey left,
			BlockKey right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(BlockKey left,
			BlockKey right)
		{
			return !left.Equals(right);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes the <see cref="BlockKey"/> struct.
		/// </summary>
		static BlockKey()
		{
			nextId = 1;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BlockKey"/> struct.
		/// </summary>
		/// <param name="id">The id.</param>
		public BlockKey(uint id)
			: this()
		{
			this.id = id;
		}

		#endregion

		#region Fields

		private readonly uint id;
		private static uint nextId;

		#endregion
	}
}
