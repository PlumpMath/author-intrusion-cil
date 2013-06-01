// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Blocks;
using AuthorIntrusion.Common.Commands;
using AuthorIntrusion.Common.Plugins;
using AuthorIntrusion.Common.Projects;

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
		/// Gets the block structure supervisor which handles the management of block
		/// structure which, in effect, assigns the block types during the editing
		/// processing.
		/// </summary>
		public BlockStructureSupervisor BlockStructures { get; private set; }

		/// <summary>
		/// Gets the block type Supervisor associated with this project.
		/// </summary>
		public BlockTypeSupervisor BlockTypes { get; private set; }

		/// <summary>
		/// Contains all the ordered blocks inside the project.
		/// </summary>
		public BlockOwnerCollection Blocks { get; private set; }

		/// <summary>
		/// Gets the commands supervisor for handling commands, undo, and redo.
		/// </summary>
		public BlockCommandSupervisor Commands { get; private set; }

		/// <summary>
		/// Gets the plugin supervisor associated with the project.
		/// </summary>
		public PluginSupervisor Plugins { get; private set; }

		/// <summary>
		/// Gets the settings associated with this project.
		/// </summary>
		public ProjectSettings Settings { get; private set; }

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Project"/> class.
		/// </summary>
		public Project()
		{
			// We need the settings set up first since it may contribute
			// to the loading of other components of the project.
			Settings = new ProjectSettings();

			BlockTypes = new BlockTypeSupervisor(this);
			Blocks = new BlockOwnerCollection(this);

			// The block structure needs both block types and blocks to be initialized.
			BlockStructures = new BlockStructureSupervisor(this);

			Commands = new BlockCommandSupervisor(this);
			Plugins = new PluginSupervisor(this);
		}

		#endregion
	}
}
