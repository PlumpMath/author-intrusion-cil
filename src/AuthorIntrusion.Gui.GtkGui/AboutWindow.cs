// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Dedications;
using Gtk;

namespace AuthorIntrusion.Gui.GtkGui
{
	/// <summary>
	/// The about window which includes a display of the dedication and information
	/// </summary>
	public class AboutWindow: Dialog
	{
		#region Methods

		private void CreateGui(
			string programName,
			Dedication dedication)
		{
			// Create the title and version labels.
			var programLabel = new Label
			{
				Markup = "<b><span size='100'>" + programName + "</span></b>"
			};
			var versionLabel = new Label
			{
				Markup = "<b><span size='80'>" + dedication.Version + "</span></b>"
			};
			var authorLabel = new Label
			{
				Markup = "<b><span size='80'>" + dedication.Author + "</span></b>"
			};

			// Set up the dedication text.
			string html = string.Join("\n", dedication.Lines);
			string text = html + "- " + dedication.Dedicator;

			// Create an HTML display widget with the text.
			var dedicationView = new TextView
			{
				Buffer =
				{
					Text = text
				},
				WrapMode = WrapMode.Word,
				PixelsBelowLines = 10,
				RightMargin = 8,
				LeftMargin = 4,
				Justification = Justification.Fill,
				Sensitive = false,
			};

			var dedicatorTag = new TextTag("dedicator");
			dedicatorTag.Style = Pango.Style.Italic;
			dedicatorTag.Justification = Justification.Right;
			dedicationView.Buffer.TagTable.Add(dedicatorTag);

			TextIter dedicatorIterBegin =
				dedicationView.Buffer.GetIterAtOffset(html.Length);
			TextIter dedicatorIterEnd = dedicationView.Buffer.GetIterAtOffset(
				text.Length);
			dedicationView.Buffer.ApplyTag(
				dedicatorTag, dedicatorIterBegin, dedicatorIterEnd);

			// Wrap it in a scroll window with some additional spacing.
			var dedicationScroll = new ScrolledWindow
			{
				BorderWidth = 4
			};
			dedicationScroll.Add(dedicationView);

			// Create the notebook we'll be using for the larger text tabs.
			var notebook = new Notebook();
			notebook.SetSizeRequest(512, 256);
			notebook.BorderWidth = 4;

			notebook.AppendPage(dedicationScroll, new Label("Dedication"));

			// Arrange everything in the vertical box.
			VBox.PackStart(programLabel, false, false, 4);
			VBox.PackStart(versionLabel, false, false, 4);
			VBox.PackStart(authorLabel, false, false, 4);
			VBox.PackStart(notebook, true, true, 4);

			// Set up the buttons.
			Modal = true;

			AddButton("Close", ResponseType.Close);
		}

		#endregion

		#region Constructors

		public AboutWindow()
		{
			// Pull out some useful variables.
			var dedicationManager = new DedicationManager();
			Dedication dedication = dedicationManager.CurrentDedication;

			// Set the title to the longer title with the program name.
			const string programName = "Author Intrusion";
			const string windowTitle = "About " + programName;

			Title = windowTitle;

			// Create the rest of the GUI.
			CreateGui(programName, dedication);
			ShowAll();

			//			Version = dedication.ToString();

			//			Comments = dedication.Html;

			//			Copyright = "2013, Moonfire Games";

			//			License = @"Copyright (c) 2013, Moonfire Games
			//
			//Permission is hereby granted, free of charge, to any person obtaining a copy
			//of this software and associated documentation files (the ""Software""), to deal
			//in the Software without restriction, including without limitation the rights
			//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
			//copies of the Software, and to permit persons to whom the Software is
			//furnished to do so, subject to the following conditions:
			//
			//The above copyright notice and this permission notice shall be included in
			//all copies or substantial portions of the Software.
			//
			//THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
			//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
			//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
			//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
			//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
			//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
			//THE SOFTWARE.";

			//			Authors = new[]
			//			{
			//				"Dylan R. E. Moonfire"
			//			};
		}

		#endregion
	}
}
