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
		public IProjectPlugin[] ProjectPlugins { get; private set; }

		#endregion

		#region Methods

		/// <summary>
		/// Tries to get the given plugin via the name.
		/// </summary>
		/// <param name="pluginName">Name of the plugin.</param>
		/// <param name="plugin">The plugin, if found.</param>
		/// <returns><c>true<c> if the plugin is found, otherwise </c>false</c>.</returns>
		public bool TryGet(
			string pluginName,
			out IProjectPlugin plugin)
		{
			foreach (IProjectPlugin projectPlugin in ProjectPlugins)
			{
				if (projectPlugin.Name == pluginName)
				{
					plugin = projectPlugin;
					return true;
				}
			}

			// We couldn't find it, so put in the default and return a false.
			plugin = null;
			return false;
		}

		#endregion

		#region Constructors

		public PluginManager(IProjectPlugin[] projectPlugins)
		{
			// Save all the block analzyers into a private array.
			ProjectPlugins = projectPlugins;
		}

		#endregion
	}
}
