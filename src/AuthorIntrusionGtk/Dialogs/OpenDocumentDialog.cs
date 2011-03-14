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

using AuthorIntrusion.Contracts.IO;

using AuthorIntrusionGtk.Resources;

using Gtk;

#endregion

namespace AuthorIntrusionGtk.Dialogs
{
	/// <summary>
	/// Implements a Open dialog box that handles the various input types.
	/// </summary>
	public class OpenDocumentDialog : FileChooserDialog
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="OpenDocumentDialog"/> class.
		/// </summary>
		/// <param name="parent">The parent.</param>
		public OpenDocumentDialog(MainWindow parent, IInputManager inputManager)
			: base(
				DialogResources.OpenDocumentDialogTitle,
				parent,
				FileChooserAction.Open,
				DialogResources.CancelButtonText,
				ResponseType.Cancel,
				DialogResources.OpenButtonText,
				ResponseType.Accept)
		{
			// Create a filter of all supported input types.
			var allFilter = new FileFilter();
			allFilter.Name = "All Supported Files";
			AddFilter(allFilter);

			foreach (IInputReader reader in inputManager.Readers)
			{
				// Create a file-specific filter.
				var filter = new FileFilter();

				filter.Name = reader.Name;

				AddFilter(filter);

				// Add the reader's extension and MIME to both filters.
				foreach (string extension in reader.FileExtensions)
				{
					allFilter.AddPattern("*" + extension);
					filter.AddPattern("*" + extension);
				}

				foreach (string mimeType in reader.MimeTypes)
				{
					allFilter.AddMimeType(mimeType);
					filter.AddMimeType(mimeType);
				}
			}
		}

		#endregion
	}
}