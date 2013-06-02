// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System;
using System.Diagnostics.Contracts;
using System.Linq;
using C5;

namespace AuthorIntrusion.Common.Blocks
{
	/// <summary>
	/// Defines a structural element for the organization of blocks. A block structure
	/// defines the dynamic structure for grouping blocks together to form chapters,
	/// scenes, and other elements that make up the common components of a novel or
	/// story. Each structure is keyed off a specific block type.
	/// </summary>
	public class BlockStructure
	{
		#region Properties

		/// <summary>
		/// Gets or sets the block type that this structure represents.
		/// </summary>
		public BlockType BlockType { get; set; }

		/// <summary>
		/// Gets the nested structures underneath this block. This structure is
		/// ignored if the BlockType.IsStructural is false.
		/// </summary>
		public IList<BlockStructure> ChildStructures { get; private set; }

		/// <summary>
		/// Gets or sets the maximum occurances for this block structure.
		/// </summary>
		public int MaximumOccurances { get; set; }

		/// <summary>
		/// Gets or sets the minimum occurances for this structure.
		/// </summary>
		public int MinimumOccurances { get; set; }

		public BlockStructure ParentStructure { get; set; }

		#endregion

		#region Methods

		/// <summary>
		/// Determines whether this instance contains a child structure of the given block type.
		/// </summary>
		/// <param name="blockType">Type of the block.</param>
		/// <returns>
		///   <c>true</c> if [contains child structure] [the specified block type]; otherwise, <c>false</c>.
		/// </returns>
		public bool ContainsChildStructure(BlockType blockType)
		{
			return
				ChildStructures.Any(childStructure => childStructure.BlockType == blockType);
		}

		/// <summary>
		/// Gets the child structure associated with a given block type.
		/// </summary>
		/// <param name="blockType">Type of the block.</param>
		/// <returns>A child structure of the given type.</returns>
		/// <exception cref="System.IndexOutOfRangeException">Cannot find child block type:  + blockType</exception>
		public BlockStructure GetChildStructure(BlockType blockType)
		{
			foreach (BlockStructure childStructure in
				ChildStructures.Where(
					childStructure => childStructure.BlockType == blockType))
			{
				return childStructure;
			}
			throw new IndexOutOfRangeException(
				"Cannot find child block type: " + blockType);
		}

		private void OnChildInserted(
			object sender,
			ItemAtEventArgs<BlockStructure> e)
		{
			// Make sure we have a sane state.
			Contract.Assert(e.Item.ParentStructure == null);

			// Clear out the parent relationship.
			BlockStructure blockStructure = e.Item;

			blockStructure.ParentStructure = this;
		}

		private void OnChildRemoveAt(
			object sender,
			ItemAtEventArgs<BlockStructure> e)
		{
			// Make sure we have a sane state.
			Contract.Assert(e.Item.ParentStructure == this);

			// Clear out the parent relationship.
			BlockStructure blockStructure = e.Item;

			blockStructure.ParentStructure = null;
		}

		#endregion

		#region Constructors

		public BlockStructure()
		{
			// Set up the default values for a block structure.
			MinimumOccurances = 1;
			MaximumOccurances = Int32.MaxValue;

			// Set up the inner collections.
			ChildStructures = new ArrayList<BlockStructure>();
			ChildStructures.ItemInserted += OnChildInserted;
			ChildStructures.ItemRemovedAt += OnChildRemoveAt;
		}

		#endregion
	}
}
