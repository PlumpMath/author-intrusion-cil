using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

using AuthorIntrusion.Contracts.Collections;
using AuthorIntrusion.Contracts.Matters;

using C5;

using MfGames.Locking;

namespace AuthorIntrusion.Contracts.Processors
{
	/// <summary>
	/// A heavy-weight management of Processor instances.
	/// </summary>
	public class DocumentProcessorManager: IEnumerable<IProcessor>
	{
		#region Fields

		private Document document;
		private readonly IProcessor[] processors;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="DocumentProcessorManager"/> class.
		/// </summary>
		public DocumentProcessorManager(IProcessor[] processors)
		{
			// Save the processors so we can create a graph later.
			this.processors = processors;

			// Set up the rest of the collections.
			queueLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
			paragraphProcesses = new HashDictionary<int, ProcessorContext>();
		}

		#endregion

		#region Documents

		/// <summary>
		/// Sets the document and connects the events.
		/// </summary>
		/// <param name="item">The item.</param>
		public void SetDocument(Document item)
		{
			// Assign the document to the member fields.
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}

			document = item;

			// Attach to the events from the document.
			document.ParagraphChanged += OnParagraphChanged;
		}

		#endregion

		#region Processors

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
		/// </returns>
		/// <filterpriority>1</filterpriority>
		public IEnumerator<IProcessor> GetEnumerator()
		{
			var list = new ArrayList<IProcessor>();
			list.AddAll(processors);
			return list.GetEnumerator();
		}

		#endregion

		#region Editing

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

		#endregion

		#region Processing

		private readonly HashDictionary<int, ProcessorContext> paragraphProcesses;
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
		internal void Canceled(ProcessorContext process)
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
		internal void Finished(ProcessorContext process)
		{
			// Remove the process from the list.
			using (new WriteLock(queueLock))
			{
				int paragraphProcessKey = process.Paragraph.ParagraphProcessKey;

				if (paragraphProcesses.Contains(paragraphProcessKey) &&
					paragraphProcesses[paragraphProcessKey] == process)
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
					ProcessorContext oldProcess = paragraphProcesses[paragraphProcessKey];

					oldProcess.Cancel();
					paragraphProcesses.Remove(paragraphProcessKey);

					// Merge the old processe's operations into the one we'll be
					// adding. We do this so if the prior job was a Parse and the
					// second was a Report, we'll won't lose the Parse request.
					processTypes |= oldProcess.ProcessTypes;
				}

				// Create a new process.
				var process = new ProcessorContext();
				process.Paragraph = paragraph;
				process.OldContents = oldContents;
				process.ProcessTypes = processTypes;
				process.Processors = this;

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
		internal void Started(ProcessorContext process)
		{
			Debug.WriteLine("Starting " + process);
			RaiseParagraphStarted(process.Paragraph);
		}

		#endregion
	}
}