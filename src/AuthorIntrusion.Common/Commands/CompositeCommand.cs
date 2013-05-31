// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System;
using AuthorIntrusion.Common.Blocks;
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

		public IList<IBlockCommand> InverseCommands
		{
			get { return inverseComposite.Commands; }
		}

		public Func<BlockPosition> InverseLastPositionFunc { get; set; }

		public bool IsUndoable { get; private set; }

		public BlockPosition LastPosition
		{
			get
			{
				if (LastPositionFunc == null)
				{
					return BlockPosition.Empty;
				}

				BlockPosition dyanmicLastPosition = LastPositionFunc();
				return dyanmicLastPosition;
			}
			set { LastPositionFunc = () => value; }
		}

		/// <summary>
		/// Gets or sets the last position dynamic function. If this is null, then
		/// the LastPosition will return BlockPosition.Empty.
		/// </summary>
		public Func<BlockPosition> LastPositionFunc { get; set; }

		#endregion

		#region Methods

		public void Do(Project project)
		{
			// Loop through the commands and perform each one in turn.
			foreach (IBlockCommand command in Commands)
			{
				command.Do(project);
			}

			// If we have an inverse position, use that instead.
			if (LastPositionFunc != null)
			{
				LastPosition = LastPositionFunc();
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

					inverseComposite.LastPositionFunc = () => inverseCommand.LastPosition;
				}

				// If we have a last position function for the inverse, set it.
				if (InverseLastPositionFunc != null)
				{
					inverseComposite.LastPositionFunc = InverseLastPositionFunc;
				}
			}

			// Return the resulting inverse composite command.
			return inverseComposite;
		}

		#endregion

		#region Constructors

		public CompositeCommand(bool isUndoable = true)
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
