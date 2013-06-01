// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

namespace AuthorIntrusion.Common.Commands
{
	/// <summary>
	/// Internal class to manage an undo/redo command and its plumbing for the
	/// BlockCommandSupervisor.
	/// </summary>
	internal class UndoRedoCommand
	{
		#region Properties

		public IBlockCommand Command { get; set; }
		public IBlockCommand InverseCommand { get; set; }

		#endregion

		#region Methods

		public override string ToString()
		{
			return "UndoRedo " + Command;
		}

		#endregion
	}
}
