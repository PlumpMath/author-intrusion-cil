// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System.Collections.Generic;

namespace AuthorIntrusion.Common.Plugins
{
	/// <summary>
	/// Defines the signature for a controller that integrates with other plugins
	/// such as spelling.
	/// </summary>
	public interface IFrameworkProjectPlugin: IProjectPlugin
	{
		#region Methods

		/// <summary>
		/// Called when a new plugin controller is enabled for a project.
		/// </summary>
		/// <param name="project">The project.</param>
		/// <param name="controller">The controller.</param>
		void HandleAddedController(
			Project project,
			IProjectPlugin controller);

		/// <summary>
		/// Called when a plugin controller is removed from a project.
		/// </summary>
		/// <param name="project">The project.</param>
		/// <param name="controller">The controller.</param>
		void HandleRemovedController(
			Project project,
			IProjectPlugin controller);

		/// <summary>
		/// Initializes the plugin framework after it is added to the system. This will
		/// always be called after the plugin is added.
		/// </summary>
		/// <param name="project">The project.</param>
		/// <param name="controllers">The controllers already enabled in the project. This list will not include the current controller.</param>
		void InitializePluginFramework(
			Project project,
			IEnumerable<IProjectPlugin> controllers);

		#endregion
	}
}
