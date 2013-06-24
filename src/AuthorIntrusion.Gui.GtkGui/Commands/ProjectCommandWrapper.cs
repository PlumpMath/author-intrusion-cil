// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Commands;
using MfGames.Commands;
using MfGames.GtkExt.TextEditor.Models;

namespace AuthorIntrusion.Gui.GtkGui.Commands
{
	public class ProjectCommandWrapper: IWrappedCommand
	{
		#region Properties

		public ProjectCommandAdapter Adapter { get; private set; }

		public bool CanUndo
		{
			get { return command.CanUndo; }
		}

		public bool IsTransient
		{
			get { return command.IsTransient; }
		}

		#endregion

		#region Methods

		public void Do(BlockCommandContext context)
		{
			command.Do(context);
		}

		public void PostDo(OperationContext context)
		{
			Adapter.PostDo(context);
		}

		public void PostUndo(OperationContext context)
		{
			Adapter.PostUndo(context);
		}

		public void Redo(BlockCommandContext context)
		{
			command.Redo(context);
		}

		public void Undo(BlockCommandContext context)
		{
			command.Undo(context);
		}

		#endregion

		#region Constructors

		public ProjectCommandWrapper(
			ProjectCommandAdapter adapter,
			IUndoableCommand<BlockCommandContext> command)
		{
			Adapter = adapter;
			this.command = command;
		}

		#endregion

		#region Fields

		private readonly IUndoableCommand<BlockCommandContext> command;

		#endregion
	}
}
