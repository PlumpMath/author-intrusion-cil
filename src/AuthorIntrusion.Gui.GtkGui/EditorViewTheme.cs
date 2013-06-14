// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Blocks;
using Cairo;
using MfGames.GtkExt;
using MfGames.GtkExt.TextEditor.Models.Styles;

namespace AuthorIntrusion.Gui.GtkGui
{
	public static class EditorViewTheme
	{
		#region Methods

		/// <summary>
		/// Sets up the theme elements.
		/// </summary>
		/// <param name="theme"></param>
		public static void SetupTheme(Theme theme)
		{
			// Set up the paragraph style.
			var paragraphyStyle = new LineBlockStyle(theme.TextLineStyle)
			{
				FontDescription =
					FontDescriptionCache.GetFontDescription("Source Code Pro 16"),
				Padding =
				{
					Top = 8,
					Bottom = 8
				}
			};

			// Set up the chapter style.
			var chapterStyle = new LineBlockStyle(theme.TextLineStyle)
			{
				FontDescription =
					FontDescriptionCache.GetFontDescription("Source Code Pro Bold 32"),
				Margins =
				{
					Bottom = 5
				},
				Borders =
				{
					Bottom = new Border(2, new Color(0, 0, 0))
				}
			};

			// Set up the scene style.
			var sceneStyle = new LineBlockStyle(theme.TextLineStyle)
			{
				FontDescription =
					FontDescriptionCache.GetFontDescription("Source Code Pro Italic 24"),
				ForegroundColor = new Color(.5, .5, .5)
			};

			// Set up the epigraph style.
			var epigraphStyle = new LineBlockStyle(theme.TextLineStyle)
			{
				FontDescription =
					FontDescriptionCache.GetFontDescription("Source Code Pro 12"),
				Padding =
				{
					Left = 20
				}
			};

			// Set up the epigraph attributation style.
			var epigraphAttributationStyle = new LineBlockStyle(theme.TextLineStyle)
			{
				FontDescription =
					FontDescriptionCache.GetFontDescription("Source Code Pro Italic 12"),
				Padding =
				{
					Left = 20
				}
			};

			// Add all the styles into the theme.
			theme.LineStyles[BlockTypeSupervisor.ParagraphName] = paragraphyStyle;
			theme.LineStyles[BlockTypeSupervisor.ChapterName] = chapterStyle;
			theme.LineStyles[BlockTypeSupervisor.SceneName] = sceneStyle;
			theme.LineStyles[BlockTypeSupervisor.EpigraphName] = epigraphStyle;
			theme.LineStyles[BlockTypeSupervisor.EpigraphAttributionName] =
				epigraphAttributationStyle;
		}

		#endregion
	}
}
