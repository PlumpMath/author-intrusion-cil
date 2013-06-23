// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System;
using System.Diagnostics.Contracts;
using System.Threading;
using AuthorIntrusion.Common.Blocks.Locking;
using C5;

namespace AuthorIntrusion.Common.Blocks
{
	/// <summary>
	/// A block is the primary structural element inside a ownerCollection. It
	/// represents various paragraphs (normal, epigraphs) as well as some
	/// organizational units (chapters, scenes).
	/// </summary>
	public class Block
	{
		#region Properties

		public BlockKey BlockKey { get; private set; }

		/// <summary>
		/// Gets the block structure associated with this block.
		/// </summary>
		public BlockStructure BlockStructure { get; private set; }

		/// <summary>
		/// Gets or sets the type of the block.
		/// </summary>
		public BlockType BlockType
		{
			get { return blockType; }
		}

		/// <summary>
		/// Gets the owner collection associated with this block.
		/// </summary>
		public ProjectBlockCollection Blocks { get; private set; }

		public bool IsWriteLockHeld
		{
			get { return accessLock.IsWriteLockHeld; }
		}

		/// <summary>
		/// Gets or sets the block that is the organizational parent for this block.
		/// </summary>
		public Block ParentBlock { get; private set; }

		public Project Project
		{
			get { return Blocks.Project; }
		}

		/// <summary>
		/// Gets the properties associated with the block.
		/// </summary>
		public BlockPropertyDictionary Properties { get; private set; }

		/// <summary>
		/// Gets or sets the text associated with the block.
		/// </summary>
		public string Text
		{
			get { return text; }
		}

		public TextSpanCollection TextSpans { get; private set; }

		public int Version
		{
			get { return version; }
		}

		#endregion

		#region Methods

		public IDisposable AcquireBlockLock(RequestLock requestedBlockLock)
		{
			IDisposable acquiredLock = Blocks.AcquireBlockLock(requestedBlockLock, this);
			return acquiredLock;
		}

		/// <summary>
		/// Acquires a lock on a block while using the given opaque lock object for
		/// the collection lock. When the block's lock is disposed, so will the
		/// collection lock.
		/// 
		/// <code>using (blocks.AcquireLock(accessLock)) {}</code>
		/// </summary>
		/// <returns>An opaque lock object that will release lock on disposal.</returns>
		public IDisposable AcquireLock(
			IDisposable collectionLock,
			RequestLock requestedLock)
		{
			return new BlockLock(collectionLock, accessLock, requestedLock);
		}

		public IList<Block> GetBlockAndParents()
		{
			var blocks = new ArrayList<Block>();
			Block block = this;

			while (block != null)
			{
				blocks.Add(block);
				block = block.ParentBlock;
			}

			return blocks;
		}

		/// <summary>
		/// Determines whether the specified block version is stale (the version had
		/// changed compared to the supplied version).
		/// </summary>
		/// <param name="blockVersion">The block version.</param>
		/// <returns>
		///   <c>true</c> if the specified block version is stale; otherwise, <c>false</c>.
		/// </returns>
		public bool IsStale(int blockVersion)
		{
			// Ensure we have a lock in effect.
			Contract.Assert(
				accessLock.IsReadLockHeld || accessLock.IsUpgradeableReadLockHeld
					|| accessLock.IsWriteLockHeld);

			// Determine if we have the same version.
			return version != blockVersion;
		}

		/// <summary>
		/// Sets the block structure and fire the appropriate events to listeners.
		/// If the block structure has not changed, then no events will be fired.
		/// </summary>
		/// <param name="blockStructure">The block structure.</param>
		/// <remarks>
		/// This is typically managed by the BlockStructureSupervisor.
		/// </remarks>
		public void SetBlockStructure(BlockStructure blockStructure)

		{
			bool changed = BlockStructure != blockStructure;

			if (changed)
			{
				BlockStructure = blockStructure;
			}
		}

		/// <summary>
		/// Sets the type of the block and fires the events to update the internal
		/// structure. If the block type is identical, no events are fired.
		/// </summary>
		/// <param name="newBlockType">New type of the block.</param>
		public void SetBlockType(BlockType newBlockType)
		{
			// Make sure we have a sane state.
			Contract.Assert(newBlockType != null);
			Contract.Assert(Blocks.Project == newBlockType.Supervisor.Project);

			// We only do things if we are changing the block type.
			bool changed = blockType != newBlockType;

			if (changed)
			{
				// Assign the new block type.
				BlockType oldBlockType = blockType;

				blockType = newBlockType;

				// Raise an event that the block type had changed. This is done
				// before the plugins are called because they may make additional
				// changes and we want to avoid recursion.
				Project.Blocks.RaiseBlockTypeChanged(this);

				// Fire the events in the block structure supervisor.
				Project.BlockStructures.Update();
				Project.Plugins.ChangeBlockType(this, oldBlockType);
			}
		}

		/// <summary>
		/// Sets the parent block and fire the appropriate events to indicate the change.
		/// If the block is identical, then no events will be fired.
		/// </summary>
		/// <param name="parentBlock">The parent block.</param>
		/// <remarks>
		/// This is typically managed by the BlockStructureSupervisor.
		/// </remarks>
		public void SetParentBlock(Block parentBlock)
		{
			bool changed = ParentBlock != parentBlock;

			if (changed)
			{
				// Keep track of the old block for later and then change the block's
				// parent.
				Block oldParentBlock = ParentBlock;
				ParentBlock = parentBlock;

				// Allow the plugin manager to handle any alterations for block
				// parents.
				Project.Plugins.ChangeBlockParent(this, oldParentBlock);
			}
		}

		public void SetText(string newText)
		{
			// Verify that we have a write lock on this block.
			Contract.Requires<InvalidOperationException>(IsWriteLockHeld);

			// TODO: This is disabled because inserting new lines doesn't properly cause events to be fired.
			//// If nothing changed, then we don't have to do anything.
			//if (newText == Text)
			//{
			//	return;
			//}

			// Update the text and bump up the version of this block.
			text = newText ?? string.Empty;
			version++;

			// Raise an event that we changed the text. We do this before processing
			// the project plugins because the immediate editors may make additional
			// changes that will also raise change events.
			Project.Blocks.RaiseBlockTextChanged(this);

			// Trigger the events for any listening plugins.
			Project.Plugins.ProcessBlockAnalysis(this);
		}

		public override string ToString()
		{
			// Figure out a trimmed version of the text.
			string trimmedText = text.Length > 20
				? text.Substring(0, 17) + "..."
				: text;

			// Return a formatted version of the block.
			return string.Format("{0} {1}: {2}", BlockKey, BlockType, trimmedText);
		}

		#endregion

		#region Constructors

		public Block(ProjectBlockCollection blocks)
			: this(blocks, blocks.Project.BlockTypes.Paragraph, string.Empty)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Block" /> class.
		/// </summary>
		/// <param name="blocks">The ownerCollection.</param>
		/// <param name="initialBlockType">Initial type of the block.</param>
		/// <param name="text">The text.</param>
		public Block(
			ProjectBlockCollection blocks,
			BlockType initialBlockType,
			string text = "")
		{
			BlockKey = BlockKey.GetNext();
			Blocks = blocks;
			blockType = initialBlockType;
			this.text = text;
			Properties = new BlockPropertyDictionary();
			TextSpans = new TextSpanCollection();
			accessLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
		}

		#endregion

		#region Fields

		private readonly ReaderWriterLockSlim accessLock;

		private BlockType blockType;
		private string text;
		private volatile int version;

		#endregion
	}
}
