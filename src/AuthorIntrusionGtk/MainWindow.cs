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

using AuthorIntrusion.Contracts;
using AuthorIntrusion.Contracts.IO;
using AuthorIntrusion.Contracts.Matters;

using AuthorIntrusionGtk.Dialogs;
using AuthorIntrusionGtk.Editors;

using Gtk;

using MfGames.GtkExt.Actions;
using MfGames.GtkExt.TextEditor;
using MfGames.GtkExt.TextEditor.Models.Styles;

using StructureMap;

using Container=StructureMap.Container;

#endregion

namespace AuthorIntrusionGtk
{
	/// <summary>
	/// Main application window for Author Intrusion.
	/// </summary>
	public class MainWindow : Window
	{
		private readonly IContainer container;
		private readonly Context context;

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="MainWindow"/> class.
		/// </summary>
		public MainWindow(IContainer container, Context context)
			: base("Author Intrusion")
		{
			// Store the member variables.
			if (container == null)
			{
				throw new ArgumentNullException("container");
			}

			if (context == null)
			{
				throw new ArgumentNullException("context");
			}

			this.container = container;
			this.context = context;

			// Set up the GUI.
			ConfigureGui();

			// Set up the keybindings.
			context.ActionManager.AttachToRootWindow(this);

			// Hook up events to the context.
			context.UnloadedDocument += OnUnloadedDocument;
			context.LoadedDocument += OnLoadedDocument;
		}

		#endregion

		#region GUI

		#region Setup

		private IndicatorView indicatorView;
		private Statusbar statusbar;
		private EditorView editorView;

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
			context.ActionManager.Layout.Populate(menuBar, "Main");
			box.PackStart(menuBar, false, false, 0);

			// Create the primary text control.
			box.PackStart(CreateGuiEditor(), true, true, 0);

			// Add the status bar
			statusbar = new Statusbar();
			statusbar.Push(0, "Welcome!");
			statusbar.HasResizeGrip = true;
			box.PackStart(statusbar, false, false, 0);

			// Set up the initial indicator bar theme.
			// Update the theme with some additional colors.
			Theme theme = editorView.Theme;

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
			editorView = new EditorView();
			ThemeHelper.SetupTheme(editorView.Theme);

			// Wrap the text editor in a scrollbar.
			var scrolledWindow = new ScrolledWindow();
			scrolledWindow.VscrollbarPolicy = PolicyType.Always;
			scrolledWindow.Add(editorView);

			// Create the indicator bar that is 10 px wide.
			indicatorView = new IndicatorView(editorView);
			indicatorView.SetSizeRequest(20, 1);

			// Add the editor and bar to the hbox and return it.
			var hbox = new HBox(false, 0);

			hbox.PackStart(scrolledWindow, true, true, 0);
			hbox.PackStart(indicatorView, false, false, 4);

			return hbox;
		}

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
			var lineBuffer = new DocumentLineBuffer(context.Document);
			editorView.SetLineBuffer(lineBuffer);

			context.Document.ParagraphChanged += OnParagraphChanged;
		}

		private void OnParagraphChanged(object sender,
		                                ParagraphChangedEventArgs e)
		{
			Debug.WriteLine("Paragraph Changed: " + e.Paragraph.GetContents());
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
			context.Document.ParagraphChanged -= OnParagraphChanged;
			// Clear out the buffers on the displays.
			editorView.ClearLineBuffer();
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