// <copyright file="TitleInfo.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

namespace AuthorIntrusion.IO
{
	/// <summary>
	/// Represents the titles that can be assigned to a project.
	/// </summary>
	public class TitleInfo : INamed
	{
		#region Public Properties

		/// <summary>
		/// Gets or sets the abbreviation.
		/// </summary>
		/// <value>
		/// The abbreviation.
		/// </value>
		public string Abbreviation { get; set; }

		/// <summary>
		/// Gets or sets the subtitle.
		/// </summary>
		/// <value>
		/// The subtitle.
		/// </value>
		public string Subtitle { get; set; }

		/// <summary>
		/// Gets or sets the title.
		/// </summary>
		/// <value>
		/// The title.
		/// </value>
		public string Title { get; set; }

		#endregion

		#region Explicit Interface Properties

		/// <summary>
		/// Gets the name of the object.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		string INamed.Name { get { return Title; } }

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return Title ?? "<Untitled>";
		}

		#endregion
	}
}
