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
	/// Implements a Save As dialog box.
	/// </summary>
	public class SaveDocumentAsDialog : FileChooserDialog
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="SaveDocumentAsDialog"/> class.
		/// </summary>
		/// <param name="parent">The parent.</param>
		/// <param name="outputManager">The output manager.</param>
		public SaveDocumentAsDialog(
			MainWindow parent,
			IOutputManager outputManager)
			: base(
				DialogResources.SaveDocumentAsTitle,
				parent,
				FileChooserAction.Save,
				DialogResources.CancelButtonText,
				ResponseType.Cancel,
				DialogResources.SaveButtonText,
				ResponseType.Accept)
		{
			// Go through all the output formats.
			foreach (IOutputWriter writer in outputManager.Writers)
			{
				// Create a file-specific filter.
				var filter = new FileFilter();

				filter.Name = writer.Name;

				AddFilter(filter);

				// Add the writer's extension and MIME to the filter.
				foreach (string extension in writer.FileExtensions)
				{
					filter.AddPattern("*" + extension);
				}

				foreach (string mimeType in writer.MimeTypes)
				{
					filter.AddMimeType(mimeType);
				}
			}
		}

		#endregion
	}
}