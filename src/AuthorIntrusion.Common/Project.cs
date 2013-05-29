// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Commands;

namespace AuthorIntrusion.Common
{
	/// <summary>
	/// A project encapsulates all of the text, settings, and organization of a
	/// written document (novel, short story).
	/// </summary>
	public class Project
	{
		#region Properties

		/// <summary>
		/// Gets the block type Supervisor associated with this project.
		/// </summary>
		public BlockTypeSupervisor BlockTypeSupervisor { get; private set; }

		/// <summary>
		/// Contains all the ordered blocks inside the project.
		/// </summary>
		public BlockOwnerCollection Blocks { get; private set; }

		/// <summary>
		/// Gets the commands supervisor for handling commands, undo, and redo.
		/// </summary>
		public BlockCommandSupervisor Commands { get; private set; }

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Project"/> class.
		/// </summary>
		public Project()
		{
			BlockTypeSupervisor = new BlockTypeSupervisor(this);
			Blocks = new BlockOwnerCollection(this);
			Commands = new BlockCommandSupervisor(this);
		}

		#endregion
	}
}
