// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System.Globalization;
using AuthorIntrusion.Common;
using AuthorIntrusion.Common.Blocks;
using MfGames.GtkExt.TextEditor.Models;
using MfGames.GtkExt.TextEditor.Models.Buffers;

namespace AuthorIntrusion.Gui.GtkGui
{
	/// <summary>
	/// Encapsulates an adapter class between the text editor's line buffer and
	/// the project information.
	/// </summary>
	public class ProjectLineBuffer: LineBuffer
	{
		#region Properties

		public override int LineCount
		{
			get
			{
				using (project.Blocks.AcquireReadLock())
				{
					int results = project.Blocks.Count;
					return results;
				}
			}
		}

		public override bool ReadOnly
		{
			get { return false; }
		}

		#endregion

		#region Methods

		public override LineBufferOperationResults Do(ILineBufferOperation operation)
		{
			return new LineBufferOperationResults();
		}

		public override int GetLineLength(
			int lineIndex,
			LineContexts lineContexts)
		{
			string line = GetLineText(lineIndex, lineContexts);
			return line.Length;
		}

		public override string GetLineNumber(int lineIndex)
		{
			return lineIndex.ToString(CultureInfo.InvariantCulture);
		}

		public override string GetLineText(
			int lineIndex,
			LineContexts lineContexts)
		{
			using (project.Blocks.AcquireReadLock())
			{
				Block block = project.Blocks[lineIndex];
				string line = block.Text;
				return line;
			}
		}

		#endregion

		#region Constructors

		public ProjectLineBuffer(Project project)
		{
			this.project = project;
		}

		#endregion

		#region Fields

		private readonly Project project;

		#endregion
	}
}
