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

using System;
using System.Windows.Forms;

using AuthorIntrustionSwf;

#endregion

namespace AuthorIntrusion.Gui.WinFormsUI
{
	/// <summary>
	/// The GUI factory class for creating a standard WinForms-based application.
	/// </summary>
	public class WinFormsGuiFactory : IGuiFactory
	{
		/// <summary>
		/// Gets a value indicating whether WinForms can be used in this
		/// environment.
		/// </summary>
		public bool IsValid
		{
			get { return true; }
		}

		/// <summary>
		/// Gets the priority of the Winforms GUI factory. This will be 1000
		/// on Windows platforms, otherwise 10.
		/// </summary>
		public int Priority
		{
			get
			{
				switch (Environment.OSVersion.Platform)
				{
					case PlatformID.Unix:
					case PlatformID.MacOSX:
						return 10;
					default:
						return 1000;
				}
			}
		}

		/// <summary>
		/// Starts the WinForms GUI interface and manages its lifecycle.
		/// </summary>
		public void Start()
		{
			// Set up the windowing elements used all WinForms.
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			// Create the form and display it.
			Application.Run(new Form1());
		}
	}
}