// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System.IO;
using AuthorIntrusion.Common.Plugins;

namespace AuthorIntrusion.Common.Persistence
{
	/// <summary>
	/// The project plugin to describe an export/save format for a project.
	/// </summary>
	public class FilesystemPersistenceProjectPlugin: IProjectPlugin
	{
		#region Methods

		/// <summary>
		/// Writes out the project file to a given directory.
		/// </summary>
		/// <param name="directoryInfo">The directory to save the file.</param>
		public void Save(DirectoryInfo directoryInfo)
		{
		}

		/// <summary>
		/// Configures a standard file layout that uses an entire directory for
		/// the layout.
		/// </summary>
		public void SetIndividualDirectoryLayout()
		{
		}

		#endregion
	}
}
