﻿// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Blocks;

namespace AuthorIntrusion.Common.Commands
{
	public class BlockCommandContext
	{
		#region Properties

		/// <summary>
		/// Contains the blocks associated with the project.
		/// </summary>
		public ProjectBlockCollection Blocks
		{
			get { return Project.Blocks; }
		}

		public BlockPosition? Position { get; set; }

		/// <summary>
		/// Contains the project associated with this context.
		/// </summary>
		public Project Project { get; private set; }

		#endregion

		#region Constructors

		public BlockCommandContext(Project project)
		{
			Project = project;
		}

		#endregion
	}
}
