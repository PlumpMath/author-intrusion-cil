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

using AuthorIntrusion;
using AuthorIntrusion.Contracts;

#endregion

namespace AuthorIntrusionGtk
{
	/// <summary>
	/// A static class for holding the various components of the system for
	/// the Gtk client.
	/// </summary>
	public static class Context
	{
		#region Common

		/// <summary>
		/// Gets or sets the Author Intrusion manager.
		/// </summary>
		/// <value>The manager.</value>
		public static Manager Manager { get; set; }

		#endregion

		#region Document

		private static Document document;

		/// <summary>
		/// Gets or sets the current loaded document.
		/// </summary>
		/// <value>The document.</value>
		public static Document Document
		{
			[DebuggerStepThrough]
			get { return document; }
			set
			{
				// Check to see if we already have a document.
				if (document != null)
				{
					FireUnloadedDocument();
				}

				// Set the new document.
				document = value;

				// Raise the event that we have a new document.
				if (document != null)
				{
					FireLoadedDocument();
				}
			}
		}

		/// <summary>
		/// Fires the loaded document event.
		/// </summary>
		private static void FireLoadedDocument()
		{
			var listeners = LoadedDocument;

			if (listeners != null)
			{
				listeners(Document, EventArgs.Empty);
			}
		}

		/// <summary>
		/// Fires the unloaded document event.
		/// </summary>
		private static void FireUnloadedDocument()
		{
			var listeners = UnloadedDocument;

			if (listeners != null)
			{
				listeners(Document, EventArgs.Empty);
			}
		}

		/// <summary>
		/// Occurs when the current document is loaded.
		/// </summary>
		public static event EventHandler LoadedDocument;

		/// <summary>
		/// Occurs when a document is unloaded from memory.
		/// </summary>
		public static event EventHandler UnloadedDocument;

		#endregion
	}
}