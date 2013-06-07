// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

namespace AuthorIntrusion.Common.Persistence
{
	/// <summary>
	/// Defines the serializable settings used to control the filesystem persistence
	/// project plugin.
	/// 
	/// All of the string variables in here can have project macros in them, which are
	/// expanded at the point of saving.
	/// </summary>
	public class FilesystemPersistenceSettings
	{
		#region Properties

		/// <summary>
		/// Gets or sets the external blocks directory. This is the directory where
		/// external blocks (e.g., chapters that are extracted from the main file) are
		/// stored.
		/// 
		/// If this is null or empty, then external files are not allowed. This will
		/// have macros expanded.
		/// </summary>
		public string ExternalBlocksDirectory { get; set; }

		/// <summary>
		/// Gets or sets the external settings directory. This is the directory
		/// where non-project settings are stored. Macros inside the variable will
		/// be expanded at the point of saving. If this is blank or null, then the
		/// project settings will be stored in the project file.
		/// </summary>
		public string ExternalSettingsDirectory { get; set; }

		/// <summary>
		/// Gets or sets the project settings directory. This is expanded with
		/// macros to determine where the project settings will be stored. If
		/// this is blank, then the project settings will be stored in the project
		/// file.
		/// </summary>
		public string ProjectSettingsDirectory { get; set; }

		#endregion
	}
}
