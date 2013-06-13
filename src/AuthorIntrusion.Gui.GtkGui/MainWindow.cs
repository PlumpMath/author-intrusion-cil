// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

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

		private Widget CreateGuiMenubar()
		{
			var menuBar = new MenuBar();
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

		#endregion

		#region Constructors

		public MainWindow()
			: this("Author Intrusion")
		{
		}

		public MainWindow(string title)
			: base(title)
		{
			// Set up the GUI elements.
			CreateGui();

			// Hook up the window-level events.
			DeleteEvent += OnDeleteWindow;

			// Resize the window to a resonable size.
			SetSizeRequest(512, 512);
		}

		#endregion
	}
}
