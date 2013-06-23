// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Commands;
using MfGames.Commands;
using MfGames.Commands.TextEditing;

namespace AuthorIntrusion.Gui.GtkGui.Commands
{
	public class ProjectCommandWrapper : IUndoableCommand<BlockCommandContext>
	{
		public ProjectCommandAdapter Adapter { get; private set; }

		private readonly IUndoableCommand<BlockCommandContext> command;

		public ProjectCommandWrapper(
			ProjectCommandAdapter adapter,
			IUndoableCommand<BlockCommandContext> command)
		{
			this.Adapter = adapter;
			this.command = command;
		}

		public void Do(BlockCommandContext context)
		{
			command.Do(context);
		}

		public void Redo(BlockCommandContext context)
		{
			command.Redo(context);
		}

		public void Undo(BlockCommandContext context)
		{
			command.Undo(context);
		}

		public bool CanUndo {
			get { return command.CanUndo; }
		}
		public bool IsTransient {
			get { return command.IsTransient; }
		}
	}
}