// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common;
using AuthorIntrusion.Common.Commands;
using MfGames.Commands.TextEditing;
using MfGames.GtkExt.TextEditor.Models;

namespace AuthorIntrusion.Gui.GtkGui.Commands
{
	public class ProjectInsertTextCommand: ProjectCommandAdapter,
		IInsertTextCommand<OperationContext>
	{
		#region Constructors

		public ProjectInsertTextCommand(
			Project project,
			TextPosition textPosition,
			string text)
			: base(project)
		{
			// Create the project command wrapper.
			var command = new InsertTextCommand(textPosition, text);

			// Set the command into the adapter.
			Command = command;
		}

		#endregion
	}
}
