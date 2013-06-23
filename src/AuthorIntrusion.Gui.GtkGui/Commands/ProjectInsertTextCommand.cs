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
	public class ProjectInsertTextCommand: ProjectCommandAdapter,
		IInsertTextCommand<OperationContext>
	{
		#region Constructors

		public ProjectInsertTextCommand(
			Project project,
			TextPosition textPosition,
			string text)
			: base(project)
		{
			// We need a specific block for this operation.
			Block block;

			using (
				project.Blocks.AcquireBlockLock(
					RequestLock.Read, textPosition.Line.Index, out block))
			{
				// Create the project command wrapper.
				var position = new BlockPosition(block.BlockKey, textPosition.Character);
				var command = new InsertTextCommand(position, text);

				// Set the command into the adapter.
				Command = command;
			}
		}

		#endregion
	}
}
