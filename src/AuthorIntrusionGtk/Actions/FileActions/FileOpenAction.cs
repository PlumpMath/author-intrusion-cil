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

using System.IO;

using AuthorIntrusion.Contracts;
using AuthorIntrusion.Contracts.IO;

using AuthorIntrusionGtk.Dialogs;

using Gtk;

#endregion

namespace AuthorIntrusionGtk.Actions.FileActions
{
	/// <summary>
	/// Handles the Open... action in the file menu.
	/// </summary>
	public class FileOpenAction : ContextualAction
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="FileOpenAction"/> class.
		/// </summary>
		/// <param name="context">The context.</param>
		public FileOpenAction(Context context)
			: base(context, "FileOpen", "_Open...", null, Stock.Open)
		{
		}

		#endregion

		#region Actions

		/// <summary>
		/// Called when the action is activated.
		/// </summary>
		protected override void OnActivated()
		{
			var dialog = Container.GetInstance<OpenDocumentDialog>();

			try
			{
				// If the user cancelled, then just break out.
				if (dialog.Run() != (int) ResponseType.Accept)
				{
					return;
				}

				// The user accepted it, so attempt to parse the document.
				var inputManager = Context.Container.GetInstance<IInputManager>();
				var file = new FileInfo(dialog.Filename);

				Document document = inputManager.Read(file);
				Context.Document = document;
			}
			finally
			{
				dialog.Destroy();
			}
		}

		#endregion
	}
}