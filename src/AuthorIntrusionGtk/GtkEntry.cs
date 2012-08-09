#region Copyright and License

// Copyright (c) 2011, Moonfire Games
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

using System.Diagnostics;
using System.IO;

using Awesomium.Mono;

using Gtk;

using Container=StructureMap.Container;

#endregion

namespace AuthorIntrusionGtk
{
	/// <summary>
	/// Main entry point into the Gtk application.
	/// </summary>
	internal static class GtkEntry
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		public static void Main(string[] args)
		{
			// Initialize Gtk.
			Application.Init();

			// Initialize the Author Intrusion manager. We have to use Configure
			// because StructureMap does not automatically find some of our
			// leaf objects (e.g., Actions).
			Container container = new Container();

			container.Configure(
				x => x.Scan(
				     	scanner =>
				     	{
				     		// List the places we are searching for assemblies.
				     		scanner.AssembliesFromApplicationBaseDirectory();
				     		scanner.AssembliesFromPath("Extensions");

				     		// List the common types we need to load.
				     		scanner.AddAllTypesOf<Action>();
				     	}));

			// Get the context and populate it.
			//var context = container.GetInstance<Context>();
			//context.ActionManager = container.GetInstance<GlobalActionManager>();

			// Create the window and show it.
			var webCoreConfig = new WebCoreConfig() { CustomCSS = "::-webkit-scrollbar { color: red; }", AutoUpdatePeriod = 10 };
			WebCore.Initialize(webCoreConfig);

			var mainWindow = container.GetInstance<MainWindow>();
			mainWindow.ShowAll();

			mainWindow.WebControl.LoadURL("file:///C:/Users/dmoonfire/Documents/MfGames/author-intrusion/src/test.html");

			while (mainWindow.WebControl.IsLoadingPage)
			{
				System.Threading.Thread.Sleep(10);
			}

			WebCore.Update();

			// If we have a command-line option that is a file, open it.
			if (args.Length > 0)
			{
				// Find out if the first argument is a file.
				var file = new FileInfo(args[0]);

				if (file.Exists)
				{
					//var inputManager = container.GetInstance<IInputManager>();
					//Document document = inputManager.Read(file);
					//context.Document = document;
				}
			}

			Debug.WriteLine(container.WhatDoIHave());

			Application.Run();
		}
	}
}