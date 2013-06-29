// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System.Text.RegularExpressions;
using MfGames;

namespace AuthorIntrusion.Dedications
{
	/// <summary>
	/// Contains the information about a single dedication to the program.
	/// </summary>
	public class Dedication
	{
		#region Properties

		/// <summary>
		/// Gets or sets the author the version is dedicated to.
		/// </summary>
		public string Author { get; set; }

		/// <summary>
		/// Gets or sets the person who wrote the dedication.
		/// </summary>
		public string Dedicator { get; set; }

		/// <summary>
		/// Gets or sets the HTML dedication.
		/// </summary>
		public string Html { get; set; }

		public string[] Lines
		{
			get
			{
				string text = Html;
				text = Regex.Replace(text, "(\\s+|<p>)+", " ").Trim();
				string[] lines = Regex.Split(text, "\\s*</p>\\s*");
				return lines;
			}
		}

		/// <summary>
		/// Gets or sets the version for the dedication.
		/// </summary>
		public ExtendedVersion Version { get; set; }

		#endregion

		#region Constructors

		public Dedication()
		{
			Html = string.Empty;
		}

		#endregion
	}
}
