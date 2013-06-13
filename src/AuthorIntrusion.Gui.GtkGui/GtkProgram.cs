// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System;
using Gtk;

namespace AuthorIntrusion.Gui.GtkGui
{
	/// <summary>
	/// Sets up the Gtk framework and starts up the application.
	/// </summary>
	internal static class GtkProgram
	{
		#region Methods

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		private static void Main(string[] args)
		{
			// Initial the Gtk GUI framework.
			Application.Init("Author Intrusion", ref args);

			// Create a single window and show it.
			var window = new MainWindow();
			window.ShowAll();

			// Start running the application.
			Application.Run();
		}

		#endregion
	}
}
