// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Blocks;

namespace AuthorIntrusion.Common.Commands
{
	/// <summary>
	/// Encapsulates a command that is identified by a single block but affects the
	/// entire structure of the document.
	/// </summary>
	public abstract class MultipleBlockKeyCommand: BlockKeyCommand
	{
		#region Methods

		public override void Do(Project project)
		{
			using (project.Blocks.AcquireWriteLock())
			{
				UnlockedDo(project);
			}
		}

		#endregion

		#region Constructors

		protected MultipleBlockKeyCommand(BlockKey blockKey)
			: base(blockKey)
		{
		}

		#endregion
	}
}
