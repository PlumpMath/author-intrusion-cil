// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System.IO;
using AuthorIntrusion.Common.Plugins;

namespace AuthorIntrusion.Common.Persistence
{
	/// <summary>
	/// Defines the common interface for all plugins that are involved with reading
	/// and writing project data to a filesystem or network.
	/// </summary>
	public interface IPersistencePlugin: IProjectPluginProviderPlugin
	{
		#region Methods

		/// <summary>
		/// Determines whether this instance can read the specified project file.
		/// </summary>
		/// <param name="projectFile">The project file.</param>
		/// <returns>
		///   <c>true</c> if this instance can read the specified project file; otherwise, <c>false</c>.
		/// </returns>
		bool CanRead(FileInfo projectFile);

		/// <summary>
		/// Reads the project from the given file and returns it.
		/// </summary>
		/// <param name="projectFile">The project file.</param>
		/// <returns>The resulting project file.</returns>
		Project ReadProject(FileInfo projectFile);

		#endregion
	}
}
