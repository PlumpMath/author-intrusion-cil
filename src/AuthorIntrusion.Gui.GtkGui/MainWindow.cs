// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System;
using System.IO;
using AuthorIntrusion.Common.Events;
using Gtk;
using MfGames.GtkExt.TextEditor;

namespace AuthorIntrusion.Gui.GtkGui
{
	/// <summary>
	/// Primary interfact window for a tabbed document interface.
	/// </summary>
	public class MainWindow: Window
	{
		#region Methods

		/// <summary>
		/// Creates the GUI from code elements.
		/// </summary>
		private void CreateGui()
		{
			// Hook up the accelerator group.
			accelerators = new AccelGroup();
			AddAccelGroup(accelerators);

			// The main frame has a VBox to arrange all the components. The VBox
			// contains the menu, the primary text editor, and a status bar.
			var vertical = new VBox(false, 0);
			Add(vertical);

			// Create the various components and add them to the vertical box.
			Widget menuBarWidget = CreateGuiMenubar();
			Widget textEditorWidget = CreateGuiEditor();
			Widget statusBarWidget = CreateGuiStatusbar();

			vertical.PackStart(menuBarWidget, false, false, 0);
			vertical.PackStart(textEditorWidget, true, true, 0);
			vertical.PackStart(statusBarWidget, false, false, 0);
		}

		private Widget CreateGuiEditor()
		{
			// Create the editor for the user.
			editorView = new EditorView();
			EditorViewTheme.SetupTheme(editorView.Theme);

			// Wrap the text editor in a scrollbar.
			var scrolledWindow = new ScrolledWindow();
			scrolledWindow.VscrollbarPolicy = PolicyType.Always;
			scrolledWindow.Add(editorView);

			// Create the indicator bar that is 10 px wide.
			indicatorView = new IndicatorView(editorView);
			indicatorView.SetSizeRequest(20, 1);

			var indicatorFrame = new Frame
			{
				BorderWidth = 2,
				ShadowType = ShadowType.None,
				Shadow = ShadowType.None
			};
			indicatorFrame.Add(indicatorView);

			// Add the editor and bar to the current tab.
			var editorBand = new HBox(false, 0);
			editorBand.PackStart(scrolledWindow, true, true, 0);
			editorBand.PackStart(indicatorFrame, false, false, 4);

			// Return the top-most frame.
			return editorBand;
		}

		/// <summary>
		/// Creates the menubar elements of a GUI.
		/// </summary>
		/// <returns></returns>
		private Widget CreateGuiMenubar()
		{
			// Create the menu items we'll be using.
			newMenuItem = new ImageMenuItem(Stock.New, accelerators);
			newMenuItem.Activated += OnProjectMenuNewItem;

			openMenuItem = new ImageMenuItem(Stock.Open, accelerators);
			openMenuItem.Activated += OnProjectMenuOpenItem;

			closeMenuItem = new ImageMenuItem(Stock.Close, accelerators)
			{
				Sensitive = false
			};
			closeMenuItem.Activated += OnProjectMenuCloseItem;

			saveMenuItem = new ImageMenuItem(Stock.Save, accelerators)
			{
				Sensitive = false
			};
			saveMenuItem.Activated += OnProjectMenuSaveItem;

			exitMenuItem = new ImageMenuItem(Stock.Quit, accelerators);
			exitMenuItem.Activated += OnProjectMenuExitItem;

			// Create the project menu.
			var projectMenu = new Menu
			{
				newMenuItem,
				openMenuItem,
				closeMenuItem,
				new SeparatorMenuItem(),
				saveMenuItem,
				new SeparatorMenuItem(),
				exitMenuItem
			};

			var projectMenuItem = new MenuItem("_Project")
			{
				Submenu = projectMenu
			};

			// Create the menu bar and reutrn it.
			var menuBar = new MenuBar
			{
				projectMenuItem
			};

			return menuBar;
		}

		private Widget CreateGuiStatusbar()
		{
			var statusbar = new Statusbar();
			return statusbar;
		}

		private void OnDeleteWindow(
			object o,
			DeleteEventArgs args)
		{
			Application.Quit();
		}

		/// <summary>
		/// Called when a project is loaded.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="ProjectEventArgs"/> instance containing the event data.</param>
		private void OnProjectLoaded(
			object sender,
			ProjectEventArgs e)
		{
			// Set up the line buffer for the loaded project.
			var projectLineBuffer = new ProjectLineBuffer(e.Project, editorView);
			editorView.SetLineBuffer(projectLineBuffer);

			// Update the GUI element.
			UpdateGuiState();
		}

		private void OnProjectMenuCloseItem(
			object sender,
			EventArgs e)
		{
			projectManager.CloseProject();
		}

		private void OnProjectMenuExitItem(
			object sender,
			EventArgs e)
		{
			Application.Quit();
		}

		private void OnProjectMenuNewItem(
			object sender,
			EventArgs e)
		{
		}

		private void OnProjectMenuOpenItem(
			object sender,
			EventArgs e)
		{
			// We need an open file dialog box and use that to select the project.
			var dialog = new FileChooserDialog(
				"Open Author Intrusion Project",
				this,
				FileChooserAction.Open,
				"Cancel",
				ResponseType.Cancel,
				"Open",
				ResponseType.Accept);

			// Set up the filter on the dialog.
			var filter = new FileFilter
			{
				Name = "Project Files"
			};
			filter.AddMimeType("binary/x-author-intrusion");
			filter.AddPattern("*.aiproj");
			dialog.AddFilter(filter);

			// Show the dialog and process the results.
			try
			{
				// Show the dialog and get the button the user selected.
				int results = dialog.Run();

				// If the user accepted a file, then use that to open the file.
				if (results != (int) ResponseType.Accept)
				{
					return;
				}

				// Get the project file and load it.
				var file = new FileInfo(dialog.Filename);

				if (!file.Exists)
				{
					return;
				}

				// Open the project.
				OpenProject(file);
			}
			finally
			{
				// Destroy the dialog the box.
				dialog.Destroy();
			}
		}

		/// <summary>
		/// Opens the given project file.
		/// </summary>
		/// <param name="file">A file object to load.</param>
		public void OpenProject(FileInfo file)
		{
			// Load the project into memory.
			projectManager.OpenProject(file);
		}

		private void OnProjectMenuSaveItem(
			object sender,
			EventArgs e)
		{
			projectManager.SaveProject();
		}

		/// <summary>
		/// Called when a project is unloaded.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="ProjectEventArgs"/> instance containing the event data.</param>
		private void OnProjectUnloaded(
			object sender,
			ProjectEventArgs e)
		{
			// Remove the line buffer.
			editorView.ClearLineBuffer();

			// Update the GUI state elements.
			UpdateGuiState();
		}

		/// <summary>
		/// Updates the state of various components of the GUI.
		/// </summary>
		private void UpdateGuiState()
		{
			saveMenuItem.Sensitive = projectManager.HasLoadedProject;
			closeMenuItem.Sensitive = projectManager.HasLoadedProject;
		}

		#endregion

		#region Constructors

		public MainWindow(ProjectManager projectManager)
			: base("Author Intrusion")
		{
			// Set up the manager to handle project loading and unloading.
			this.projectManager = projectManager;

			projectManager.ProjectLoaded += OnProjectLoaded;
			projectManager.ProjectUnloaded += OnProjectUnloaded;

			// Set up the GUI elements.
			CreateGui();

			// Hook up the window-level events.
			DeleteEvent += OnDeleteWindow;

			// Resize the window to a resonable size.
			SetSizeRequest(512, 512);
		}

		#endregion

		#region Fields

		private AccelGroup accelerators;
		private ImageMenuItem closeMenuItem;
		private EditorView editorView;

		private MenuItem exitMenuItem;
		private IndicatorView indicatorView;

		private MenuItem newMenuItem;
		private MenuItem openMenuItem;
		private readonly ProjectManager projectManager;
		private MenuItem saveMenuItem;

		#endregion
	}
}
