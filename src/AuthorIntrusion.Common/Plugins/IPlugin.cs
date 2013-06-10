// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

namespace AuthorIntrusion.Common.Plugins
{
	/// <summary>
	/// Defines a generic plugin used in the system.
	/// </summary>
	public interface IPlugin
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
