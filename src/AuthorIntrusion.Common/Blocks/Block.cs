// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System.Diagnostics.Contracts;

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
		public BlockOwnerCollection OwnerCollection { get; private set; }

		/// <summary>
		/// Gets or sets the block that is the organizational parent for this block.
		/// </summary>
		public Block ParentBlock { get; private set; }

		public Project Project
		{
			get { return OwnerCollection.Project; }
		}

		/// <summary>
		/// Gets or sets the text associated with the block.
		/// </summary>
		public string Text
		{
			get { return text; }
			set
			{
				text = value ?? string.Empty;
				version++;
			}
		}

		public int Version
		{
			get { return version; }
		}

		#endregion

		#region Methods

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
			Contract.Assert(OwnerCollection.Project == newBlockType.Supervisor.Project);

			// We only do things if we are changing the block type.
			bool changed = blockType != newBlockType;

			if (changed)
			{
				// Assign the new block type.
				blockType = newBlockType;

				// Fire the events in the block structure supervisor.
				Project.BlockStructures.Update();
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
				ParentBlock = parentBlock;
			}
		}

		public void SetText(string newText)
		{
			Text = newText;
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

		/// <summary>
		/// Initializes a new instance of the <see cref="Block"/> class.
		/// </summary>
		/// <param name="ownerCollection">The ownerCollection.</param>
		public Block(BlockOwnerCollection ownerCollection)
			: this(ownerCollection, ownerCollection.Project.BlockTypes.Paragraph)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Block"/> class.
		/// </summary>
		/// <param name="ownerCollection">The ownerCollection.</param>
		public Block(
			BlockOwnerCollection ownerCollection,
			BlockType initialBlockType)
		{
			BlockKey = BlockKey.GetNext();
			OwnerCollection = ownerCollection;
			blockType = initialBlockType;
			text = string.Empty;
		}

		#endregion

		#region Fields

		private BlockType blockType;
		private string text;
		private volatile int version;

		#endregion
	}
}
