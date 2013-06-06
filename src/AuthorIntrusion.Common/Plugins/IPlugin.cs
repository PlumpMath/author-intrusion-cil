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
		/// Gets the cannon, human-readable name of the plugin.
		/// </summary>
		string Name { get; }

		#endregion
	}
}
