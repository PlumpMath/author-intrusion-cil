﻿using AuthorIntrusion.Common.Commands;
using MfGames.Commands;
using MfGames.GtkExt.TextEditor.Models;

namespace AuthorIntrusion.Gui.GtkGui.Commands
{
	public class ProjectCompositeCommandAdapter: CompositeCommand<BlockCommandContext>, IWrappedCommand
	{
		public void PostDo(OperationContext context)
		{
			foreach (IWrappedCommand wrappedCommand in Commands)
			{
				wrappedCommand.PostDo(context);
			}
		}

		public void PostUndo(OperationContext context)
		{
			foreach(IWrappedCommand wrappedCommand in Commands)
			{
				wrappedCommand.PostUndo(context);
			}
		}

		public ProjectCompositeCommandAdapter(
			ProjectCommandController projectCommandController,
			CompositeCommand<OperationContext> composite)
			: base(true, false)
		{
			// Go through the commands and wrap each one.
			bool updatePosition;

			foreach (ICommand<OperationContext> command in composite.Commands)
			{
				var wrappedCommand = projectCommandController.WrapCommand(
					command, out updatePosition);
				Commands.Add(wrappedCommand);
			}
		}
	}
}