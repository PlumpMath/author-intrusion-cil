// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Commands;
using MfGames.Commands;
using MfGames.GtkExt.TextEditor.Models;

namespace AuthorIntrusion.Gui.GtkGui.Commands
{
	public interface IWrappedCommand: IUndoableCommand<BlockCommandContext>
	{
		#region Methods

		void PostDo(OperationContext context);

		void PostUndo(OperationContext context);

		#endregion
	}
}
