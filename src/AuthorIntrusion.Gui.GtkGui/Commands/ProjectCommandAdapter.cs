// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System;
using System.Diagnostics;
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
	public abstract class ProjectCommandAdapter:
		ITextEditingCommand<OperationContext>
	{
		#region Properties

		public bool CanUndo
		{
			get { return true; }
		}

		public IBlockCommand Command
		{
			get { return command; }
			set
			{
				command = value;

				// Wrapped commands default to not updating themselves.
				command.UpdateTextPosition = DoTypes.None;
			}
		}

		public bool IsTransient
		{
			get { return false; }
		}

		public DoTypes UpdateTextPosition
		{
			get { return Command.UpdateTextPosition; }
			set
			{
				Debug.WriteLine(
					this + ": Changeing UpdateTextPosition from " + Command.UpdateTextPosition
						+ " to " + value);
				Command.UpdateTextPosition = value;
			}
		}

		public DoTypes UpdateTextSelection { get; set; }

		protected Project Project { get; private set; }

		#endregion

		#region Methods

		public virtual void Do(OperationContext context)
		{
			Action<BlockCommandContext> action = Command.Do;
			PerformCommandAction(context, action);
		}

		public virtual void PostDo(OperationContext context)
		{
		}

		public virtual void PostUndo(OperationContext context)
		{
		}

		public void Redo(OperationContext context)
		{
			Action<BlockCommandContext> action = Command.Redo;
			PerformCommandAction(context, action);
		}

		public virtual void Undo(OperationContext context)
		{
			Action<BlockCommandContext> action = Command.Undo;
			PerformCommandAction(context, action);
		}

		private void PerformCommandAction(
			OperationContext context,
			Action<BlockCommandContext> action)
		{
			// Every command needs a full write lock on the blocks.
			using (Project.Blocks.AcquireLock(RequestLock.Write))
			{
				// Create the context for the block commands.
				var blockContext = new BlockCommandContext(Project);

				// Execute the internal command.
				action(blockContext);

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
			}
		}

		#endregion

		#region Constructors

		protected ProjectCommandAdapter(Project project)
		{
			Project = project;
		}

		#endregion

		#region Fields

		private IBlockCommand command;

		#endregion
	}
}
