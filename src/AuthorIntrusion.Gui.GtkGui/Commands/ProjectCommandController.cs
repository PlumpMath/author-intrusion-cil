// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using AuthorIntrusion.Common;
using MfGames.Commands;
using MfGames.Commands.TextEditing;
using MfGames.GtkExt.TextEditor.Models;

namespace AuthorIntrusion.Gui.GtkGui.Commands
{
	/// <summary>
	/// An adapter class that converts the editor view's controller commands into
	/// the Author Intrusion command elements.
	/// </summary>
	public class ProjectCommandController:
		UndoRedoCommandController<OperationContext>,
		ITextEditingCommandController<OperationContext>
	{
		#region Properties

		/// <summary>
		/// Contains the currently loaded project for the controller.
		/// </summary>
		public Project Project { get; set; }

		public ProjectLineBuffer ProjectLineBuffer { get; set; }

		#endregion

		#region Methods

		public IDeleteLineCommand<OperationContext> CreateDeleteLineCommand(
			Position line)
		{
			// Establish our code contracts.
			Contract.Requires<ArgumentNullException>(Project != null);

			// Create the command adapter and return it.
			var command = new ProjectDeleteLineCommand(ProjectLineBuffer, Project, line);
			Debug.WriteLine("CreateDeleteLineCommand: " + line);
			return command;
		}

		public IDeleteTextCommand<OperationContext> CreateDeleteTextCommand(
			SingleLineTextRange range)
		{
			// Establish our code contracts.
			Contract.Requires<ArgumentNullException>(Project != null);

			// Create the command adapter and return it.
			var command = new ProjectDeleteTextCommand(Project, range);
			Debug.WriteLine("CreateDeleteTextCommand: " + range);
			return command;
		}

		public IInsertLineCommand<OperationContext> CreateInsertLineCommand(
			Position line)
		{
			// Establish our code contracts.
			Contract.Requires<ArgumentNullException>(Project != null);

			// Create the command adapter and return it.
			var command = new ProjectInsertLineCommand(ProjectLineBuffer, Project, line);
			Debug.WriteLine("CreateInsertLineCommand: " + line);
			return command;
		}

		public IInsertTextCommand<OperationContext> CreateInsertTextCommand(
			TextPosition textPosition,
			string text)
		{
			// Establish our code contracts.
			Contract.Requires<ArgumentNullException>(Project != null);

			// Create the command adapter and return it.
			var command = new ProjectInsertTextCommand(Project, textPosition, text);
			Debug.WriteLine("CreateInsertTextCommand: " + textPosition + ", " + text);
			return command;
		}

		public IInsertTextFromTextRangeCommand<OperationContext>
			CreateInsertTextFromTextRangeCommand(
			TextPosition destinationPosition,
			SingleLineTextRange sourceRange)
		{
			// Establish our code contracts.
			Contract.Requires<ArgumentNullException>(Project != null);

			// Create the command adapter and return it.
			var command = new ProjectInsertTextFromTextRangeCommand(
				Project, destinationPosition, sourceRange);
			Debug.WriteLine("CreateInsertTextFromTextRangeCommand: " + destinationPosition+ ", " + sourceRange);
			return command;
		}

		#endregion
	}
}
