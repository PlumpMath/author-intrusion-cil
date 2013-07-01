// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System;
using AuthorIntrusion.Common.Blocks;

namespace AuthorIntrusion.Common.Events
{
	public class BlockEventArgs: EventArgs
	{
		#region Properties

		public Block Block { get; private set; }

		public ProjectBlockCollection Blocks
		{
			get { return Block.Blocks; }
		}

		public Project Project
		{
			get { return Block.Project; }
		}

		#endregion

		#region Constructors

		public BlockEventArgs(Block block)
		{
			Block = block;
		}

		#endregion
	}
}
