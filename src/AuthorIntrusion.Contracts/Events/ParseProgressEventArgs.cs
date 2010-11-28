#region Namespaces

using System;

#endregion

namespace AuthorIntrusion.Contracts.Events
{
	/// <summary>
	/// Defines the event arguments for showing the parsing progress.
	/// </summary>
	public class ParseProgressEventArgs : EventArgs
	{

		public ParseProgressEventArgs(int paragraphsProcessed, int paragraphCount)
		{
			this.paragraphsProcessed = paragraphsProcessed;
			this.paragraphCount = paragraphCount;
		}

		private int paragraphsProcessed;
		private int paragraphCount;

		public int ParagraphsProcessed
		{
			get { return paragraphsProcessed; }
		}

		public int ParagraphCount
		{
			get { return paragraphCount; }
		}
	}
}
