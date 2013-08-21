// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System;
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
	public class ProjectCommandController: ICommandController<OperationContext>,
		ITextEditingCommandController<OperationContext>
	{
		#region Properties

		public bool CanRedo
		{
			get { return Commands.CanRedo; }
		}

		public bool CanUndo
		{
			get { return Commands.CanUndo; }
		}

		public BlockCommandSupervisor Commands
		{
			get { return Project.Commands; }
		}

		/// <summary>
		/// Contains the currently loaded project for the controller.
		/// </summary>
		public Project Project { get; set; }

		public ProjectLineBuffer ProjectLineBuffer { get; set; }
		public OperationContext State { get; private set; }

		#endregion

		#region Methods

		public IDeleteLineCommand<OperationContext> CreateDeleteLineCommand(
			LinePosition line)
		{
			// Create the command adapter and return it.
			var command = new ProjectDeleteLineCommand(ProjectLineBuffer, Project, line);
			//Debug.WriteLine("CreateDeleteLineCommand: " + line);
			return command;
		}

		public IDeleteTextCommand<OperationContext> CreateDeleteTextCommand(
			SingleLineTextRange range)
		{
			// Create the command adapter and return it.
			var command = new ProjectDeleteTextCommand(Project, range);
			//Debug.WriteLine("CreateDeleteTextCommand: " + range);
			return command;
		}

		public IInsertLineCommand<OperationContext> CreateInsertLineCommand(
			LinePosition line)
		{
			// Create the command adapter and return it.
			var command = new ProjectInsertLineCommand(ProjectLineBuffer, Project, line);
			//Debug.WriteLine("CreateInsertLineCommand: " + line);
			return command;
		}

		public IInsertTextCommand<OperationContext> CreateInsertTextCommand(
			TextPosition textPosition,
			string text)
		{
			// Create the command adapter and return it.
			var command = new ProjectInsertTextCommand(Project, textPosition, text);
			//Debug.WriteLine("CreateInsertTextCommand: " + textPosition + ", " + text);
			return command;
		}

		public IInsertTextFromTextRangeCommand<OperationContext>
			CreateInsertTextFromTextRangeCommand(
			TextPosition destinationPosition,
			SingleLineTextRange sourceRange)
		{
			// Create the command adapter and return it.
			var command = new ProjectInsertTextFromTextRangeCommand(
				Project, destinationPosition, sourceRange);
			//Debug.WriteLine(
			//	"CreateInsertTextFromTextRangeCommand: " + destinationPosition + ", "
			//		+ sourceRange);
			return command;
		}

		public void DeferDo(ICommand<OperationContext> command)
		{
			throw new NotImplementedException();
		}

		public void Do(
			ICommand<OperationContext> command,
			OperationContext context)
		{
			// Every command needs a full write lock on the blocks.
			using (Project.Blocks.AcquireLock(RequestLock.Write))
			{
				// Create the context for the block commands.
				var blockContext = new BlockCommandContext(Project);
				Block currentBlock = Project.Blocks[(int) context.Position.Line];
				blockContext.Position = new BlockPosition(
					currentBlock, context.Position.Character);

				// Wrap the command with our wrappers.
				IWrappedCommand wrappedCommand = WrapCommand(command, context);

				Project.Commands.Do(wrappedCommand, blockContext);

				// Set the operation context from the block context.
				if (blockContext.Position.HasValue)
				{
					// Grab the block position and figure out the index.
					BlockPosition blockPosition = blockContext.Position.Value;
					int blockIndex = Project.Blocks.IndexOf(blockPosition.BlockKey);

					var position = new BufferPosition(
						blockIndex, (int) blockPosition.TextIndex);

					// Set the context results.
					context.Results = new LineBufferOperationResults(position);
				}

				// Make sure we process our wrapped command.
				wrappedCommand.PostDo(context);
			}
		}

		public ICommand<OperationContext> Redo(OperationContext context)
		{
			// Every command needs a full write lock on the blocks.
			using (Project.Blocks.AcquireLock(RequestLock.Write))
			{
				// Create the context for the block commands.
				var blockContext = new BlockCommandContext(Project);

				// Execute the internal command.
				ICommand<BlockCommandContext> command = Commands.Redo(blockContext);

				// Set the operation context from the block context.
				if (blockContext.Position.HasValue)
				{
					// Grab the block position and figure out the index.
					BlockPosition blockPosition = blockContext.Position.Value;
					int blockIndex = Project.Blocks.IndexOf(blockPosition.BlockKey);

					var position = new BufferPosition(
						blockIndex, (int) blockPosition.TextIndex);

					// Set the context results.
					context.Results = new LineBufferOperationResults(position);
				}

				// See if we have a wrapped command, then do the post do.
				var wrapped = command as IWrappedCommand;

				if (wrapped != null)
				{
					wrapped.PostDo(context);
				}
			}

			// Always return null for now.
			return null;
		}

		public ICommand<OperationContext> Undo(OperationContext context)
		{
			// Every command needs a full write lock on the blocks.
			using (Project.Blocks.AcquireLock(RequestLock.Write))
			{
				// Create the context for the block commands.
				var blockContext = new BlockCommandContext(Project);

				// Execute the internal command.
				ICommand<BlockCommandContext> command = Commands.Undo(blockContext);

				// Set the operation context from the block context.
				if (blockContext.Position.HasValue)
				{
					// Grab the block position and figure out the index.
					BlockPosition blockPosition = blockContext.Position.Value;
					int blockIndex = Project.Blocks.IndexOf(blockPosition.BlockKey);

					var position = new BufferPosition(
						blockIndex, (int) blockPosition.TextIndex);

					// Set the context results.
					context.Results = new LineBufferOperationResults(position);
				}

				// See if we have a wrapped command, then do the post do.
				var wrapped = command as IWrappedCommand;

				if (wrapped != null)
				{
					wrapped.PostUndo(context);
				}
			}

			return null;
		}

		public IWrappedCommand WrapCommand(ICommand<OperationContext> command,
			OperationContext operationContext)
		{
			// If the command is a ProjectCommandAdapter, then we want to wrap the
			// individual commands.
			var adapter = command as ProjectCommandAdapter;

			if (adapter != null)
			{
				// Implement the commands in the wrapper.
				var wrappedCommand = new ProjectCommandWrapper(adapter, adapter.Command);
				return wrappedCommand;
			}

			// If we have a composite command, we want to wrap it in a custom
			// composite of our own.
			var composite = command as CompositeCommand<OperationContext>;

			if (composite != null)
			{
				var wrappedCompositeCommand = new ProjectCompositeCommandAdapter(
					this, composite, operationContext);
				return wrappedCompositeCommand;
			}

			// If we got this far, we have an invalid state.
			throw new InvalidOperationException("Cannot wrap a command " + command);
		}

		#endregion
	}
}
