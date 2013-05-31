// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

namespace AuthorIntrusion.Common.Plugins
{
	/// <summary>
	/// Defines the signature for a plugin that is used at the project level. This
	/// differs from IEnvironmentPlugin in that each project has their own plugins
	/// (some can be duplicates of the same IProjectPlugin) along with their own
	/// configuration.
	/// </summary>
	public interface IProjectPlugin
	{
		/// <summary>
		/// Gets a value indicating whether there can be multiple controllers
		/// for this plugin inside a given project. This would be false for plugins
		/// that have little or no configuration or that their operation would
		/// conflict with each other. It would be true for customizable plugins
		/// such as ones that allow for word highlighting or specific grammer rules.
		/// </summary>
		/// <value>
		///   <c>true</c> if multiple controllers are allowed; otherwise, <c>false</c>.
		/// </value>
		bool AllowMultiple { get; }

		/// <summary>
		/// Gets the cannon, human-readable name of the plugin.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Gets a project-specific controller for the plugin.
		/// </summary>
		/// <param name="project">The project.</param>
		/// <returns>A controller for the project.</returns>
		IProjectPluginController GetController(Project project);
	}
}
