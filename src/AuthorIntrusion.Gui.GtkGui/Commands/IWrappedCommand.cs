using AuthorIntrusion.Common.Commands;
using MfGames.Commands;
using MfGames.GtkExt.TextEditor.Models;

namespace AuthorIntrusion.Gui.GtkGui.Commands
{
	public interface IWrappedCommand:IUndoableCommand<BlockCommandContext>
	{
		void PostDo(OperationContext context);

		void PostUndo(OperationContext context);
	}
}