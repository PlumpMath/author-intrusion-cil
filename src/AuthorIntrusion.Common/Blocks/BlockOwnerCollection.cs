// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System.Threading;

namespace AuthorIntrusion.Common.Blocks
{
	/// <summary>
	/// A block collection that manages ownership of blocks along with processing
	/// of both collection operations (insert, delete) and block operations (insert
	/// text).
	/// </summary>
	public class BlockOwnerCollection: BlockCollection
	{
		#region Properties

		public ReaderWriterLockSlim Lock { get; private set; }

		/// <summary>
		/// Gets the project associated with this collection.
		/// </summary>
		public Project Project { get; private set; }

		#endregion

		#region Methods

		/// <summary>
		/// Finds the index of a given block key.
		/// </summary>
		/// <param name="blockKey">The block key to look it up.</param>
		/// <returns>The index of the position.</returns>
		public int IndexOf(BlockKey blockKey)
		{
			Block block = this[blockKey];
			int index = IndexOf(block);
			return index;
		}

		/// <summary>
		/// Ensures the minimum blocks inside the collection.
		/// </summary>
		private void EnsureMinimumBlocks()
		{
			if (Count == 0)
			{
				var initialBlock = new Block(this)
				{
					BlockType = Project.BlockTypes.Paragraph
				};

				Add(initialBlock);
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="BlockOwnerCollection"/> class.
		/// </summary>
		/// <param name="project">The project.</param>
		public BlockOwnerCollection(Project project)
		{
			// Assign the project so we have an association with the block.
			Project = project;
			Lock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);

			// Create the initial block item.
			EnsureMinimumBlocks();
		}

		#endregion
	}
}
