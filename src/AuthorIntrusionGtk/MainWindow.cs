#region Copyright and License

// Copyright (c) 2005-2011, Moonfire Games
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

#endregion

#region Namespaces

using System;
using System.IO;

using AuthorIntrusion.Contracts;
using AuthorIntrusion.Contracts.IO;

using AuthorIntrusionGtk.Dialogs;

using Gtk;

using MfGames.GtkExt.LineTextEditor;
using MfGames.GtkExt.LineTextEditor.Buffers;
using MfGames.GtkExt.LineTextEditor.Enumerations;
using MfGames.GtkExt.LineTextEditor.Indicators;
using MfGames.GtkExt.LineTextEditor.Interfaces;
using MfGames.GtkExt.LineTextEditor.Visuals;

#endregion

namespace AuthorIntrusionGtk
{
	/// <summary>
	/// Main application window for Author Intrusion.
	/// </summary>
	public class MainWindow : Window
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="MainWindow"/> class.
		/// </summary>
		public MainWindow()
			: base("Author Intrusion")
		{
			// Set up the GUI.
			ConfigureGui();

			// Hook up events to the context.
			Context.UnloadedDocument += OnUnloadedDocument;
			Context.LoadedDocument += OnLoadedDocument;
		}

		#endregion

		#region GUI

		#region Setup

		private LineIndicatorBar lineIndicatorBar;
		private Statusbar statusbar;
		private TextEditor textEditor;
		private UIManager uiManager;

		/// <summary>
		/// Configures the GUI.
		/// </summary>
		private void ConfigureGui()
		{
			// Create a window
			SetDefaultSize(1000, 800);

			// Hook up events.
			DeleteEvent += OnWindowDelete;

			// Create the window frame
			var box = new VBox();
			Add(box);

			// Add the menu on the top.
			box.PackStart(CreateGuiMenu(), false, false, 0);

			// Create the primary text control.
			box.PackStart(CreateGuiEditor(), true, true, 0);

			// Add the status bar
			statusbar = new Statusbar();
			statusbar.Push(0, "Welcome!");
			statusbar.HasResizeGrip = true;
			box.PackStart(statusbar, false, false, 0);

			// Set up the initial indicator bar theme.
			// Update the theme with some additional colors.
			Theme theme = textEditor.Theme;

			theme.IndicatorRenderStyle = IndicatorRenderStyle.Ratio;
			theme.IndicatorPixelHeight = 2;
			theme.IndicatorRatioPixelGap = 1;
		}

		/// <summary>
		/// Creates the GUI editor and related components.
		/// </summary>
		/// <returns></returns>
		private Widget CreateGuiEditor()
		{
			// Create the text editor.
			// TODO Remove this parameter from the MfGames.GtkExt.LineTextEditor.
			textEditor = new TextEditor(null);

			// Wrap the text editor in a scrollbar.
			var scrolledWindow = new ScrolledWindow();
			scrolledWindow.VscrollbarPolicy = PolicyType.Always;
			scrolledWindow.Add(textEditor);

			// Create the indicator bar that is 10 px wide.
			lineIndicatorBar = new LineIndicatorBar(textEditor);
			lineIndicatorBar.SetSizeRequest(20, 1);

			// Add the editor and bar to the hbox and return it.
			var hbox = new HBox(false, 0);

			hbox.PackStart(scrolledWindow, true, true, 0);
			hbox.PackStart(lineIndicatorBar, false, false, 4);

			return hbox;
		}

		/// <summary>
		/// Creates the GUI menu and returns it.
		/// </summary>
		/// <returns></returns>
		private Widget CreateGuiMenu()
		{
			// Build up the actions
			var actions = new ActionGroup("group");
			actions.Add(CreateGuiMenuFile());
			actions.Add(CreateGuiMenuEdit());
			actions.Add(CreateGuiMenuView());
			actions.Add(CreateGuiMenuGo());
			actions.Add(CreateGuiMenuHelp());

			// Create the UI manager and add the various entries and actions
			// into it.
			uiManager = new UIManager();
			uiManager.InsertActionGroup(actions, 0);
			AddAccelGroup(uiManager.AccelGroup);

			// Set up the interfaces from XML
			uiManager.AddUiFromResource("AuthorIntrusionGtk.ui.xml");

			// TODO Disable various items until they are implemented.
			uiManager.GetWidget("/MenuBar/FileMenu/New").Sensitive = false;
			uiManager.GetWidget("/MenuBar/FileMenu/Save").Sensitive = false;
			uiManager.GetWidget("/MenuBar/FileMenu/Properties").Sensitive = false;
			uiManager.GetWidget("/MenuBar/FileMenu/Close").Sensitive = false;

			uiManager.GetWidget("/MenuBar/EditMenu/Cut").Sensitive = false;
			uiManager.GetWidget("/MenuBar/EditMenu/Copy").Sensitive = false;
			uiManager.GetWidget("/MenuBar/EditMenu/Paste").Sensitive = false;
			uiManager.GetWidget("/MenuBar/EditMenu/Preferences").Sensitive = false;

			// Return the top-level menu bar.
			return uiManager.GetWidget("/MenuBar");
		}

		#region Menu Actions

		/// <summary>
		/// Creates the GUI Edit menu entries.
		/// </summary>
		/// <returns></returns>
		private static ActionEntry[] CreateGuiMenuEdit()
		{
			// Set up the actions
			ActionEntry[] entries = new[]
			{
				// "Edit" Menu
				new ActionEntry(
					"EditMenu",
					null,
					"Edit",
					null,
					null,
					null),
				new ActionEntry(
					"Cut",
					Stock.Cut,
					"Cu_t",
					"<control>X",
					"Cuts the current selection.",
					null),
				new ActionEntry(
					"Copy",
					Stock.Copy,
					"_Copy",
					"<control>O",
					"Opens an existing document.",
					null),
				new ActionEntry(
					"Paste",
					Stock.Paste,
					"_Paste",
					"<control>V",
					"Pastes the current selection.",
					null),
				new ActionEntry(
					"Preferences",
					Stock.Preferences,
					"Preference_s",
					null,
					"Edits the application preferences.",
					null),
			};

			return entries;
		}

		/// <summary>
		/// Creates the GUI file menu entries.
		/// </summary>
		/// <returns></returns>
		private ActionEntry[] CreateGuiMenuFile()
		{
			// Set up the actions
			ActionEntry[] entries = new[]
			{
				// "File" Menu
				new ActionEntry(
					"FileMenu",
					null,
					"_File",
					null,
					null,
					null),
				new ActionEntry(
					"New",
					Stock.New,
					"_New",
					"<control>N",
					"Creates a new document.",
					null),
				new ActionEntry(
					"Open",
					Stock.Open,
					"_Open...",
					"<control>O",
					"Opens an existing document.",
					OnFileOpen),
				new ActionEntry(
					"Save",
					Stock.Save,
					"_Save",
					"<control>S",
					"Saves a current document.",
					null),
				new ActionEntry(
					"SaveAs",
					Stock.SaveAs,
					"Save _As...",
					"<control><shift>S",
					"Saves the current document with a new name.",
					OnFileSaveAs),
				new ActionEntry(
					"Properties",
					Stock.Properties,
					"_Properties...",
					null,
					"Edits the properties of the current document.",
					null),
				new ActionEntry(
					"Close",
					Stock.Close,
					"_Close",
					"<control>W",
					"Closes the current document.",
					null),
				new ActionEntry(
					"Quit",
					Stock.Quit,
					"_Quit",
					"<control>Q",
					"Quits the application.",
					OnQuitAction),
			};

			return entries;
		}

		/// <summary>
		/// Creates the GUI Go menu entries.
		/// </summary>
		/// <returns></returns>
		private static ActionEntry[] CreateGuiMenuGo()
		{
			// Set up the actions
			ActionEntry[] entries = new[]
			{
				// "Go" Menu
				new ActionEntry(
					"GoMenu",
					null,
					"_Go",
					null,
					null,
					null),
			};

			return entries;
		}

		/// <summary>
		/// Creates the GUI Help menu entries.
		/// </summary>
		/// <returns></returns>
		private static ActionEntry[] CreateGuiMenuHelp()
		{
			// Set up the actions
			ActionEntry[] entries = new[]
			{
				// "Help" Menu
				new ActionEntry(
					"HelpMenu",
					null,
					"_Help",
					null,
					null,
					null),
			};

			return entries;
		}

		/// <summary>
		/// Creates the GUI View menu entries.
		/// </summary>
		/// <returns></returns>
		private static ActionEntry[] CreateGuiMenuView()
		{
			// Set up the actions
			ActionEntry[] entries = new[]
			{
				// "View" Menu
				new ActionEntry(
					"ViewMenu",
					null,
					"_View",
					null,
					null,
					null),
			};

			return entries;
		}

		#endregion

		#endregion

		#region Events

		/// <summary>
		/// Called when a document is loaded.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="args">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void OnLoadedDocument(
			object sender,
			EventArgs args)
		{
			// Wrap the document in the various line buffers.
			// Create a patterned line buffer and make it read-write.
			ILineBuffer lineBuffer = new MemoryLineBuffer();

			// A markup buffer that highlights keywords and wrap it in one that
			// handles simple mouse selections.
			ILineMarkupBuffer markupBuffer = new UnformattedLineMarkupBuffer(lineBuffer);

			markupBuffer = new SimpleSelectionLineMarkupBuffer(markupBuffer);

			// Provide a simple layout buffer that doesn't do anything.
			ILineLayoutBuffer layoutBuffer =
				new SimpleLineLayoutBuffer(markupBuffer);

			// Finally, wrap it in a cached buffer.
			layoutBuffer = new CachedLineLayoutBuffer(layoutBuffer);

			// Set the buffers on the controls.
			textEditor.LineLayoutBuffer = layoutBuffer;
			lineIndicatorBar.LineIndicatorBuffer = null;
		}

		/// <summary>
		/// Triggers the quit menu.
		/// </summary>
		private static void OnQuitAction(
			object sender,
			EventArgs args)
		{
			Application.Quit();
		}

		/// <summary>
		/// Called when a document is unloaded.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="args">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void OnUnloadedDocument(
			object sender,
			EventArgs args)
		{
			// Clear out the buffers on the displays.
			textEditor.LineLayoutBuffer = null;
			lineIndicatorBar.LineIndicatorBuffer = null;
		}

		/// <summary>
		/// Fired when the window is closed.
		/// </summary>
		private static void OnWindowDelete(
			object obj,
			DeleteEventArgs args)
		{
			Application.Quit();
		}

		#region File Menu

		/// <summary>
		/// Called when the file open item is selected.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="args">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void OnFileOpen(
			object sender,
			EventArgs args)
		{
			var dialog = new OpenDocumentDialog(this);

			try
			{
				// If the user cancelled, then just break out.
				if (dialog.Run() != (int) ResponseType.Accept)
				{
					return;
				}

				// The user accepted it, so attempt to parse the document.
				IInputManager inputManager = Context.Manager.InputManager;
				var file = new FileInfo(dialog.Filename);
				Document document = inputManager.Read(file);

				Context.Document = document;
			}
			finally
			{
				dialog.Destroy();
			}
		}

		/// <summary>
		/// Called when the File's Save As is selected.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="args">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void OnFileSaveAs(
			object sender,
			EventArgs args)
		{
			var dialog = new SaveDocumentAsDialog(this);

			try
			{
				// If the user cancelled, then just break out.
				if (dialog.Run() != (int) ResponseType.Accept)
				{
					return;
				}

				// The user accepted it, so attempt to parse the document.
				IOutputManager outputManager = Context.Manager.OutputManager;
				var file = new FileInfo(dialog.Filename);

				outputManager.Write(file, Context.Document);
			}
			finally
			{
				dialog.Destroy();
			}
		}

		#endregion

		#endregion

		#endregion
	}
}