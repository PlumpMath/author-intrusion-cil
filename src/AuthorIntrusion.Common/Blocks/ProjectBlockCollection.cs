// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System;
using System.Threading;
using AuthorIntrusion.Common.Blocks.Locking;
using MfGames.Locking;

namespace AuthorIntrusion.Common.Blocks
{
	/// <summary>
	/// A block collection that manages ownership of blocks along with processing
	/// of both collection operations (insert, delete) and block operations (insert
	/// text).
	/// </summary>
	public class ProjectBlockCollection: BlockCollection
	{
		#region Properties

		/// <summary>
		/// Gets the project associated with this collection.
		/// </summary>
		public Project Project { get; private set; }

		#endregion

		#region Methods

		/// <summary>
		/// Acquires a lock on the collection, of the requested type, and also a lock
		/// on the block referenced by the index.
		/// </summary>
		/// <param name="blockIndex">The index of the block to lock.</param>
		/// <param name="block">The block retrieved by the index.</param>
		/// <param name="requestedBlockLock"></param>
		/// <returns>An opaque lock object that will release the lock on disposal.</returns>
		public IDisposable AcquireBlockLock(
			RequestLock requestedBlockLock,
			int blockIndex,
			out Block block)
		{
			return AcquireBlockLock(
				RequestLock.Read, requestedBlockLock, blockIndex, out block);
		}

		/// <summary>
		/// Acquires a lock on the collection, of the requested type, and also a lock
		/// on the block referenced by the index.
		/// </summary>
		/// <param name="requestedCollectionLock"></param>
		/// <param name="blockIndex">The index of the block to lock.</param>
		/// <param name="block">The block retrieved by the index.</param>
		/// <param name="requestedBlockLock"></param>
		/// <returns>An opaque lock object that will release the lock on disposal.</returns>
		public IDisposable AcquireBlockLock(
			RequestLock requestedCollectionLock,
			RequestLock requestedBlockLock,
			int blockIndex,
			out Block block)
		{
			// Start by getting a read lock on the collection itself.
			IDisposable collectionLock = AcquireLock(requestedCollectionLock);

			// Grab the block via the index.
			block = this[blockIndex];

			// Get a read lock on the block and then return it.
			IDisposable blockLock = block.AcquireLock(
				collectionLock, requestedCollectionLock);
			return blockLock;
		}

		public IDisposable AcquireBlockLock(
			RequestLock requestedBlockLock,
			BlockKey blockKey,
			out Block block)
		{
			return AcquireBlockLock(
				RequestLock.Read, requestedBlockLock, blockKey, out block);
		}

		public IDisposable AcquireBlockLock(
			RequestLock requestedCollectionLock,
			RequestLock requestedBlockLock,
			BlockKey blockKey,
			out Block block)
		{
			// Start by getting a read lock on the collection itself.
			IDisposable collectionLock = AcquireLock(requestedCollectionLock);

			// Grab the block via the index.
			block = this[blockKey];

			// Get a read lock on the block and then return it.
			IDisposable blockLock = block.AcquireLock(collectionLock, requestedBlockLock);
			return blockLock;
		}

		public IDisposable AcquireBlockLock(
			RequestLock requestedBlockLock,
			Block block)
		{
			return AcquireBlockLock(RequestLock.Read, requestedBlockLock, block);
		}

		public IDisposable AcquireBlockLock(
			RequestLock requestedCollectionLock,
			RequestLock requestedBlockLock,
			Block block)
		{
			// Start by getting a read lock on the collection itself.
			IDisposable collectionLock = AcquireLock(requestedCollectionLock);

			// Get a read lock on the block and then return it.
			IDisposable blockLock = block.AcquireLock(collectionLock, requestedBlockLock);
			return blockLock;
		}

		/// <summary>
		/// Acquires a lock of the specified type and returns an opaque lock object
		/// that will release the lock when disposed.
		/// 
		/// <code>using (blocks.AcquireLock(RequestLock.Read)) {}</code>
		/// </summary>
		/// <returns>An opaque lock object that will release lock on disposal.</returns>
		public IDisposable AcquireLock(RequestLock requestLock)
		{
			// Acquire the lock based on the requested type.
			IDisposable acquiredLock;

			switch (requestLock)
			{
				case RequestLock.Read:
					acquiredLock = new NestableReadLock(accessLock);
					break;

				case RequestLock.UpgradableRead:
					acquiredLock = new NestableUpgradableReadLock(accessLock);
					break;

				case RequestLock.Write:
					acquiredLock = new NestableWriteLock(accessLock);
					break;

				default:
					throw new InvalidOperationException(
						"Could not acquire lock with unknown type: " + requestLock);
			}

			// Return the resulting lock.
			return acquiredLock;
		}

		/// <summary>
		/// Ensures the minimum blocks inside the collection.
		/// </summary>
		private void EnsureMinimumBlocks()
		{
			if (Count == 0)
			{
				var initialBlock = new Block(this);

				Add(initialBlock);
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="ProjectBlockCollection"/> class.
		/// </summary>
		/// <param name="project">The project.</param>
		public ProjectBlockCollection(Project project)
		{
			// Assign the project so we have an association with the block.
			Project = project;

			// Set up the internal state.
			accessLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);

			// Create the initial block item.
			EnsureMinimumBlocks();
		}

		#endregion

		#region Fields

		private readonly ReaderWriterLockSlim accessLock;

		#endregion
	}
}
