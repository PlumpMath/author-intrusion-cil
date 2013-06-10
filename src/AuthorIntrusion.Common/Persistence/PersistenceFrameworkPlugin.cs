// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System.Collections.Generic;
using System.Linq;
using AuthorIntrusion.Common.Plugins;
using C5;

namespace AuthorIntrusion.Common.Persistence
{
	/// <summary>
	/// Defines a system plugin for handling the Persistence layer. This manages the
	/// various ways a file can be loaded and saved from the filesystem and network.
	/// </summary>
	public class PersistenceFrameworkPlugin: IFrameworkPlugin,
		IProjectPluginProviderPlugin
	{
		#region Properties

		public bool AllowMultiple
		{
			get { return false; }
		}

		public string Key
		{
			get { return "Persistence Framework"; }
		}

		/// <summary>
		/// Gets the list of persistent plugins registered with the framework.
		/// </summary>
		public ArrayList<IPersistencePlugin> PersistentPlugins
		{
			get { return plugins; }
		}

		#endregion

		#region Methods

		public IProjectPlugin GetProjectPlugin(Project project)
		{
			var projectPlugin = new PersistenceFrameworkProjectPlugin(project);
			return projectPlugin;
		}

		public void RegisterPlugins(IEnumerable<IPlugin> additionalPlugins)
		{
			// Go through all the plugins and add the persistence plugins to our
			// internal list.
			IEnumerable<IPersistencePlugin> persistencePlugins =
				additionalPlugins.OfType<IPersistencePlugin>();

			foreach (IPersistencePlugin plugin in persistencePlugins)
			{
				plugins.Add(plugin);
			}
		}

		#endregion

		#region Constructors

		public PersistenceFrameworkPlugin()
		{
			plugins = new ArrayList<IPersistencePlugin>();
		}

		#endregion

		#region Fields

		private readonly ArrayList<IPersistencePlugin> plugins;

		#endregion
	}
}
