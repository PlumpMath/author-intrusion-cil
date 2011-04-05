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

using System.Threading;

using AuthorIntrusion.Contracts.Collections;
using AuthorIntrusion.Contracts.Matters;
using AuthorIntrusion.Contracts.Processes;

#endregion

namespace AuthorIntrusion.Processes
{
	/// <summary>
	/// Represents the information about a single paragraph process.
	/// </summary>
	internal class ParagraphProcess
	{
		#region Fields

		private bool isCanceled;

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the old contents of the paragraph.
		/// </summary>
		/// <value>The old contents.</value>
		public ContentList OldContents { get; set; }

		/// <summary>
		/// Gets or sets the paragraph being processed.
		/// </summary>
		/// <value>The paragraph.</value>
		public Paragraph Paragraph { get; set; }

		/// <summary>
		/// Gets or sets the process manager associated with this process.
		/// </summary>
		/// <value>The process manager.</value>
		public ProcessManager ProcessManager { get; set; }

		/// <summary>
		/// Gets or sets the types of processes needed to be run.
		/// </summary>
		/// <value>The process types.</value>
		public ProcessTypes ProcessTypes { get; set; }

		#endregion

		#region Process Management

		/// <summary>
		/// Tells the process to cancel itself.
		/// </summary>
		public void Cancel()
		{
			isCanceled = true;
		}

		#endregion

		#region Processing

		/// <summary>
		/// Processes the given paragraph.
		/// </summary>
		public void Process(object state)
		{
			// Mark that we started our process.
			ProcessManager.Started(this);

			// Do something takes a lot of work.
			for (int i = 0; i < 50; i++)
			{
				// Sleep for a short period of time.
				Thread.Sleep(100);

				// Check to see if we are canceled.
				if (isCanceled)
				{
					ProcessManager.Canceled(this);
					return;
				}
			}

			// If we got this far, we finished.
			ProcessManager.Finished(this);
		}

		#endregion

		#region Conversion

		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </returns>
		public override string ToString()
		{
			return "Process for Paragraph " + Paragraph.ParagraphProcessKey;
		}

		#endregion
	}
}