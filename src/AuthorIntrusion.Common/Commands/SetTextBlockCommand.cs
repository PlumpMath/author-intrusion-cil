// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

namespace AuthorIntrusion.Common.Commands
{
	/// <summary>
	/// An operation that sets the text for the entire block with no respect to
	/// position or current state.
	/// </summary>
	public class SetTextBlockCommand: SingleBlockCommand
	{
		#region Properties

		public string Text { get; private set; }

		#endregion

		#region Constructors

		public SetTextBlockCommand(
			BlockKey blockKey,
			string text)
			: base(blockKey)
		{
			Text = text;
		}

		#endregion

		public override bool IsUndoable
		{
			get { return true; }
		}

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
			var command = new SetTextBlockCommand(BlockKey,block.Text);
			return command;
		}
	}
}
