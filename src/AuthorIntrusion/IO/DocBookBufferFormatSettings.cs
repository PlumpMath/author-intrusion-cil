// <copyright file="DocBookBufferFormatSettings.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

namespace AuthorIntrusion.IO
{
	/// <summary>
	/// Defines the settings for the DocBook buffer format.
	/// </summary>
	public class DocBookBufferFormatSettings : IBufferFormatSettings
	{
		#region Constructors and Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="DocBookBufferFormatSettings"/> class.
		/// </summary>
		public DocBookBufferFormatSettings()
		{
			RootElement = "article";
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Contains the DocBook 5 XML element name for the top level. Defaults to
		/// "article".
		/// </summary>
		public string RootElement { get; set; }

		#endregion
	}
}
