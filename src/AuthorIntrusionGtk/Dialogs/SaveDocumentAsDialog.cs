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
		public SaveDocumentAsDialog(Window parent)
			: base(
				"Choose the name of the document",
				parent,
				FileChooserAction.Save,
				"Cancel",
				ResponseType.Cancel,
				"Save",
				ResponseType.Accept)
		{
		}

		#endregion
	}
}