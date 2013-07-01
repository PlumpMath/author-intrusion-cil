// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Blocks;

namespace AuthorIntrusion.Common.Commands
{
	public class SplitBlockCommand: InsertMultilineTextCommand
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="SplitBlockCommand"/> class.
		/// </summary>
		/// <param name="position">The position to break the paragraph.</param>
		public SplitBlockCommand(
			ProjectBlockCollection blocks,
			BlockPosition position)
			: base(position, "\n")
		{
		}

		#endregion
	}
}
