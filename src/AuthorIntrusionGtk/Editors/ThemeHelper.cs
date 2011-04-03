#region Copyright and License

// Copyright (c) 2011, Moonfire Games
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

#endregion

#region Namespaces

using System;

using AuthorIntrusion.Contracts.Matters;

using Cairo;

using MfGames.GtkExt;
using MfGames.GtkExt.TextEditor.Models.Styles;

#endregion

namespace AuthorIntrusionGtk.Editors
{
	/// <summary>
	/// Helper class that sets up the theme settings for the application. This
	/// creates the basic theme structure, including elements for the various
	/// matter and region types and sets the permission for editing.
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
			// Pull out the common styles we'll use and set them up.
			var textStyle = theme.TextLineStyle;
			textStyle.FontDescription =
				FontDescriptionCache.GetFontDescription("Courier New 12");

			// Create styles for all the matter types.
			foreach (MatterType matterType in Enum.GetValues(typeof(MatterType)))
			{
				// Create the default matter style.
				var matterStyle = new LineBlockStyle(textStyle);

				theme.LineStyles[matterType.ToString()] = matterStyle;

				// Paragraphs also have an empty style.
				switch (matterType)
				{
					case MatterType.Paragraph:
						// All region styles have an associated empty style.
						var emptyStyle = new LineBlockStyle(matterStyle);
						emptyStyle.ForegroundColor = new Color(0.8, 0.8, 0.8);

						theme.LineStyles["Blank " + matterType] = emptyStyle;
						break;

					case MatterType.Region:
						matterStyle.FontDescription =
							FontDescriptionCache.GetFontDescription("Courier New Bold 12");
						break;
				}
			}

			// Create styles for all the region types.
			foreach (RegionType regionType in Enum.GetValues(typeof(RegionType)))
			{
				// Create the basic region style.
				var regionStyle = new LineBlockStyle(theme.LineStyles["Region"]);

				theme.LineStyles[regionType.ToString()] = regionStyle;

				// All region styles have an associated empty style.
				var emptyStyle = new LineBlockStyle(regionStyle);
				emptyStyle.ForegroundColor = new Color(0.8, 0.8, 0.8);

				theme.LineStyles["Blank " + regionType] = emptyStyle;

				// Process any default customizations on the styles.
				switch (regionType)
				{
					case RegionType.Book:
						regionStyle.FontDescription =
							FontDescriptionCache.GetFontDescription("Courier New Bold 18");
						break;

					case RegionType.Chapter:
					case RegionType.Article:
						regionStyle.FontDescription =
							FontDescriptionCache.GetFontDescription("Courier New Bold 14");
						break;
				}
			}

			// Return the resulting theme.
			return;
		}
	}
}