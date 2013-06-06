// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

namespace AuthorIntrusion.Common.Plugins
{
	public class PluginManager
	{
		#region Properties

		/// <summary>
		/// Gets or sets the static singleton instance.
		/// </summary>
		public static PluginManager Instance { get; set; }

		/// <summary>
		/// Gets an unsorted list of block analyzers available in the system.
		/// </summary>
		public IPlugin[] Plugins { get; private set; }

		#endregion

		#region Methods

		/// <summary>
		/// Tries to get the given project plugin via the name.
		/// </summary>
		/// <param name="pluginName">Name of the plugin.</param>
		/// <param name="plugin">The plugin, if found.</param>
		/// <returns><c>true<c> if the plugin is found, otherwise </c>false</c>.</returns>
		public bool TryGetProjectPlugin(
			string pluginName,
			out IProjectPluginProviderPlugin plugin)
		{
			// Go through all the project plugins and make sure they are both a
			// project plugin and they match the name.
			foreach (IPlugin projectPlugin in Plugins)
			{
				if (projectPlugin.Name == pluginName
					&& projectPlugin is IProjectPluginProviderPlugin)
				{
					plugin = (IProjectPluginProviderPlugin) projectPlugin;
					return true;
				}
			}

			// We couldn't find it, so put in the default and return a false.
			plugin = null;
			return false;
		}

		#endregion

		#region Constructors

		public PluginManager(params IPlugin[] plugins)
		{
			// Save all the block analzyers into a private array.
			Plugins = plugins;
		}

		#endregion
	}
}
