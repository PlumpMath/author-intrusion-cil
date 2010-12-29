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

using Gtk;

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
			ConfigureGui();
		}

		#endregion

		#region GUI

		#region Setup

		private Statusbar statusbar;
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

			// Add the menu
			box.PackStart(CreateGuiMenu(), false, false, 0);

			// Create a label for the middle.
			var label = new Label("Hello, World.");
			box.PackStart(label, true, true, 0);

			// Add the status bar
			statusbar = new Statusbar();
			statusbar.Push(0, "Welcome!");
			statusbar.HasResizeGrip = true;
			box.PackStart(statusbar, false, false, 0);
		}

		/// <summary>
		/// Creates the GUI menu and returns it.
		/// </summary>
		/// <returns></returns>
		private Widget CreateGuiMenu()
		{
			// Defines the menu
			const string uiInfo =
				"<ui>" + "  <menubar name='MenuBar'>" + "    <menu action='FileMenu'>" +
				"      <menuitem action='Quit'/>" + "    </menu>" + "  </menubar>" + "</ui>";

			// Set up the actions
			var entries = new[]
			              {
			              	// "File" Menu
			              	new ActionEntry(
			              		"FileMenu", null, "_File", null, null, null),
			              	new ActionEntry(
			              		"Quit",
			              		Stock.Quit,
			              		"_Quit",
			              		"<control>Q",
			              		"Quit",
			              		OnQuitAction),
			              };

			// Build up the actions
			var actions = new ActionGroup("group");
			actions.Add(entries);

			// Create the UI manager and add the various entries and actions
			// into it.
			uiManager = new UIManager();
			uiManager.InsertActionGroup(actions, 0);
			AddAccelGroup(uiManager.AccelGroup);

			// Set up the interfaces from XML
			uiManager.AddUiFromString(uiInfo);
			return uiManager.GetWidget("/MenuBar");
		}

		#endregion

		#region Events

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
		/// Fired when the window is closed.
		/// </summary>
		private static void OnWindowDelete(
			object obj,
			DeleteEventArgs args)
		{
			Application.Quit();
		}

		#endregion

		#endregion
	}
}