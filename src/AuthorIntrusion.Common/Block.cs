// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System.Diagnostics.Contracts;

namespace AuthorIntrusion.Common
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
		/// Gets or sets the type of the block.
		/// </summary>
		public BlockType BlockType
		{
			get { return blockType; }
			set
			{
				Contract.Assert(value != null);
				Contract.Assert(OwnerCollection.Project == value.Supervisor.Project);
				blockType = value;
			}
		}

		/// <summary>
		/// Gets the owner collection associated with this block.
		/// </summary>
		public BlockOwnerCollection OwnerCollection { get; private set; }

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

		public void SetText(string text)
		{
			Text = text;
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Block"/> class.
		/// </summary>
		/// <param name="ownerCollection">The ownerCollection.</param>
		public Block(BlockOwnerCollection ownerCollection)
		{
			BlockKey = BlockKey.GetNext();
			OwnerCollection = ownerCollection;
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
