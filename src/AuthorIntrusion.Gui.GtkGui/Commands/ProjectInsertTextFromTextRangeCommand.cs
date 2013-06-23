// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common;
using AuthorIntrusion.Common.Blocks;
using AuthorIntrusion.Common.Blocks.Locking;
using AuthorIntrusion.Common.Commands;
using MfGames.Commands.TextEditing;
using MfGames.GtkExt.TextEditor.Models;

namespace AuthorIntrusion.Gui.GtkGui.Commands
{
	public class ProjectInsertTextFromTextRangeCommand: ProjectCommandAdapter,
		IInsertTextFromTextRangeCommand<OperationContext>
	{
		#region Constructors

		public ProjectInsertTextFromTextRangeCommand(
			Project project,
			TextPosition destinationPosition,
			SingleLineTextRange sourceRange)
			: base(project)
		{
			// We need a specific block for this operation.
			Block destinationBlock;

			using (
				project.Blocks.AcquireBlockLock(
					RequestLock.Read, destinationPosition.Line.Index, out destinationBlock))
			{
				// We also need the source block.
				Block sourceBlock = project.Blocks[(int) sourceRange.Line];
				BlockKey sourceBlockKey = sourceBlock.BlockKey;

				// Create the project command wrapper.
				var destinationBlockPosition = new BlockPosition(
					destinationBlock.BlockKey, destinationPosition.Character);
				var command = new InsertTextFromBlock(
					destinationBlockPosition,
					sourceBlockKey,
					sourceRange.CharacterBegin,
					sourceRange.CharacterEnd);

				// Set the command into the adapter.
				Command = command;
			}
		}

		#endregion
	}
}
