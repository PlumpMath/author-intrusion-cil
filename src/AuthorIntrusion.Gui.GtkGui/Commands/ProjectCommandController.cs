// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using AuthorIntrusion.Common;
using AuthorIntrusion.Common.Blocks;
using AuthorIntrusion.Common.Blocks.Locking;
using AuthorIntrusion.Common.Commands;
using MfGames.Commands;
using MfGames.Commands.TextEditing;
using MfGames.GtkExt.TextEditor.Models;
using MfGames.GtkExt.TextEditor.Models.Buffers;

namespace AuthorIntrusion.Gui.GtkGui.Commands
{
	/// <summary>
	/// An adapter class that converts the editor view's controller commands into
	/// the Author Intrusion command elements.
	/// </summary>
	public class ProjectCommandController:
		ICommandController<OperationContext>,
		ITextEditingCommandController<OperationContext>
	{
		public void DeferDo(ICommand<OperationContext> command)
		{
			throw new NotImplementedException();
		}

		public void Do(
			ICommand<OperationContext> command,
			OperationContext context)
		{
			// Establish our code contracts.
			Contract.Requires<InvalidOperationException>(command is ProjectCommandAdapter);

			// Pull out the "real" command from the adapter.
			var adapter = command as ProjectCommandAdapter;

			// Every command needs a full write lock on the blocks.
			using(Project.Blocks.AcquireLock(RequestLock.Write))
			{
				// Create the context for the block commands.
				var blockContext = new BlockCommandContext(Project);

				// Execute the internal command.
				Commands.Do(adapter.Command, blockContext);

				// Set the operation context from the block context.
				if(((ITextEditingCommand<OperationContext>) adapter).UpdateTextPosition && blockContext.Position.HasValue)
				{
					// Grab the block position and figure out the index.
					BlockPosition blockPosition = blockContext.Position.Value;
					int blockIndex = Project.Blocks.IndexOf(blockPosition.BlockKey);

					var position = new BufferPosition(blockIndex,blockPosition.TextIndex);

					// Set the context results.
					context.Results = new LineBufferOperationResults(position);
				}
			}
		}

		public void Redo(OperationContext context)
		{
			// Every command needs a full write lock on the blocks.
			using(Project.Blocks.AcquireLock(RequestLock.Write))
			{
				// Create the context for the block commands.
				var blockContext = new BlockCommandContext(Project);

				// Execute the internal command.
				Commands.Redo(blockContext);

				// Set the operation context from the block context.
				// DREM if(((ITextEditingCommand<OperationContext>) adapter).UpdateTextPosition && blockContext.Position.HasValue)
				{
					// Grab the block position and figure out the index.
					BlockPosition blockPosition = blockContext.Position.Value;
					int blockIndex = Project.Blocks.IndexOf(blockPosition.BlockKey);

					var position = new BufferPosition(blockIndex,blockPosition.TextIndex);

					// Set the context results.
					context.Results = new LineBufferOperationResults(position);
				}
			}
		}

		public void Undo(OperationContext context)
		{
			// Every command needs a full write lock on the blocks.
			using(Project.Blocks.AcquireLock(RequestLock.Write))
			{
				// Create the context for the block commands.
				var blockContext = new BlockCommandContext(Project);

				// Execute the internal command.
				Commands.Undo(blockContext);

				// Set the operation context from the block context.
				// DREM if(((ITextEditingCommand<OperationContext>) adapter).UpdateTextPosition && blockContext.Position.HasValue)
				{
					// Grab the block position and figure out the index.
					BlockPosition blockPosition = blockContext.Position.Value;
					int blockIndex = Project.Blocks.IndexOf(blockPosition.BlockKey);

					var position = new BufferPosition(blockIndex,blockPosition.TextIndex);

					// Set the context results.
					context.Results = new LineBufferOperationResults(position);
				}
			}
		}

		public bool CanRedo {
			get { return Commands.CanRedo; }
		}
		public bool CanUndo {
			get { return Commands.CanUndo; }
		}
		public OperationContext State { get; private set; }

		#region Properties

		/// <summary>
		/// Contains the currently loaded project for the controller.
		/// </summary>
		public Project Project { get; set; }

		public BlockCommandSupervisor Commands
		{
			get { return Project.Commands; }
		}

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
			Debug.WriteLine(
				"CreateInsertTextFromTextRangeCommand: " + destinationPosition + ", "
					+ sourceRange);
			return command;
		}

		#endregion
	}
}
