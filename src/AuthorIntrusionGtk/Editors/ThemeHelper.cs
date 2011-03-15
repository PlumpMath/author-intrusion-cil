using System;

using AuthorIntrusion.Contracts.Enumerations;

using MfGames.GtkExt;
using MfGames.GtkExt.LineTextEditor.Visuals;

namespace AuthorIntrusionGtk.Editors
{
	/// <summary>
	/// Class to configure the structure of the text editor theme.
	/// </summary>
	public static class ThemeHelper
	{
		/// <summary>
		/// Creates the theme structure for the text editor and gives it
		/// sane defaults.
		/// </summary>
		/// <returns></returns>
		public static void SetupTheme(Theme theme)
		{
			// Pull out the common styles we'll use.
			var textStyle = theme.TextLineStyle;

			// Create the section style container.
			var sectionStyle = new LineStyle(textStyle);

			sectionStyle.StyleUsage = StyleUsage.Application;

			theme.LineStyles["Section"] = sectionStyle;

			// Go through all the structure types and create a theme for them.
			foreach (StructureType structureType in Enum.GetValues(typeof(StructureType)))
			{
				// Create the basic style.
				var lineStyle = new LineStyle(
					structureType == StructureType.Paragraph ? textStyle : sectionStyle);

				lineStyle.StyleUsage = StyleUsage.Application;

				theme.LineStyles[structureType.ToString()] = lineStyle;

				// Perform type-specific initialization.
				switch (structureType)
				{
					case StructureType.Book:
						lineStyle.FontDescription =
							FontDescriptionCache.GetFontDescription("Sans Bold 24");
						break;

					case StructureType.Chapter:
					case StructureType.Article:
						lineStyle.FontDescription =
							FontDescriptionCache.GetFontDescription("Sans Bold 20");
						break;
				}
			}

			// Update the styles to make them a little more distinct as a default.
			theme.TextLineStyle.FontDescription =
				FontDescriptionCache.GetFontDescription("Serif 12");

			sectionStyle.FontDescription =
				FontDescriptionCache.GetFontDescription("Sans 12");

			// Return the resulting theme.
			return;
		}
	}
}