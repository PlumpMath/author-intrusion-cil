// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Blocks;

namespace AuthorIntrusion.Common.Commands
{
	/// <summary>
	/// An operation that sets the text for the entire block with no respect to
	/// position or current state.
	/// </summary>
	public class SetTextCommand: SingleBlockCommand
	{
		#region Properties

		public override bool IsUndoable
		{
			get { return true; }
		}

		public string Text { get; private set; }

		#endregion

		#region Methods

		protected override void Do(
			Project project,
			Block block)
		{
			block.SetText(Text);
		}

		protected override IBlockCommand GetInverseCommand(
			Project project,
			Block block)
		{
			var command = new SetTextCommand(BlockKey, block.Text);
			return command;
		}

		#endregion

		#region Constructors

		public SetTextCommand(
			BlockKey blockKey,
			string text)
			: base(blockKey)
		{
			Text = text;
		}

		#endregion
	}
}
