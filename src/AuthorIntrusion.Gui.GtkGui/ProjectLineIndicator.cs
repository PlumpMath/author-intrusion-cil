// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using MfGames.GtkExt.TextEditor.Models.Buffers;

namespace AuthorIntrusion.Gui.GtkGui
{
	public class ProjectLineIndicator: ILineIndicator
	{
		#region Properties

		public string LineIndicatorStyle { get; private set; }

		#endregion

		#region Constructors

		public ProjectLineIndicator(string lineIndicatorStyle)
		{
			LineIndicatorStyle = lineIndicatorStyle;
		}

		#endregion
	}
}
