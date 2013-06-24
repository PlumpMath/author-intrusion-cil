// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Blocks;
using MfGames.Commands;

namespace AuthorIntrusion.Common.Commands
{
	/// <summary>
	/// An operation that sets the text for the entire block with no respect to
	/// position or current state.
	/// </summary>
	public class SetTextCommand: SingleBlockKeyCommand
	{
		#region Properties

		public string Text { get; private set; }

		#endregion

		#region Methods

		protected override void Do(
			BlockCommandContext context,
			Block block)
		{
			previousText = block.Text;
			block.SetText(Text);

			if(UpdateTextPosition.HasFlag(DoTypes.Do))
				context.Position = new BlockPosition(BlockKey,Text.Length);
		}

		protected override void Undo(
			BlockCommandContext context,
			Block block)
		{
			block.SetText(previousText);
			if(UpdateTextPosition.HasFlag(DoTypes.Undo))
				context.Position = new BlockPosition(BlockKey,previousText.Length);
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

		#region Fields

		private string previousText;

		#endregion
	}
}
