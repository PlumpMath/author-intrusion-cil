// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Blocks;

namespace AuthorIntrusion.Common.Commands
{
	/// <summary>
	/// Defines the signature for an operation that changes the internal block
	/// structure for the project. Examples of these commands would be inserting or
	/// deleting text, changing block types, or the individual results of auto-correct.
	/// </summary>
	public interface IBlockCommand
	{
		#region Properties

		/// <summary>
		/// Gets the final position in a block after the command has completed. This
		/// is where the cursor will be placed when undoing or redoing, or if an 
		/// immediate edit makes changes to the line.
		/// </summary>
		BlockPosition LastPosition { get; }

		/// <summary>
		/// Gets a value indicating whether this command can be undone. If it cannot,
		/// then any operation before it cannot also be undone since this command would
		/// put it into an unknown state.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is undoable; otherwise, <c>false</c>.
		/// </value>
		bool IsUndoable { get; }

		#endregion

		#region Methods

		/// <summary>
		/// Performs the specific action on the project.
		/// 
		/// In most cases, this should not be called directly by user code. Instead,
		/// using <c>BlockCommandSupervisor.Do</c> is the appropriate function to use
		/// since it handles the undo/redo logic. Calling this directly will put the
		/// system into an unknown state which will cause problems with undo/redo.
		/// </summary>
		/// <param name="project">The project.</param>
		void Do(Project project);

		/// <summary>
		/// Gets the inverse command that is needed to restore the project back to the
		/// state it was in before this operation was performed. Inverse operations
		/// will only be performed directly after the block command is performed.
		/// </summary>
		/// <param name="project">The project to use to determine the current state.</param>
		/// <returns>An IBlockCommand that would undo the operation.</returns>
		IBlockCommand GetInverseCommand(Project project);

		#endregion
	}
}
