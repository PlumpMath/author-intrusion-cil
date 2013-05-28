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

using System;
using System.Diagnostics;
using System.IO;

using Antlr4.StringTemplate;

namespace AuthorIntrusion.Gui.AwesomiumInterop
{
	/// <summary>
	/// Manages web views and handles the internals for rendering styles and components
	/// for the views.
	/// </summary>
	public class WebViewManager
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="WebViewManager"/> class.
		/// </summary>
		/// <param name="webViews">The web views.</param>
		public WebViewManager(IWebView[] webViews)
		{
			Debug.WriteLine("WebViewManager: " + webViews);

			// Load in the string templates for the known functions and default theme.
			//TemplateGroup functinonsGroup = new TemplateGroupFile();
		}

		#endregion

		#region Environment

		/// <summary>
		/// Gets the location that this plugin resides in.
		/// </summary>
		public static string PluginDirectory
		{
			get {
				// Get the location of the WebViewManager (which is going to be in the plugins
				// directory).
				string location = typeof(WebViewManager).Assembly.Location;

				if (location == null)
					throw new ApplicationException("Cannot support the WebViewManager assembly being loaded dynamically.");

				// Get the directory for the assembly.
				DirectoryInfo directory = Directory.GetParent(location);

				return directory.FullName;
			}
		}


		#endregion

	}
}