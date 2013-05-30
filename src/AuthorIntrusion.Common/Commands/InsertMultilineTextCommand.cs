// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System.Diagnostics.Contracts;
using AuthorIntrusion.Common.Blocks;
using C5;

namespace AuthorIntrusion.Common.Commands
{
	/// <summary>
	/// Command to insert multiple lines of text into the blocks.
	/// </summary>
	public class InsertMultilineTextCommand: IBlockCommand
	{
		#region Properties

		public bool IsUndoable
		{
			get { return true; }
		}

		public BlockPosition BlockPosition { get; set; }
		public string Text { get; private set; }

		protected CompositeCommand InverseCommand { get; private set; }

		#endregion

		#region Methods

		public void Do(Project project)
		{
			// We have to clear the undo buffer every time because we'll be creating
			// new blocks.
			InverseCommand.Commands.Clear();

			// Start by breaking apart the lines on the newline.
			string[] lines = Text.Split('\n');

			// Make changes to the first line by creating a command, adding it to the
			// list of commands we need an inverse for, and then performing it.
			Block block = project.Blocks[BlockPosition.BlockKey];
			string remainingText = block.Text.Substring(BlockPosition.TextIndex);
			var deleteFirstCommand = new DeleteTextCommand(
				BlockPosition, block.Text.Length - BlockPosition.TextIndex);
			var insertFirstCommand = new InsertTextCommand(BlockPosition, lines[0]);

			IBlockCommand inverseDeleteFirstCommand =
				deleteFirstCommand.GetInverseCommand(project);
			IBlockCommand inverseInsertFirstCommand =
				insertFirstCommand.GetInverseCommand(project);
			InverseCommand.Commands.Add(inverseInsertFirstCommand);
			InverseCommand.Commands.Add(inverseDeleteFirstCommand);

			// Perform the commands.
			deleteFirstCommand.UnlockedDo(project);
			insertFirstCommand.UnlockedDo(project);

			// Update the final lines text with the remains of the first line.
			lines[lines.Length - 1] += remainingText;

			// For the remaining lines, we need to insert each one in turn.
			if (lines.Length > 1)
			{
				// Go through all the lines in reverse order to insert them.
				int firstBlockIndex = project.Blocks.IndexOf(block);

				for (int i = lines.Length - 1;
					i > 0;
					i--)
				{
					// Insert the line and set its text value.
					var newBlock = new Block(project.Blocks)
					{
						Text = lines[i],
						BlockType = project.BlockTypes.Paragraph,
					};

					project.Blocks.Insert(firstBlockIndex + 1, newBlock);

					// Insert in the reverse operation.
					var deleteCommand = new DeleteBlockCommand(newBlock.BlockKey);

					InverseCommand.Commands.Add(deleteCommand);
				}
			}
		}

		public IBlockCommand GetInverseCommand(Project project)
		{
			return InverseCommand;
		}

		#endregion

		#region Constructors

		public InsertMultilineTextCommand(
			BlockPosition position,
			string text)
		{
			// Make sure we have a sane state.
			Contract.Assert(!text.Contains("\r"));

			// Save the text for the changes.
			BlockPosition = position;
			Text = text;
			InverseCommand = new CompositeCommand();
		}

		#endregion
	}
}
