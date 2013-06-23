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
	public class ProjectDeleteLineCommand: ProjectCommandAdapter,
		IDeleteLineCommand<OperationContext>
	{
		#region Methods

		public override void PostDo(OperationContext context)
		{
			lineBuffer.RaiseLineDeleted(line.Index);
		}

		public override void PostUndo(OperationContext context)
		{
			lineBuffer.RaiseLineInserted(line.Index);
		}

		#endregion

		#region Constructors

		public ProjectDeleteLineCommand(
			ProjectLineBuffer lineBuffer,
			Project project,
			Position line)
			: base(project)
		{
			// Save the line for later events.
			this.lineBuffer = lineBuffer;
			this.line = line;

			// Get a lock on the blocks so we can retrieve information.
			using (project.Blocks.AcquireLock(RequestLock.Read))
			{
				// Create the project command wrapper.
				Block block = project.Blocks[(int) line];
				var command = new DeleteBlockCommand(block.BlockKey);

				// Set the command into the adapter.
				Command = command;
			}
		}

		#endregion

		#region Fields

		private readonly Position line;
		private readonly ProjectLineBuffer lineBuffer;

		#endregion
	}
}
