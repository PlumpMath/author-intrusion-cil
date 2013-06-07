// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Plugins;

namespace AuthorIntrusion.Common.Persistence
{
	/// <summary>
	/// Defines the common interface for all plugins that are involved with reading
	/// and writing project data to a filesystem or network.
	/// </summary>
	public interface IPersistencePlugin: IProjectPluginProviderPlugin
	{
	}
}
