// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System;
using Gtk;
using MfGames.GtkExt.TextEditor;
using MfGames.GtkExt.TextEditor.Models;

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
			var editorView = new EditorView();
			editorView.SetLineBuffer(new MemoryLineBuffer());

			// Wrap the text editor in a scrollbar.
			var scrolledWindow = new ScrolledWindow();
			scrolledWindow.VscrollbarPolicy = PolicyType.Always;
			scrolledWindow.Add(editorView);

			//// Create the indicator bar that is 10 px wide.
			//var indicatorView = new IndicatorView(editorView);
			//indicatorView.SetSizeRequest(20,1);

			// Add the editor and bar to the current tab.
			var editorBand = new HBox(false, 0);
			editorBand.PackStart(scrolledWindow, true, true, 0);
			//editorBand.PackStart(indicatorView,false,false,4);

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
			newMenuItem = new MenuItem("_New");
			newMenuItem.Activated += OnProjectMenuNewItem;

			openMenuItem = new MenuItem("_Open");
			openMenuItem.Activated += OnProjectMenuOpenItem;

			saveMenuItem = new MenuItem("_Save");
			saveMenuItem.Activated += OnProjectMenuSaveItem;

			exitMenuItem = new MenuItem("_Quit");
			exitMenuItem.Activated += OnProjectMenuExitItem;

			// Create the project menu.
			var projectMenu = new Menu
			{
				newMenuItem,
				openMenuItem,
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
		}

		private void OnProjectMenuSaveItem(
			object sender,
			EventArgs e)
		{
		}

		#endregion

		#region Constructors

		public MainWindow(ProjectManager projectManager)
			: base("Author Intrusion")
		{
			// Set up the manager to handle project loading and unloading.
			this.projectManager = projectManager;

			// Set up the GUI elements.
			CreateGui();

			// Hook up the window-level events.
			DeleteEvent += OnDeleteWindow;

			// Resize the window to a resonable size.
			SetSizeRequest(512, 512);
		}

		#endregion

		#region Fields

		private MenuItem exitMenuItem;

		private MenuItem newMenuItem;
		private MenuItem openMenuItem;
		private readonly ProjectManager projectManager;
		private MenuItem saveMenuItem;

		#endregion
	}
}
