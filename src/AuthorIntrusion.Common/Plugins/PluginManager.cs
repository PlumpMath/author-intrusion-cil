// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System.Collections.Generic;
using System.Linq;
using C5;

namespace AuthorIntrusion.Common.Plugins
{
	/// <summary>
	/// Gathers and coordinates the various plugins used in the system. This includes
	/// both project- and system-specific plugins.
	/// </summary>
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
		/// Gets the plugin with the specified name.
		/// </summary>
		/// <param name="pluginName">Name of the plugin.</param>
		/// <returns></returns>
		/// <exception cref="C5.NoSuchItemException">Cannot find plugin:  + pluginName</exception>
		public IPlugin Get(string pluginName)
		{
			IEnumerable<IPlugin> namedPlugins = Plugins.Where(p => p.Name == pluginName);

			foreach (IPlugin plugin in namedPlugins)
			{
				return plugin;
			}

			throw new NoSuchItemException("Cannot find plugin: " + pluginName);
		}

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

			// Once we have the plugins sorted, we need to allow the framework
			// plugins to hook themselves up to the other plugins.
			foreach (IPlugin plugin in plugins)
			{
				// See if this is a framework plugin (e.g., one that coordinates with
				// other plugins).
				var frameworkPlugin = plugin as IFrameworkPlugin;

				if (frameworkPlugin == null)
				{
					continue;
				}

				// Build a list of plugins that doesn't include the framework plugin.
				var relatedPlugins = new ArrayList<IPlugin>();
				relatedPlugins.AddAll(plugins);
				relatedPlugins.Remove(plugin);

				// Register the plugins with the calling class.
				frameworkPlugin.RegisterPlugins(relatedPlugins);
			}
		}

		#endregion
	}
}
