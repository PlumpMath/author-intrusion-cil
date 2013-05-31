// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System.Diagnostics.Contracts;
using AuthorIntrusion.Common.Blocks;
using C5;

namespace AuthorIntrusion.Common.Commands
{
	/// <summary>
	/// Project-based manager class to handle command processing along with the undo
	/// and redo functionality.
	/// </summary>
	public class BlockCommandSupervisor
	{
		#region Properties

		/// <summary>
		/// Gets a value indicating if there is a command that can be redone.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance can redo; otherwise, <c>false</c>.
		/// </value>
		public bool CanRedo
		{
			get { return !redoCommands.IsEmpty; }
		}

		/// <summary>
		/// Gets a value indicating whether there is a command that can be undone.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance can undo; otherwise, <c>false</c>.
		/// </value>
		public bool CanUndo
		{
			get { return !undoCommands.IsEmpty; }
		}

		private Project Project { get; set; }

		#endregion

		#region Methods

		/// <summary>
		/// Performs the given block command on the project. This also manages the
		/// undo/redo handling for the command.
		/// </summary>
		/// <param name="blockCommand">The command to perform.</param>
		public void Do(IBlockCommand blockCommand)
		{
			// Create an UndoRedoCommand wrapper to store the state of the command
			// at the point it was first performed.
			var command = new UndoRedoCommand
			{
				Command = blockCommand
			};

			// Determine if this command is undoable or not.
			if (blockCommand.IsUndoable)
			{
				// Grab the inverse command to undo the operation.
				IBlockCommand inverseCommand = blockCommand.GetInverseCommand(Project);

				command.InverseCommand = inverseCommand;
			}
			else
			{
				// Since the command cannot be undone, we need to clear out the undo
				// buffer, which will make sure CanUndo will return false and we can
				// recover that memory.
				undoCommands.Clear();
			}

			// In all cases, we clear out the redo buffer because this is a
			// user-initiated command and redo no longer makes sense.
			redoCommands.Clear();

			// Perform the operation.
			Do(command, false);
		}

		/// <summary>
		/// Helper method to create and perform the InsertText command.
		/// </summary>
		/// <param name="block">The block to insert text.</param>
		/// <param name="textIndex">Index to start the insert.</param>
		/// <param name="text">The text to insert.</param>
		public void InsertText(
			Block block,
			int textIndex,
			string text)
		{
			InsertText(new BlockPosition(block.BlockKey, textIndex), text);
		}

		/// <summary>
		/// Redoes the last command that was undone.
		/// </summary>
		public void Redo()
		{
			// Make sure we're in a known and valid state.
			Contract.Assert(CanRedo);

			// Pull off the first command from the redo buffer and perform it.
			UndoRedoCommand command = redoCommands.RemoveFirst();
			Do(command, false);
		}

		/// <summary>
		/// Performs the inverse operation for the last command.
		/// </summary>
		public void Undo()
		{
			// Make sure we're in a known and valid state.
			Contract.Assert(CanUndo);

			// Pull off the first undo command, get its inverse operation, and
			// perform it.
			UndoRedoCommand command = undoCommands.RemoveFirst();
			Do(command, true);
		}

		/// <summary>
		/// The internal "do" method is what performs the actual work on the project.
		/// It also handles pushing the appropriate command on the correct undo/redo
		/// list.
		/// </summary>
		/// <param name="command">The command.</param>
		/// <param name="useInverse">if set to <c>true</c> [use inverse].</param>
		private void Do(
			UndoRedoCommand command,
			bool useInverse)
		{
			// Figure out which block command we'll be using.
			IBlockCommand blockCommand = useInverse
				? command.InverseCommand
				: command.Command;

			// Perform the action on the system.
			blockCommand.Do(Project);

			// Add the action to the appropriate buffer. This assumes that the undo
			// and redo operations have been properly managed before this method is
			// called. This does not manage the buffers since the undo/redo allows
			// the user to go back and forth between the two lists.
			if (command.Command.IsUndoable)
			{
				if (useInverse)
				{
					redoCommands.Push(command);
				}
				else
				{
					undoCommands.Push(command);
				}
			}
		}

		/// <summary>
		/// Helper method to create and perform the InsertText command.
		/// </summary>
		/// <param name="position">The position to insert the text.</param>
		/// <param name="text">The text to insert.</param>
		private void InsertText(
			BlockPosition position,
			string text)
		{
			var command = new InsertTextCommand(position, text);
			Do(command);
		}

		#endregion

		#region Constructors

		public BlockCommandSupervisor(Project project)
		{
			// Make sure we have a sane state.
			Contract.Assert(project != null);

			// Save the member variables so we can use them to perform actions.
			Project = project;

			// Initialize our internal lists.
			undoCommands = new LinkedList<UndoRedoCommand>();
			redoCommands = new LinkedList<UndoRedoCommand>();
		}

		#endregion

		#region Fields

		private readonly LinkedList<UndoRedoCommand> redoCommands;
		private readonly LinkedList<UndoRedoCommand> undoCommands;

		#endregion

		#region Nested Type: UndoRedoCommand

		private class UndoRedoCommand
		{
			#region Properties

			public IBlockCommand Command { get; set; }
			public IBlockCommand InverseCommand { get; set; }

			#endregion
		}

		#endregion
	}
}
