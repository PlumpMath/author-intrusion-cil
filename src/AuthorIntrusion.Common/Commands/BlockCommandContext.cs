// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Blocks;

namespace AuthorIntrusion.Common.Commands
{
	public class BlockCommandContext
	{
		/// <summary>
		/// Contains the project associated with this context.
		/// </summary>
		public Project Project { get; private set; }

		/// <summary>
		/// Contains the blocks associated with the project.
		/// </summary>
		public ProjectBlockCollection Blocks
		{
			get { return Project.Blocks; }
		}

		public BlockCommandContext(Project project)
		{
			Project = project;
		}
	}
}