// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System;
using System.Collections.Generic;
using System.Threading;
using AuthorIntrusion.Common.Blocks.Locking;
using AuthorIntrusion.Common.Plugins;

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
			return AcquireBlockLock(RequestLock.Read, requestedBlockLock);
		}

		public IDisposable AcquireBlockLock(
			RequestLock requestedCollectionLock,
			RequestLock requestedBlockLock)
		{
			IDisposable acquiredLock = Blocks.AcquireBlockLock(
				requestedCollectionLock, requestedBlockLock, this);
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

		/// <summary>
		/// Adds a flag that a plugin has performed its analysis on the block.
		/// </summary>
		/// <param name="plugin">The plugin.</param>
		public void AddAnalysis(IBlockAnalyzerProjectPlugin plugin)
		{
			using (AcquireBlockLock(RequestLock.Write))
			{
				previouslyAnalyzedPlugins.Add(plugin);
			}
		}

		/// <summary>
		/// Clears the analysis state of a block to indicate that all analysis
		/// needs to be completed on the task.
		/// </summary>
		public void ClearAnalysis()
		{
			using (AcquireBlockLock(RequestLock.Write))
			{
				previouslyAnalyzedPlugins.Clear();
			}
		}

		/// <summary>
		/// Clears the analysis for a single plugin.
		/// </summary>
		/// <param name="plugin">The plugin.</param>
		public void ClearAnalysis(IBlockAnalyzerProjectPlugin plugin)
		{
			using (AcquireBlockLock(RequestLock.Write))
			{
				previouslyAnalyzedPlugins.Remove(plugin);
			}
		}

		/// <summary>
		/// Retrieves a snapshot of the current block analysis on the block.
		/// </summary>
		/// <returns></returns>
		public HashSet<IBlockAnalyzerProjectPlugin> GetAnalysis()
		{
			using (AcquireBlockLock(RequestLock.Read))
			{
				var results =
					new HashSet<IBlockAnalyzerProjectPlugin>(previouslyAnalyzedPlugins);
				return results;
			}
		}

		public IList<Block> GetBlockAndParents()
		{
			var blocks = new List<Block>();
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
			if (!accessLock.IsReadLockHeld
				&& !accessLock.IsUpgradeableReadLockHeld
				&& !accessLock.IsWriteLockHeld)
			{
				throw new InvalidOperationException(
					"Cannot check block status without a read, upgradable read, or write lock on the block.");
			}

			// Determine if we have the same version.
			return version != blockVersion;
		}

		/// <summary>
		/// Indicates that the text spans of a block have changed significantly.
		/// </summary>
		public void RaiseTextSpansChanged()
		{
			Project.Blocks.RaiseTextSpansChanged(this);
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
			if (newBlockType == null)
			{
				throw new ArgumentNullException("newBlockType");
			}

			if (Blocks.Project != newBlockType.Supervisor.Project)
			{
				throw new InvalidOperationException(
					"Cannot assign a block type with a different Project than the block's Project.");
			}

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
				Project.Blocks.RaiseBlockTypeChanged(this, oldBlockType);
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
				Blocks.RaiseBlockParentChanged(this, oldParentBlock);
			}
		}

		public void SetText(string newText)
		{
			// Verify that we have a write lock on this block.
			if (!IsWriteLockHeld)
			{
				throw new InvalidOperationException(
					"Cannot use SetText without having a write lock on the block.");
			}

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
			ClearAnalysis();
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
			previouslyAnalyzedPlugins = new HashSet<IBlockAnalyzerProjectPlugin>();
		}

		#endregion

		#region Fields

		private readonly ReaderWriterLockSlim accessLock;

		private BlockType blockType;

		/// <summary>
		/// Contains the set of block analyzers that have previously processed
		/// this block.
		/// </summary>
		private readonly HashSet<IBlockAnalyzerProjectPlugin>
			previouslyAnalyzedPlugins;

		private string text;
		private volatile int version;

		#endregion
	}
}
