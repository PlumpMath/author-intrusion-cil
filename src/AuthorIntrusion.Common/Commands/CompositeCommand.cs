// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using C5;

namespace AuthorIntrusion.Common.Commands
{
	/// <summary>
	/// Encapsulates multiple commands into a single, potentially reversable, command.
	/// </summary>
	public class CompositeCommand: IBlockCommand
	{
		#region Properties

		public IList<IBlockCommand> Commands { get; private set; }
		public bool IsUndoable { get; private set; }

		#endregion

		#region Methods

		public void Do(Project project)
		{
			// Loop through the commands and perform each one in turn.
			foreach (IBlockCommand command in Commands)
			{
				command.Do(project);
			}
		}

		public IBlockCommand GetInverseCommand(Project project)
		{
			// If we don't have the inverse command, then create them for the first
			// time.
			if (inverseComposite == null)
			{
				// As we loop through the commands, we have to put them into reverse
				// order.
				inverseComposite = new CompositeCommand(IsUndoable);

				foreach (IBlockCommand command in Commands)
				{
					IBlockCommand inverseCommand = command.GetInverseCommand(project);

					inverseComposite.Commands.InsertFirst(inverseCommand);
				}
			}

			// Return the resulting inverse composite command.
			return inverseComposite;
		}

		#endregion

		#region Constructors

		public CompositeCommand(bool isUndoable)
		{
			IsUndoable = isUndoable;
			Commands = new ArrayList<IBlockCommand>();
		}

		#endregion

		#region Fields

		private CompositeCommand inverseComposite;

		#endregion
	}
}
