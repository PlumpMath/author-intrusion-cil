// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

namespace AuthorIntrusion.Common.Plugins
{
	/// <summary>
	/// Defines a flyweight configuration and wrapper around an IPlugin.
	/// This is a project-specific controller that handles the project-specific
	/// settings and also exposes methods to perform the required operations of
	/// the plugin.
	/// </summary>
	public interface IProjectPlugin
	{
		#region Properties

		/// <summary>
		/// Gets the internal name of the plugin. This is not displayed to the
		/// user and it is used to look up the plugin via a string, so it must not
		/// be translated and cannot change between versions.
		/// 
		/// This should contain information to distinguish between different instances
		/// of the project plugin if the plugin allows multiple instances.
		/// </summary>
		string Key { get; }

		#endregion
	}
}
