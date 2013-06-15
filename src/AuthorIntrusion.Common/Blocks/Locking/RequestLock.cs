// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

namespace AuthorIntrusion.Common.Blocks.Locking
{
	/// <summary>
	/// An enumeration of requested lock types for locking collections and blocks.
	/// </summary>
	public enum RequestLock
	{
		/// <summary>
		/// Requests a read-only lock.
		/// </summary>
		Read,

		/// <summary>
		/// Requests a read lock that can be upgraded to a write.
		/// </summary>
		UpgradableRead,

		/// <summary>
		/// Requests a write lock.
		/// </summary>
		Write,
	}
}
