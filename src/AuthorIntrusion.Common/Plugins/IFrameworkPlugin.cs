// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System.Collections.Generic;

namespace AuthorIntrusion.Common.Plugins
{
	/// <summary>
	/// Defines the interface of a plugin that coordinates between other plugins. These
	/// plugins are used to gather up other plugins or to establish a relationship
	/// between multiple plugins.
	/// </summary>
	public interface IFrameworkPlugin: IPlugin
	{
		#region Methods

		/// <summary>
		/// Attempts to register the plugins with the current plugin. The given
		/// enumerable will not contain the plugin itself.
		/// </summary>
		/// <param name="additionalPlugins">The plugins.</param>
		void RegisterPlugins(IEnumerable<IPlugin> additionalPlugins);

		#endregion
	}
}
