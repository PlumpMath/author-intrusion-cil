// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common;
using AuthorIntrusion.Common.Blocks;
using AuthorIntrusion.Common.Commands;
using MfGames.Commands.TextEditing;
using MfGames.GtkExt.TextEditor.Models;

namespace AuthorIntrusion.Gui.GtkGui.Commands
{
	public class ProjectInsertLineCommand: ProjectCommandAdapter,
		IInsertLineCommand<OperationContext>
	{
		#region Methods

		public override void Do(OperationContext context)
		{
			base.Do(context);
			lineBuffer.RaiseLineInserted(line.Index);
		}

		public override void Undo(OperationContext context)
		{
			base.Undo(context);
			lineBuffer.RaiseLineDeleted(line.Index);
		}

		#endregion

		#region Constructors

		public ProjectInsertLineCommand(
			ProjectLineBuffer lineBuffer,
			Project project,
			Position line)
			: base(project)
		{
			// Save the line buffer for later.
			this.lineBuffer = lineBuffer;
			this.line = line;

			// Create the project command wrapper.
			var block = new Block(project.Blocks);
			var command = new InsertIndexedBlockCommand(line.Index, block);

			// Set the command into the adapter.
			Command = command;
		}

		#endregion

		#region Fields

		private readonly Position line;
		private readonly ProjectLineBuffer lineBuffer;

		#endregion
	}
}
