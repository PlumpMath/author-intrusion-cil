using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using Awesomium.Core;

using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;

using Timer = System.Windows.Forms.Timer;

namespace AuthorIntrustionSwf
{
	public partial class Form1 : Form
	{
		Timer timer = new Timer();

		public Form1()
		{
			// Create the window and show it.
			var webCoreConfig = new WebCoreConfig()
			{
				CustomCSS = "::-webkit-scrollbar { color: red; }",
				AutoUpdatePeriod = 10,
			};
			WebCore.Initialize(webCoreConfig);

			InitializeComponent();

			webControl.BeginLoading += (sender,
			                args) => Debug.WriteLine("BeginLoading: " + args.Url);
			//webControl.BeginNavigation += (sender,
			//                               args) => Debug.WriteLine("BeginNavigation: " + args);
			//webControl.CursorChanged += (sender,
			//                             args) => Debug.WriteLine("CursorChanged: " + args);
			//webControl.DomReady += (sender,
			//                        args) => Debug.WriteLine("DomReady: " + args);
			webControl.LoadCompleted += OnWebControlLoaded;
			//webControl.CausesValidationChanged += (sender,
			//                                       args) => Debug.WriteLine("CausesValidations: " + args);
			//webControl.IsDirtyChanged += (sender,
			//                              args) => Debug.WriteLine("IsDirtyChanges: " + args);
			webControl.PageContentsReceived += (sender,
												args) => Debug.WriteLine("PageContentsReceived: " + args);
			webControl.OpenExternalLink += (sender,
											args) => Debug.WriteLine("OpenExternalLink: " + args.Url);
			//webControl.KeyPress += (sender,
			//                        args) => Debug.WriteLine("KeyPress: " + args.KeyChar);
		}

		private void OnWebControlLoaded(object sender,
		                                EventArgs e)
		{
			webControl.CreateObject("AuthorIntrusionGuiCallback");
			webControl.SetObjectCallback("AuthorIntrusionGuiCallback", "TriggerChange", OnChangesCallback);

			timer.Interval = PollMilliseconds;
			timer.Tick += OnPollWebControl;
			timer.Start();
		}

		private const int PollMilliseconds = 100;
		private const int IdleThreshold = 1000;
		private int idleCountdown;

		private void OnChangesCallback(object sender,
		                      JSCallbackEventArgs jsCallbackEventArgs)
		{
			idleCountdown = IdleThreshold;
		}

		private void OnPollWebControl(object sender,
		                              EventArgs e)
		{
			// If we don't have an idle threshold, then we have no changes to
			// wait for.
			if (idleCountdown <= 0)
			{
				//Debug.WriteLine("Skipping, no pooling: " + idleCountdown);
				return;
			}

			// Decrement the countdown and see if we are still waiting.
			idleCountdown -= PollMilliseconds;

			if (idleCountdown > 0)
			{
				return;
			}

			Debug.WriteLine("Polling for changes");

			// Query the web control for the results of a function call. This
			// will contain all the changed and deleted paragraphs.
			JSValue results = webControl.ExecuteJavascriptWithResult(
				"(function(){return getEditorParagraphChanges();})();");

			// If we have a string result, see if we want to deserialize it.
			if (results.Type != JSValueType.String)
			{
				return;
			}

			// We have a string, so pull it out and test its length.
			string jsonResults = results.ToString();

			if (string.IsNullOrWhiteSpace(jsonResults))
			{
				return;
			}

			// If we got this far, then we want to parse out the JSON results
			// and process them.
			var json = JsonConvert.DeserializeObject<EditorParagraphChanges>(jsonResults);

			ProcessChangesFromControl(json);
		}

		private class Paragraph
		{
			public string Id { get; set; }
			public string Html { get; set; }

			public long Timestamp { get; set; }
		}

		private Dictionary<string, long> processing = new Dictionary<string, long>(); 

		private void ProcessChangesFromControl(EditorParagraphChanges changes)
		{
			// Go through the changes paragraphs.
			foreach (string id in changes.Changes.Keys)
			{
				// Start up a task to process this.
				var para = new Paragraph();
				para.Id = id;
				para.Timestamp = DateTime.UtcNow.Ticks;
				para.Html = changes.Changes[id].Html;
				processing[id] = para.Timestamp;

				var task = new Task(() => UpdateParagraph(para));
				task.Start();
			}
		}
		
		private void UpdateParagraph(Paragraph para)
		{
			Random random = new Random();

			Thread.Sleep(random.Next(1000, 5000));

			string html = para.Html.Replace("<strong>", "").Replace("</strong>", "").Replace("bob", "<strong>bob</strong>");

			para.Html = html;

			webControl.Invoke(new Action<Paragraph>(SendUpdate), para);
		}

		private void SendUpdate(Paragraph para)
		{
			if (processing[para.Id] != para.Timestamp)
			{
				// Skip it.
				return;
			}

			string javascript = string.Format("(function(){{setEditorParagraph(\"{0}\", \"{1}\");}})();", para.Id, para.Html);
			Debug.WriteLine("Execute: {0}", javascript);
			webControl.ExecuteJavascript(javascript);
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

		private class EditorParagraphChange
		{
			public string Html { get; set; }
			public string Previous { get; set; }
			public string Next { get; set; }
		}

		private class EditorParagraphChanges
		{
			public Dictionary<string, EditorParagraphChange> Changes;
			public string[] Deleted;
		}
	}
}
