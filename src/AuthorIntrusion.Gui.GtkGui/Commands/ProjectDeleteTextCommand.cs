// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common;
using AuthorIntrusion.Common.Blocks;
using AuthorIntrusion.Common.Blocks.Locking;
using AuthorIntrusion.Common.Commands;
using MfGames.Commands.TextEditing;
using MfGames.GtkExt.TextEditor.Models;

namespace AuthorIntrusion.Gui.GtkGui.Commands
{
	public class ProjectDeleteTextCommand: ProjectCommandAdapter,
		IDeleteTextCommand<OperationContext>
	{
		#region Constructors

		public ProjectDeleteTextCommand(
			Project project,
			SingleLineTextRange range)
			: base(project)
		{
			// We need a specific block for this operation.
			Block block;

			using (
				project.Blocks.AcquireBlockLock(
					RequestLock.Read, range.Line.Index, out block))
			{
				// Create the project command wrapper.
				var position = new BlockPosition(block.BlockKey, range.CharacterBegin);
				var command = new DeleteTextCommand(position, range.CharacterEnd);

				// Set the command into the adapter.
				Command = command;
			}
		}

		#endregion
	}
}
