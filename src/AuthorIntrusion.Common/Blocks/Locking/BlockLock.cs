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
	public class BlockLock: IDisposable
	{
		#region Constructors

		/// <summary>
		/// Acquires a read lock on both the block and the block collection.
		/// </summary>
		/// <param name="collectionLock">The lock on the block collection.</param>
		/// <param name="blockLock">The lock object used to acquire the lock.</param>
		/// <param name="requestLock"></param>
		public BlockLock(
			IDisposable collectionLock,
			ReaderWriterLockSlim accessLock,
			RequestLock requestLock)
		{
			// Keep track of the collection lock so we can release it.
			this.collectionLock = collectionLock;

			// Acquire the lock based on the requested type.
			switch (requestLock)
			{
				case RequestLock.Read:
					blockLock = new NestableReadLock(accessLock);
					break;

				case RequestLock.UpgradableRead:
					blockLock = new NestableUpgradableReadLock(accessLock);
					break;

				case RequestLock.Write:
					blockLock = new NestableWriteLock(accessLock);
					break;

				default:
					throw new InvalidOperationException(
						"Could not acquire lock with unknown type: " + requestLock);
			}
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

		private IDisposable blockLock;
		private IDisposable collectionLock;

		#endregion
	}
}
