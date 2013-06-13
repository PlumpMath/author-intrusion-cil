// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Blocks;

namespace AuthorIntrusion.Common.Commands
{
	/// <summary>
	/// Encapsulates a command that works on a single block identified by a key.
	/// </summary>
	public abstract class SingleBlockKeyCommand: BlockKeyCommand
	{
		#region Methods

		public override void Do(Project project)
		{
			using (project.Blocks.AcquireReadLock())
			{
				UnlockedDo(project);
			}
		}

		#endregion

		#region Constructors

		protected SingleBlockKeyCommand(BlockKey blockKey)
			: base(blockKey)
		{
		}

		#endregion
	}
}
