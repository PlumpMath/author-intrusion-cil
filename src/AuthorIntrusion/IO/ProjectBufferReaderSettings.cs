// <copyright file="ProjectBufferReaderSettings.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

namespace AuthorIntrusion.IO
{
	/// <summary>
	/// Settings class modeled after XmlReaderSettings that gives setting information for
	/// reading stream data and populating a given buffer.
	/// </summary>
	public class ProjectBufferReaderSettings
	{
		#region Constructors and Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="ProjectBufferReaderSettings"/> class.
		/// </summary>
		/// <param name="readSummary">
		/// if set to <c>true</c> [read summary].
		/// </param>
		/// <param name="readDetail">
		/// if set to <c>true</c> [read detail].
		/// </param>
		public ProjectBufferReaderSettings(
			bool readSummary,
			bool readDetail)
		{
			ReadSummary = readSummary;
			ReadDetail = readDetail;
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets a value indicating whether detail information should be loaded into
		/// the project. This includes line details, tokens, and line- and token-specific
		/// information.
		/// </summary>
		/// <value>
		///   <c>true</c> if detail should be read into the buffer; otherwise, <c>false</c>.
		/// </value>
		public bool ReadDetail { get; private set; }

		/// <summary>
		/// Gets a value indicating whether summary information should be populated. When
		/// set, then the given project buffer will be cleared. Otherwise, the project
		/// buffer will already have the summary data loaded.
		/// </summary>
		/// <value>
		///   <c>true</c> the summary data should be read in; otherwise, <c>false</c>.
		/// </value>
		public bool ReadSummary { get; private set; }

		#endregion
	}
}
