using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Awesomium.Core;

namespace AuthorIntrustionSwf
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			// Create the window and show it.
			var webCoreConfig = new WebCoreConfig()
			{
				CustomCSS = "::-webkit-scrollbar { color: red; }",
				AutoUpdatePeriod = 10
			};
			WebCore.Initialize(webCoreConfig);

			InitializeComponent();

			webControl.BeginLoading += (sender,
			                args) => Debug.WriteLine("BeginLoading: " + args.Url);
			//webControl.BeginNavigation += (sender,
			//                               args) => Debug.WriteLine("BeginNavigation: " + args);
			webControl.CursorChanged += (sender,
										 args) => Debug.WriteLine("CursorChanged: " + args);
			//webControl.DomReady += (sender,
			//                        args) => Debug.WriteLine("DomReady: " + args);
			webControl.LoadCompleted += (sender,
										 args) => Debug.WriteLine("LoadCompleted: " + args);
			webControl.CausesValidationChanged += (sender,
												   args) => Debug.WriteLine("CausesValidations: " + args);
			//webControl.IsDirtyChanged += (sender,
			//                              args) => Debug.WriteLine("IsDirtyChanges: " + args);
			webControl.PageContentsReceived += (sender,
												args) => Debug.WriteLine("PageContentsReceived: " + args);
			webControl.OpenExternalLink += (sender,
											args) => Debug.WriteLine("OpenExternalLink: " + args.Url);
			webControl.KeyPress += (sender,
									args) => Debug.WriteLine("KeyPress: " + args.KeyChar);
		}

		private void OnLoad(object sender, EventArgs e)
		{
			webControl.LoadURL("file:///C:/Users/dmoonfire/Documents/MfGames/author-intrusion/src/test.html");

			while (webControl.IsLoadingPage)
			{
				System.Threading.Thread.Sleep(10);
			}

			WebCore.Update();
		}
	}
}
