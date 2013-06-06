// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System;
using System.Threading;
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
		/// Acquires a read lock on the entire collection. There may be multiple
		/// readers or a single writer, but not both at the same time. This is
		/// expected to be used in a using() construct as the disposing of the
		/// opaque return value releases the read lock.
		/// 
		/// <code>using (blocks.AcquireReadLock()) {}</code>
		/// </summary>
		/// <returns>An opaque lock object that will release lock on disposal.</returns>
		public IDisposable AcquireReadLock()
		{
			return new NestableReadLock(accessLock);
		}

		/// <summary>
		/// Acquires an upgradable read lock on the entire collection. This is
		/// expected to be used in a using() construct as the disposing of the
		/// opaque return value releases the read lock.
		/// 
		/// <code>using (blocks.AcquireUpgradableReadLock()) {}</code>
		/// </summary>
		/// <returns>An opaque lock object that will release lock on disposal.</returns>
		public IDisposable AcquireUpgradableReadLock()
		{
			return new NestableUpgradableReadLock(accessLock);
		}

		/// <summary>
		/// Acquires a write lock on the entire collection. There may be multiple
		/// readers or a single writer, but not both at the same time. This is
		/// expected to be used in a using() construct as the disposing of the
		/// opaque return value releases the read lock.
		/// 
		/// <code>using (blocks.AcquireWriteLock()) {}</code>
		/// </summary>
		/// <returns>An opaque lock object that will release lock on disposal.</returns>
		public IDisposable AcquireWriteLock()
		{
			return new NestableWriteLock(accessLock);
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
