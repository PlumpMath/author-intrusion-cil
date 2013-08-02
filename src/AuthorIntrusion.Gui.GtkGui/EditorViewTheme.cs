// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Blocks;
using Cairo;
using MfGames.GtkExt;
using MfGames.GtkExt.TextEditor;
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
			// Set up the indicator elements.
			SetupThemeIndicators(theme);

			// Use slightly more muted colors for the current line.
			theme.RegionStyles["EditorViewCurrentLine"].BackgroundColor = new Color(
				1, 1, 1);
			theme.RegionStyles["EditorViewCurrentWrappedLine"].BackgroundColor =
				new Color(245 / 255.0, 245 / 255.0, 220 / 255.0);

			// Set up the paragraph style.
			var paragraphyStyle = new LineBlockStyle(theme.TextLineStyle)
			{
				FontDescription =
					FontDescriptionCache.GetFontDescription("Source Code Pro 16"),
				Margins =
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

		private static void SetupThemeIndicators(Theme theme)
		{
			// Set up the indicator styles.
			theme.IndicatorRenderStyle = IndicatorRenderStyle.Ratio;
			theme.IndicatorPixelHeight = 2;
			theme.IndicatorRatioPixelGap = 1;

			var indicatorBackgroundStyle = new RegionBlockStyle
			{
				//BackgroundColor = new Color(1, 0.9, 1)
			};

			var indicatorVisibleStyle = new RegionBlockStyle
			{
				BackgroundColor = new Color(1, 1, 0.9)
			};

			indicatorVisibleStyle.Borders.SetBorder(new Border(1, new Color(0, 0.5, 0)));

			// Add the styles to the theme.
			theme.RegionStyles[IndicatorView.BackgroundRegionName] =
				indicatorBackgroundStyle;
			theme.RegionStyles[IndicatorView.VisibleRegionName] = indicatorVisibleStyle;

			// Set up the various indicators.
			theme.IndicatorStyles["Error"] = new IndicatorStyle(
				"Error", 100, new Color(1, 0, 0));
			theme.IndicatorStyles["Warning"] = new IndicatorStyle(
				"Warning", 10, new Color(1, 165 / 255.0, 0));
			theme.IndicatorStyles["Chapter"] = new IndicatorStyle(
				"Chapter", 2, new Color(100 / 255.0, 149 / 255.0, 237 / 255.0));
		}

		#endregion
	}
}
