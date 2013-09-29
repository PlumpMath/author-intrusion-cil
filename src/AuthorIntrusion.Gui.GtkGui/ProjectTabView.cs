// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Events;
using Gtk;

namespace AuthorIntrusion.Gui.GtkGui
{
	/// <summary>
	/// Encapsulates the logic for displaying a project tab view.
	/// </summary>
	public class ProjectTabView: Frame
	{
		#region Methods

		private void OnProjectLoaded(
			object sender,
			ProjectEventArgs e)
		{
			// Remove the child, if we have one.
			if (Child != null)
			{
				Remove(Child);
			}

			// Create a tree model for this project.
			var store = new TreeStore(typeof (string));

			TreeIter iter = store.AppendValues("Project");
			store.AppendValues(iter, "Chapter");

			// Create the view for the tree.
			var treeView = new TreeView
			{
				Model = store,
				HeadersVisible = false,
			};

			treeView.AppendColumn("Name", new CellRendererText(), "text", 0);

			// We need to wrap this in a scroll bar since the list might become
			// too larger.
			var scrolledWindow = new ScrolledWindow();
			scrolledWindow.Add(treeView);
			Add(scrolledWindow);

			// Show our components to the user.
			ShowAll();
		}

		private void OnProjectUnloaded(
			object sender,
			ProjectEventArgs e)
		{
			// Remove the child, if we have one.
			if (Child != null)
			{
				Remove(Child);
			}

			// Add a label to indicate we don't have a loaded project.
			var label = new Label("No Project Loaded");
			Add(label);
		}

		#endregion

		#region Constructors

		public ProjectTabView(ProjectManager projectManager)
		{
			// Save the fields for later use.
			this.projectManager = projectManager;

			// Set the internal elements of this frame.
			BorderWidth = 0;
			Shadow = ShadowType.None;
			ShadowType = ShadowType.None;

			// Hook up to the events for this project.
			projectManager.ProjectLoaded += OnProjectLoaded;
			projectManager.ProjectUnloaded += OnProjectUnloaded;
		}

		#endregion

		#region Fields

		private readonly ProjectManager projectManager;

		#endregion
	}
}
