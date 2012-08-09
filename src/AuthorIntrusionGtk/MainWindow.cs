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
using System.Diagnostics;
using System.IO;

using Awesomium.Mono;
using Awesomium.Mono.Gtk;

using Gtk;

using StructureMap;

#endregion

namespace AuthorIntrusionGtk
{
	/// <summary>
	/// Main application window for Author Intrusion.
	/// </summary>
	public class MainWindow : Window
	{
		private readonly IContainer container;

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="MainWindow"/> class.
		/// </summary>
		public MainWindow(IContainer container)
			: base("Author Intrusion")
		{
			// Store the member variables.
			if (container == null)
			{
				throw new ArgumentNullException("container");
			}

			this.container = container;

			// Set up the GUI.
			ConfigureGui();
		}

		#endregion

		#region GUI

		#region Setup

		private Statusbar statusbar;
		private WebControl webControl;

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
			var menuBar = new MenuBar();
			// DREM context.ActionManager.Layout.Populate(menuBar, "Main");
			box.PackStart(menuBar, false, false, 0);

			// Set up web core.
			WebCore.Initialize(new WebCoreConfig() { CustomCSS = "::-webkit-scrollbar { }" });

			// Create the primary text control.
			webControl = new WebControl();
			webControl.SelfUpdate = true;
			webControl.BeginLoading += (sender,
										args) => Debug.WriteLine("BeginLoading: " + args);
			webControl.BeginNavigation += (sender,
										   args) => Debug.WriteLine("BeginNavigation: " + args);
			webControl.CursorChanged += (sender,
										 args) => Debug.WriteLine("CursorChanged: " + args);
			webControl.DomReady += (sender,
									args) => Debug.WriteLine("DomReady: " + args);
			webControl.KeyPressEvent += (o,
										 args) => Debug.WriteLine("KeyPressEvent: " + args);
			webControl.KeyReleaseEvent += (o,
										   args) => Debug.WriteLine("KeyReleaseEvent: " + args);
			webControl.LoadCompleted += (sender,
										 args) => Debug.WriteLine("LoadCompleted: " + args);
			box.PackStart(webControl, true, true, 0);

			// Add the status bar
			statusbar = new Statusbar();
			statusbar.Push(0, "Welcome!");
			statusbar.HasResizeGrip = true;
			box.PackStart(statusbar, false, false, 0);
		}

		#endregion

		public WebControl WebControl
		{
			get { return webControl; }
		}

		#region Events

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