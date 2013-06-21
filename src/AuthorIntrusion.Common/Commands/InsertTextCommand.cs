// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Blocks;

namespace AuthorIntrusion.Common.Commands
{
	/// <summary>
	/// Operation to insert text into a single block at a given position.
	/// </summary>
	public class InsertTextCommand: BlockPositionCommand
	{
		#region Properties

		protected string Text { get; private set; }

		#endregion

		#region Methods

		/// <summary>
		/// Performs the command on the given block.
		/// </summary>
		/// <param name="project">The project that contains the current state.</param>
		/// <param name="block">The block to perform the action on.</param>
		protected override void Do(Block block)
		{
			// Figure out what the new text string would be.
			string newText = block.Text.Insert(TextIndex, Text);

			// Set the new text into the block. This will fire various events to
			// trigger the immediate and background processing.
			block.SetText(newText);

			// After we insert text, we need to give the immediate editor plugins a
			// chance to made any alterations to the output.
			block.Project.Plugins.ProcessImmediateEdits(block, TextIndex + Text.Length);
		}

		#endregion

		#region Constructors

		public InsertTextCommand(
			BlockPosition position,
			string text)
			: base(position)
		{
			Text = text;
		}

		#endregion
	}
}
