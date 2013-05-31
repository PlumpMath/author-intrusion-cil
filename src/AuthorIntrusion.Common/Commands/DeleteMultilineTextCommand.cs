// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Blocks;

namespace AuthorIntrusion.Common.Commands
{
	/// <summary>
	/// Command to delete multiple lines of text from the blocks.
	/// </summary>
	public class DeleteMultilineTextCommand: IBlockCommand
	{
		#region Properties

		public BlockPosition LastPosition { get; private set; }

		public bool IsUndoable
		{
			get { return true; }
		}

		public BlockPosition StartBlockPosition { get; set; }
		public BlockPosition StopBlockPosition { get; set; }

		protected CompositeCommand InverseCommand { get; private set; }

		#endregion

		#region Methods

		public void Do(Project project)
		{
			// We have to clear the undo buffer every time because we'll be creating
			// new blocks.
			InverseCommand.Commands.Clear();
			InverseCommand.LastPosition = StopBlockPosition;

			// Set up our own position.
			LastPosition = StartBlockPosition;

			// Figure out line ranges we'll be deleting text from.
			Block startBlock = project.Blocks[StartBlockPosition.BlockKey];
			Block stopBlock = project.Blocks[StopBlockPosition.BlockKey];

			int startIndex = project.Blocks.IndexOf(startBlock);
			int stopIndex = project.Blocks.IndexOf(stopBlock);

			// Figure out where we'll be replacing lines.
			int startLength = startBlock.Text.Length - StartBlockPosition.TextIndex;
			string stopText = stopBlock.Text.Substring(StopBlockPosition.TextIndex);

			var startReplace = new ReplaceTextCommand(
				StartBlockPosition, startLength, stopText);
			IBlockCommand startInverse = startReplace.GetInverseCommand(project);

			InverseCommand.Commands.Add(startInverse);
			startReplace.Do(project);

			// Go through the remaining lines.
			for (int i = startIndex + 1;
				i <= stopIndex;
				i++)
			{
				var middleDelete =
					new DeleteBlockCommand(project.Blocks[startIndex + 1].BlockKey);
				IBlockCommand middleInverse = middleDelete.GetInverseCommand(project);

				InverseCommand.Commands.Insert(1, middleInverse);
				middleDelete.Do(project);
			}
		}

		public IBlockCommand GetInverseCommand(Project project)
		{
			return InverseCommand;
		}

		#endregion

		#region Constructors

		public DeleteMultilineTextCommand(
			BlockPosition startPosition,
			BlockPosition stopPosition)
		{
			// Save the text for the changes.
			StartBlockPosition = startPosition;
			StopBlockPosition = stopPosition;
			InverseCommand = new CompositeCommand();
		}

		#endregion
	}
}
