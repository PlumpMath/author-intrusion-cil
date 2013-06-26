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
			foreach (IWrappedCommand wrappedCommand in Commands)
			{
				wrappedCommand.PostDo(context);
			}
		}

		public void PostUndo(OperationContext context)
		{
			foreach (IWrappedCommand wrappedCommand in Commands)
			{
				wrappedCommand.PostUndo(context);
			}
		}

		#endregion

		#region Constructors

		public ProjectCompositeCommandAdapter(
			ProjectCommandController projectCommandController,
			CompositeCommand<OperationContext> composite)
			: base(true, false)
		{
			// Go through the commands and wrap each one.
			foreach (ICommand<OperationContext> command in composite.Commands)
			{
				IWrappedCommand wrappedCommand =
					projectCommandController.WrapCommand(command);
				Commands.Add(wrappedCommand);
			}
		}

		#endregion
	}
}
