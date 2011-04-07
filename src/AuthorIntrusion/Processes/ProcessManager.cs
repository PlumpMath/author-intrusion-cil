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
using System.Diagnostics;
using System.Threading;

using AuthorIntrusion.Contracts;
using AuthorIntrusion.Contracts.Collections;
using AuthorIntrusion.Contracts.Matters;
using AuthorIntrusion.Contracts.Processes;

using C5;

using MfGames.Locking;

using StructureMap;

#endregion

namespace AuthorIntrusion.Processes
{
	/// <summary>
	/// This class is responsible for managing the parsing process for paragraphs
	/// and running each paragraph through the extensions.
	/// </summary>
	[PluginFamily(IsSingleton = true)]
	public class ProcessManager
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="ProcessManager"/> class.
		/// </summary>
		public ProcessManager()
		{
			queueLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);

			paragraphProcesses = new HashDictionary<int, ParagraphProcess>();
		}

		#endregion

		#region Document Management

		/// <summary>
		/// Called when a registered document's paragraph changes.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="AuthorIntrusion.Contracts.Matters.ParagraphChangedEventArgs"/> instance containing the event data.</param>
		private void OnParagraphChanged(
			object sender,
			ParagraphChangedEventArgs e)
		{
			QueueProcess(e.Paragraph, e.OldContents, ProcessTypes.All);
		}

		/// <summary>
		/// Registers the document with the process manager which causes this to
		/// listen to the changed events and automatically process the paragraphs.
		/// </summary>
		/// <param name="document">The document.</param>
		public void RegisterDocument(Document document)
		{
			// Connect to the events.
			document.ParagraphChanged += OnParagraphChanged;
		}

		/// <summary>
		/// Unregisters the document and no longer listens to the changes made
		/// to that item.
		/// </summary>
		/// <param name="document">The document.</param>
		public void UnregisterDocument(Document document)
		{
			// Disconnect to the events.
			document.ParagraphChanged -= OnParagraphChanged;
		}

		#endregion

		#region Process Management

		private readonly HashDictionary<int, ParagraphProcess> paragraphProcesses;
		private readonly ReaderWriterLockSlim queueLock;

		/// <summary>
		/// Gets the number of running processes.
		/// </summary>
		/// <value>The process count.</value>
		public int ProcessCount
		{
			get
			{
				using (new ReadLock(queueLock))
				{
					return paragraphProcesses.Count;
				}
			}
		}

		/// <summary>
		/// Called when a process handles the cancel request.
		/// </summary>
		/// <param name="process">The process.</param>
		internal void Canceled(ParagraphProcess process)
		{
			// Remove the process from the list.
			using (new WriteLock(queueLock))
			{
				int paragraphProcessKey = process.Paragraph.ParagraphProcessKey;

				if (paragraphProcesses[paragraphProcessKey] == process)
				{
					paragraphProcesses.Remove(paragraphProcessKey);
				}
			}

			// Raise the canceled paragraph event.
			RaiseParagraphCanceled(process.Paragraph);
		}

		/// <summary>
		/// Called when a process finishes.
		/// </summary>
		/// <param name="process">The process.</param>
		internal void Finished(ParagraphProcess process)
		{
			// Remove the process from the list.
			using (new WriteLock(queueLock))
			{
				int paragraphProcessKey = process.Paragraph.ParagraphProcessKey;

				if (paragraphProcesses[paragraphProcessKey] == process)
				{
					paragraphProcesses.Remove(paragraphProcessKey);
				}
			}

			// Raise the event so other listeners can update their state.
			Debug.WriteLine("Finished " + process + " " + ProcessCount + " remaining");
			RaiseParagraphFinished(process.Paragraph);
		}

		/// <summary>
		/// Occurs when a paragraph process is canceled.
		/// </summary>
		public event EventHandler<ParagraphEventArgs> ParagraphCanceled;

		/// <summary>
		/// Occurs when a paragraph process is finished.
		/// </summary>
		public event EventHandler<ParagraphEventArgs> ParagraphFinished;

		/// <summary>
		/// Occurs when paragraph is queued for processing.
		/// </summary>
		public event EventHandler<ParagraphEventArgs> ParagraphQueued;

		/// <summary>
		/// Occurs when a paragraph process starts on the paragraph.
		/// </summary>
		public event EventHandler<ParagraphEventArgs> ParagraphStarted;

		/// <summary>
		/// Processes the specified paragraph, adding it to the queue.
		/// </summary>
		/// <param name="paragraph">The paragraph.</param>
		/// <param name="oldContents">The old contents.</param>
		/// <param name="processTypes">The process types.</param>
		public void QueueProcess(
			Paragraph paragraph,
			ContentList oldContents,
			ProcessTypes processTypes)
		{
			// Verify our properties.
			if (paragraph == null)
			{
				throw new ArgumentNullException("paragraph");
			}

			// Get the unique identifier for this paragraph instance.
			int paragraphProcessKey = paragraph.ParagraphProcessKey;

			// We need a write lock around manipulating the lists.
			using (new WriteLock(queueLock))
			{
				// Check to see if we already have a process for this paragraph.
				if (paragraphProcesses.Contains(paragraphProcessKey))
				{
					// We need to cancel the previous job. We do this by pulling
					// the process out of our hash, then setting the canceled
					// flag. The process itself will check the flag to determine
					// if it should be canceled.
					ParagraphProcess oldProcess = paragraphProcesses[paragraphProcessKey];

					oldProcess.Cancel();
					paragraphProcesses.Remove(paragraphProcessKey);

					// Merge the old processe's operations into the one we'll be
					// adding. We do this so if the prior job was a Parse and the
					// second was a Report, we'll won't lose the Parse request.
					processTypes |= oldProcess.ProcessTypes;
				}

				// Create a new process.
				var process = new ParagraphProcess();
				process.Paragraph = paragraph;
				process.OldContents = oldContents;
				process.ProcessTypes = processTypes;
				process.ProcessManager = this;

				// Register the process in our working list.
				paragraphProcesses[paragraphProcessKey] = process;

				// Raise the queued event.
				RaiseParagraphQueued(paragraph);

				// Queue up the process on a worker thread.
				ThreadPool.QueueUserWorkItem(process.Process, process);
			}
		}

		/// <summary>
		/// Raises the paragraph canceled event.
		/// </summary>
		/// <param name="paragraph">The paragraph.</param>
		protected void RaiseParagraphCanceled(Paragraph paragraph)
		{
			if (ParagraphCanceled != null)
			{
				var args = new ParagraphEventArgs(paragraph);

				ParagraphCanceled(this, args);
			}
		}

		/// <summary>
		/// Raises the paragraph finished event.
		/// </summary>
		/// <param name="paragraph">The paragraph.</param>
		protected void RaiseParagraphFinished(Paragraph paragraph)
		{
			if (ParagraphFinished != null)
			{
				var args = new ParagraphEventArgs(paragraph);

				ParagraphFinished(this, args);
			}
		}

		/// <summary>
		/// Raises the paragraph queued event.
		/// </summary>
		/// <param name="paragraph">The paragraph.</param>
		protected void RaiseParagraphQueued(Paragraph paragraph)
		{
			if (ParagraphQueued != null)
			{
				var args = new ParagraphEventArgs(paragraph);

				ParagraphQueued(this, args);
			}
		}

		/// <summary>
		/// Raises the paragraph started event.
		/// </summary>
		/// <param name="paragraph">The paragraph.</param>
		protected void RaiseParagraphStarted(Paragraph paragraph)
		{
			if (ParagraphStarted != null)
			{
				var args = new ParagraphEventArgs(paragraph);

				ParagraphStarted(this, args);
			}
		}

		/// <summary>
		/// Called when a process starts on a paragraph.
		/// </summary>
		/// <param name="process">The process.</param>
		internal void Started(ParagraphProcess process)
		{
			Debug.WriteLine("Starting " + process);
			RaiseParagraphStarted(process.Paragraph);
		}

		#endregion
	}
}