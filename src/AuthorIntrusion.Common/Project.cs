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
	public class Project: IPropertiesContainer
	{
		#region Properties

		/// <summary>
		/// Gets the block type Supervisor associated with this project.
		/// </summary>
		public BlockTypeSupervisor BlockTypes { get; private set; }

		/// <summary>
		/// Contains all the ordered blocks inside the project.
		/// </summary>
		public ProjectBlockCollection Blocks { get; private set; }

		/// <summary>
		/// Gets the commands supervisor for handling commands, undo, and redo.
		/// </summary>
		public BlockCommandSupervisor Commands { get; private set; }

		/// <summary>
		/// Gets the macros associated with the project.
		/// </summary>
		public ProjectMacros Macros { get; private set; }

		/// <summary>
		/// Gets the plugin supervisor associated with the project.
		/// </summary>
		public PluginSupervisor Plugins { get; private set; }

		/// <summary>
		/// Gets the current state of the processing inside the project.
		/// </summary>
		public ProjectProcessingState ProcessingState { get; private set; }

		/// <summary>
		/// Gets the properties associated with the block.
		/// </summary>
		public PropertiesDictionary Properties { get; private set; }

		/// <summary>
		/// Gets the settings associated with this project.
		/// </summary>
		public ProjectSettings Settings { get; private set; }

		#endregion

		#region Methods

		/// <summary>
		/// Updates the current processing state for the project.
		/// </summary>
		/// <param name="processingState">New processing state for the project.</param>
		public void SetProcessingState(ProjectProcessingState processingState)
		{
			// If we are the same, we don't do anything.
			if (processingState == ProcessingState)
			{
				return;
			}

			// Update the internal state so when we call the update method
			// on the various supervisors, they'll be able to make the
			// appropriate updates.
			ProcessingState = processingState;
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Project"/> class.
		/// </summary>
		public Project(
			ProjectProcessingState initialProcessingState =
				ProjectProcessingState.Interactive)
		{
			// Set up the initial states.
			ProcessingState = initialProcessingState;

			// We need the settings set up first since it may contribute
			// to the loading of other components of the project.
			Settings = new ProjectSettings();
			Properties = new PropertiesDictionary();
			BlockTypes = new BlockTypeSupervisor(this);
			Blocks = new ProjectBlockCollection(this);
			Commands = new BlockCommandSupervisor(this);
			Plugins = new PluginSupervisor(this);
			Macros = new ProjectMacros();
		}

		#endregion
	}
}
