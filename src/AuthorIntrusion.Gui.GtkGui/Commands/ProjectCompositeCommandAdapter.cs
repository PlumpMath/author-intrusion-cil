// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Commands;
using MfGames.Commands;
using MfGames.GtkExt.TextEditor.Models;

namespace AuthorIntrusion.Gui.GtkGui.Commands
{
	public class ProjectCompositeCommandAdapter:
		CompositeCommand<BlockCommandContext>,
		IWrappedCommand
	{
		#region Methods

		public void PostDo(OperationContext context)
		{
		}

		public void PostUndo(OperationContext context)
		{
		}

		protected override void Do(
			IUndoableCommand<BlockCommandContext> command,
			BlockCommandContext context)
		{
			// Perform the base command.
			base.Do(command, context);

			// Handle the post operation for the wrapped command.
			var wrappedCommand = (IWrappedCommand) command;

			wrappedCommand.PostDo(operationContext);
		}

		protected override void Undo(
			IUndoableCommand<BlockCommandContext> command,
			BlockCommandContext context)
		{
			// Handle the base operation.
			base.Undo(command, context);

			// Handle the post operation for the wrapped command.
			var wrappedCommand = (IWrappedCommand) command;

			wrappedCommand.PostUndo(operationContext);
		}

		#endregion

		#region Constructors

		public ProjectCompositeCommandAdapter(
			ProjectCommandController projectCommandController,
			CompositeCommand<OperationContext> composite,
			OperationContext operationContext)
			: base(true, false)
		{
			// Save the operation context so we can call back into the editor.
			this.operationContext = operationContext;

			// Go through the commands and wrap each one.
			foreach (ICommand<OperationContext> command in composite.Commands)
			{
				IWrappedCommand wrappedCommand =
					projectCommandController.WrapCommand(command, operationContext);
				Commands.Add(wrappedCommand);
			}
		}

		#endregion

		#region Fields

		private readonly OperationContext operationContext;

		#endregion
	}
}
