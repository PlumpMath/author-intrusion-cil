// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System;
using System.IO;
using AuthorIntrusion.Common;
using AuthorIntrusion.Common.Events;
using AuthorIntrusion.Common.Persistence;
using AuthorIntrusion.Common.Plugins;

namespace AuthorIntrusion
{
	/// <summary>
	/// The project manager is a general manager for projects when used in a GUI
	/// interface. It functions a single point of accessing a currently loaded
	/// project, along with events to report the loading and unloading of projects.
	/// It also manages the entry into project loading and saving.
	/// </summary>
	public class ProjectManager
	{
		#region Properties

		/// <summary>
		/// Gets a value indicating whether this instance has a loaded project.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance has a loaded project; otherwise, <c>false</c>.
		/// </value>
		public bool HasLoadedProject
		{
			get { return Project != null; }
		}

		/// <summary>
		/// Gets the currently loaded project. This will be null if there are no
		/// projects loaded.
		/// </summary>
		public Project Project { get; private set; }

		#endregion

		#region Events

		/// <summary>
		/// Occurs when a project is loaded into the project manager.
		/// </summary>
		public event EventHandler<ProjectEventArgs> ProjectLoaded;

		/// <summary>
		/// Occurs when a project is unloaded from the project manager.
		/// </summary>
		public event EventHandler<ProjectEventArgs> ProjectUnloaded;

		#endregion

		#region Methods

		/// <summary>
		/// Opens the project from the given file and loads it into memory.
		/// </summary>
		/// <param name="projectFile">The project file.</param>
		public void OpenProject(FileInfo projectFile)
		{
			// Get the filesystem plugin.
			var plugin =
				(FilesystemPersistencePlugin)
					PluginManager.Instance.Get("Filesystem Persistence");

			// Load the project and connect it.
			Project project = plugin.ReadProject(projectFile);

			SetProject(project);
		}

		/// <summary>
		/// Sets the project and fires the various events 
		/// </summary>
		/// <param name="project">The project.</param>
		public void SetProject(Project project)
		{
			// If the project is identical to the currently loaded one, we
			// don't have to do anything nor do we want events loaded.
			if (project == Project)
			{
				return;
			}

			// If we already had a project loaded, we need to unload it
			// so the various listeners can handle the unloading (and
			// interrupt it).
			if (Project != null)
			{
				// Disconnect the project from the manager.
				Project unloadedProject = Project;
				Project = null;

				// Raise the associated event.
				RaiseProjectUnloaded(unloadedProject);
			}

			// If we have a new project, then load it and raise the appropriate
			// event.
			if (project != null)
			{
				// Set the new project in the manager.
				Project = project;

				// Raise the loaded event to listeners.
				RaiseProjectLoaded(project);
			}
		}

		/// <summary>
		/// Raises the project loaded event
		/// </summary>
		/// <param name="project">The project.</param>
		protected void RaiseProjectLoaded(Project project)
		{
			EventHandler<ProjectEventArgs> listeners = ProjectLoaded;

			if (listeners != null)
			{
				var args = new ProjectEventArgs(project);
				listeners(this, args);
			}
		}

		/// <summary>
		/// Raises the project is unloaded.
		/// </summary>
		/// <param name="project">The project.</param>
		protected void RaiseProjectUnloaded(Project project)
		{
			EventHandler<ProjectEventArgs> listeners = ProjectUnloaded;

			if (listeners != null)
			{
				var args = new ProjectEventArgs(project);
				listeners(this, args);
			}
		}

		#endregion
	}
}
