// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System;
using System.Diagnostics.Contracts;
using AuthorIntrusion.Common.Commands;
using C5;
using MfGames.Locking;

namespace AuthorIntrusion.Common.Blocks
{
	/// <summary>
	/// The block type supervisor is a manager class responsible for maintaining
	/// the relationship between the various blocks based on their types.
	/// </summary>
	public class BlockStructureSupervisor
	{
		#region Properties

		/// <summary>
		/// Gets the block structure that represents an invalid or out of
		/// organization block.
		/// </summary>
		public BlockStructure InvalidBlockStructure { get; private set; }

		/// <summary>
		/// Gets or sets the root block structure for the entire project.
		/// </summary>
		public BlockStructure RootBlockStructure
		{
			get { return rootBlockStructure; }
			set
			{
				if (value == null)
				{
					throw new NullReferenceException(
						"Cannot assign a null to the RootBlockStructure.");
				}

				rootBlockStructure = value;
			}
		}

		protected Project Project { get; private set; }

		#endregion

		#region Methods

		/// <summary>
		/// Updates the blocks in the project and assign them to block types that fit
		/// the document structure. This will attempt to keep the current block type
		/// of a given block if it can fit into the structure.
		/// </summary>
		public void Update()
		{
			// We need to get a write lock on the block since we'll be making changes
			// to the structure (and therefore the display).
			using (new NestableWriteLock(Project.Blocks.Lock))
			{
				// Start at the beginning with the root block structure and the
				// first block in the list.
				int blockIndex = 0;
				BlockStructure blockStructure = RootBlockStructure;
				var parentBlockTypes = new ArrayList<BlockType>();
				var changeBlocksCommand = new ChangeMultipleBlockTypesCommand();

				Update(blockStructure, ref blockIndex, parentBlockTypes, changeBlocksCommand);

				// If we have blocks that changed, add the deferred operation.
				if (!changeBlocksCommand.Changes.IsEmpty)
				{
					// Perform the action without adding to the undo/redo system.
					changeBlocksCommand.Do(Project);
				}
			}
		}

		/// <summary>
		/// Updates the blocks inside the project, assigning the block type to the
		/// blocks in turn according to the block structure.
		/// </summary>
		/// <param name="blockStructure">The block structure.</param>
		/// <param name="blockIndex">Index of the block.</param>
		/// <param name="parentBlockTypes">The parent block types.</param>
		/// <param name="changeBlocksCommand"></param>
		private void Update(BlockStructure blockStructure,
			ref int blockIndex,
			ICollection<BlockType> parentBlockTypes,
			ChangeMultipleBlockTypesCommand changeBlocksCommand)
		{
			// Go through the remaining blocks inside the project until we get to
			// the end.
			BlockOwnerCollection blocks = Project.Blocks;
			int occurances = 0;

			while (blockIndex < blocks.Count)
			{
				// Grab the next block on the list.
				Block block = blocks[blockIndex];
				BlockType blockType = block.BlockType;

				// At this part of the processing, we are looking for blocks that have
				// either the type of the given structure (which means we process it)
				// or a parent one (which means we'll leave this function).
				if (parentBlockTypes.Contains(blockType))
				{
					// We done, so break out of this function without changing the
					// block index.
					return;
				}

				// Check to see if we are the type of the structure.
				if (blockType != blockStructure.BlockType)
				{
					// See if we are under the minimum occurances.
					if (occurances < blockStructure.MinimumOccurances)
					{
						// Add the block type to the deferred block change list.
						changeBlocksCommand.Changes[block.BlockKey] = blockStructure.BlockType;
					}
					else
					{
						// We have hit the minimum occurances, so break out.
						return;
					}
				}

				// This is the block type we are looking for, so we want to
				// recurse into the structure for the next block.
				blockIndex++;
				occurances++;

				// Loop through all the nested types.
				IList<BlockStructure> childStructures = blockStructure.ChildStructures;

				for (int childIndex = 0;
					childIndex < childStructures.Count;
					childIndex++)
				{
					// Pull out the child structure for the inner loop.
					BlockStructure childStructure = childStructures[childIndex];

					// Figure out the types that would break out of this inner
					// structure. We have to create a new list for this since we
					// don't want to alter the structure for the containing loop or
					// any structure above this one.
					var breakingBlockTypes = new LinkedList<BlockType>();
					breakingBlockTypes.AddAll(parentBlockTypes);

					for (int additionalIndex = childIndex + 1;
						additionalIndex < childStructures.Count;
						additionalIndex++)
					{
						breakingBlockTypes.Add(childStructures[additionalIndex].BlockType);
					}

					// Recurse into this function with the child one.
					Update(childStructure, ref blockIndex, breakingBlockTypes, changeBlocksCommand);
				}

				// Check to see if we hit the maximum occurances. If we have and since
				// we done with the nested structures, then break out of this loop
				// and let the parent structure processing handle whats remaining.
				if (occurances > blockStructure.MaximumOccurances)
				{
					return;
				}
			}
		}

		#endregion

		#region Constructors

		public BlockStructureSupervisor(Project project)
		{
			// Make sure the current state is sane.
			Contract.Assert(project.Blocks != null);
			Contract.Assert(project.BlockTypes != null);

			// Save the members we need for later referencing.
			Project = project;

			// Hook up the events on the blocks collection so we can make alterations
			// and assign block types.
			project.Blocks.ItemInserted += (sender,
				args) => Update();
			project.Blocks.ItemRemovedAt += (sender,
				args) => Update();
			project.Blocks.ItemsAdded += (sender,
				args) => Update();
			project.Blocks.ItemsRemoved += (sender,
				args) => Update();

			// Set up the default structures.
			rootBlockStructure = new BlockStructure
			{
				BlockType = project.BlockTypes.Paragraph
			};
			InvalidBlockStructure = new BlockStructure();
		}

		#endregion

		#region Fields

		private BlockStructure rootBlockStructure;

		#endregion
	}
}
