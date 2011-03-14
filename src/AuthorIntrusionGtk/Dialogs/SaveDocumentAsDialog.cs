using AuthorIntrusion.Contracts.IO;

using AuthorIntrusionGtk.Resources;

using Gtk;

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
		public SaveDocumentAsDialog(MainWindow parent, IOutputManager outputManager)
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