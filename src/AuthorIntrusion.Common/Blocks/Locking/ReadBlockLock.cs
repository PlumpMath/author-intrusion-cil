// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System;
using System.Threading;
using MfGames.Locking;

namespace AuthorIntrusion.Common.Blocks.Locking
{
	/// <summary>
	/// Implements a read-only lock on the given block. This intended to be used
	/// in an using() code block and released once the read-lock is no longer
	/// needed.
	/// </summary>
	public class ReadBlockLock: IDisposable
	{
		#region Constructors

		/// <summary>
		/// Acquires a read lock on both the block and the block collection.
		/// </summary>
		/// <param name="block">The block to get read access to.</param>
		/// <param name="readerWriterLock">The lock object used to acquire the lock.</param>
		public ReadBlockLock(
			Block block,
			ReaderWriterLockSlim readerWriterLock)
		{
			// We always get a lock on the collection first.
			collectionLock = block.Project.Blocks.AcquireReadLock();

			// Get a read lock on this block.
			blockLock = new NestableReadLock(readerWriterLock);
		}

		#endregion

		#region Destructors

		public void Dispose()
		{
			// We have to release the block lock first before we release the collection.
			if (blockLock != null)
			{
				blockLock.Dispose();
				blockLock = null;
			}

			// Release the collection lock last to avoid deadlocks.
			if (collectionLock != null)
			{
				collectionLock.Dispose();
				collectionLock = null;
			}
		}

		#endregion

		#region Fields

		private NestableReadLock blockLock;
		private IDisposable collectionLock;

		#endregion
	}
}
