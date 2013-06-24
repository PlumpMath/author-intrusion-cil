// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System;
using System.Diagnostics.Contracts;
using AuthorIntrusion.Common.Blocks;
using MfGames.Commands;
using MfGames.Commands.TextEditing;

namespace AuthorIntrusion.Common.Commands
{
	/// <summary>
	/// Project-based manager class to handle command processing along with the undo
	/// and redo functionality.
	/// </summary>
	public class BlockCommandSupervisor:
		UndoRedoCommandController<BlockCommandContext>
	{
		#region Properties

		/// <summary>
		/// Gets the LastPosition of the last command run, if it has a non-empty
		/// position. Empty positions are ignored.
		/// </summary>
		public BlockPosition LastPosition { get; private set; }

		private Project Project { get; set; }

		#endregion

		#region Methods

		public override void Do(
			ICommand<BlockCommandContext> command,
			BlockCommandContext state)
		{
			// Call the base implementation first.
			base.Do(command, state);

			// If we have a position, set it.
			if (state.Position.HasValue)
			{
				LastPosition = state.Position.Value;
			}
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
			InsertText(new BlockPosition(block.BlockKey, (Position) textIndex), text);
		}

		public override ICommand<BlockCommandContext> Redo(BlockCommandContext state)
		{
			// Call the base implementation first.
			ICommand<BlockCommandContext> command = base.Redo(state);

			// If we have a position, set it.
			if (state.Position.HasValue)
			{
				LastPosition = state.Position.Value;
			}

			// Return the redone command.
			return command;
		}

		public override ICommand<BlockCommandContext> Undo(BlockCommandContext state)
		{
			// Call the base implementation first.
			ICommand<BlockCommandContext> command = base.Undo(state);

			// If we have a position, set it.
			if (state.Position.HasValue)
			{
				LastPosition = state.Position.Value;
			}

			// Return the undone command.
			return command;
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
			var context = new BlockCommandContext(Project);
			Do(command, context);
		}

		#endregion

		#region Constructors

		public BlockCommandSupervisor(Project project)
		{
			// Make sure we have a sane state.
			Contract.Requires<ArgumentNullException>(project != null);

			// Save the member variables so we can use them to perform actions.
			Project = project;
		}

		#endregion
	}
}
