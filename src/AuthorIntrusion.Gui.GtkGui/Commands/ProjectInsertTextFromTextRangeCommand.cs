// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common;
using AuthorIntrusion.Common.Blocks;
using AuthorIntrusion.Common.Blocks.Locking;
using AuthorIntrusion.Common.Commands;
using MfGames.Commands.TextEditing;
using MfGames.GtkExt.TextEditor.Models;
using MfGames.GtkExt.TextEditor.Models.Buffers;

namespace AuthorIntrusion.Gui.GtkGui.Commands
{
	public class ProjectInsertTextFromTextRangeCommand: ProjectCommandAdapter,
		IInsertTextFromTextRangeCommand<OperationContext>
	{
		#region Methods

		public override void Do(OperationContext context)
		{
			base.Do(context);

			// We need a read lock on the block so we can retrieve information.
			Block block;
			var blockIndex = (int) destinationPosition.Line;

			using (
				Project.Blocks.AcquireBlockLock(RequestLock.Read, blockIndex, out block))
			{
				int characterIndex = destinationPosition.Character.NormalizeIndex(block.Text);

				var bufferPosition = new BufferPosition(
					(int)destinationPosition.Line, (int) (characterIndex + block.Text.Length));
				context.Results = new LineBufferOperationResults(bufferPosition);
			}
		}

		public override void Undo(OperationContext context)
		{
			base.Undo(context);

			// We need a read lock on the block so we can retrieve information.
			Block block;
			var blockIndex = (int) destinationPosition.Line;

			using (
				Project.Blocks.AcquireBlockLock(RequestLock.Read, blockIndex, out block))
			{
				int characterIndex = destinationPosition.Character.NormalizeIndex(block.Text);

				var bufferPosition = new BufferPosition(
					(int)destinationPosition.Line, (int) (characterIndex + block.Text.Length));
				context.Results = new LineBufferOperationResults(bufferPosition);
			}
		}

		#endregion

		#region Constructors

		public ProjectInsertTextFromTextRangeCommand(
			Project project,
			TextPosition destinationPosition,
			SingleLineTextRange sourceRange)
			: base(project)
		{
			this.destinationPosition = destinationPosition;
			// We need a specific block for this operation.
			Block destinationBlock;

			using (
				project.Blocks.AcquireBlockLock(
					RequestLock.Read, destinationPosition.Line.Index, out destinationBlock))
			{
				// Create the project command wrapper.
				var command = new InsertTextFromIndexedBlock(
					destinationPosition,
					(int) sourceRange.Line,
					sourceRange.CharacterBegin,
					sourceRange.CharacterEnd);

				// Set the command into the adapter.
				Command = command;
			}
		}

		#endregion

		#region Fields

		private readonly TextPosition destinationPosition;

		#endregion
	}
}
