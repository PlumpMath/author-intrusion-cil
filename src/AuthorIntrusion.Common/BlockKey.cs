// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

namespace AuthorIntrusion.Common
{
	/// <summary>
	/// Defines the unique key to identify a block.
	/// </summary>
	public struct BlockKey
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
			return id.ToString("X8");
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
