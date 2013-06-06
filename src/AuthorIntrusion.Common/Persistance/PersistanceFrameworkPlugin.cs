// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System.Collections.Generic;
using System.Linq;
using AuthorIntrusion.Common.Plugins;
using C5;

namespace AuthorIntrusion.Common.Persistance
{
	/// <summary>
	/// Defines a system plugin for handling the persistance layer. This manages the
	/// various ways a file can be loaded and saved from the filesystem and network.
	/// </summary>
	public class PersistanceFrameworkPlugin: IFrameworkPlugin
	{
		#region Properties

		public string Name
		{
			get { return "Persistance Framework"; }
		}

		#endregion

		#region Methods

		public void RegisterPlugins(IEnumerable<IPlugin> additionalPlugins)
		{
			// Go through all the plugins and add the persistence plugins to our
			// internal list.
			IEnumerable<IPersistancePlugin> persistancePlugins =
				additionalPlugins.OfType<IPersistancePlugin>();

			foreach (IPersistancePlugin plugin in persistancePlugins)
			{
				plugins.Add(plugin);
			}
		}

		#endregion

		#region Constructors

		public PersistanceFrameworkPlugin()
		{
			plugins = new ArrayList<IPersistancePlugin>();
		}

		#endregion

		#region Fields

		private readonly ArrayList<IPersistancePlugin> plugins;

		#endregion
	}
}
