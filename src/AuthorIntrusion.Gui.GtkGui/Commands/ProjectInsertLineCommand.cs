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

		public override void PostDo(OperationContext context)
		{
			lineBuffer.RaiseLineInserted(line.Index);
		}

		public override void PostUndo(OperationContext context)
		{
			lineBuffer.RaiseLineDeleted(line.Index);
		}

		#endregion

		#region Constructors

		public ProjectInsertLineCommand(
			ProjectLineBuffer lineBuffer,
			Project project,
			LinePosition line)
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

		private readonly LinePosition line;
		private readonly ProjectLineBuffer lineBuffer;

		#endregion
	}
}
