// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Blocks;
using C5;

namespace AuthorIntrusion.Common.Plugins
{
	/// <summary>
	/// Project-based manager of plugins that are enabled and configured for a given
	/// project.
	/// </summary>
	public class PluginSupervisor
	{
		#region Properties

		public IList<ProjectPluginController> Controllers { get; private set; }
		public IList<ProjectPluginController> ImmediateEditors { get; private set; }

		/// <summary>
		/// Gets the PluginManager associated with the environment.
		/// </summary>
		public PluginManager PluginManager
		{
			get { return PluginManager.Instance; }
		}

		public Project Project { get; private set; }

		#endregion

		#region Methods

		/// <summary>
		/// Adds the specified plugin by name from the PluginManager and sets up the
		/// elements inside the project.
		/// </summary>
		/// <param name="pluginName">Name of the plugin.</param>
		/// <returns></returns>
		public bool Add(string pluginName)
		{
			// Look up the plugin from the plugin manager.
			IProjectPlugin plugin;

			if (!PluginManager.TryGet(pluginName, out plugin))
			{
				// We couldn't find the plugin inside the manager.
				return false;
			}

			// If the plugin doesn't allow duplicates, then check to see if we
			// already have a configuration object associated with this plugin.
			if (!plugin.AllowMultiple
				&& Contains(pluginName))
			{
				// We can't add a new one since we already have one.
				return false;
			}

			// We can add this plugin to the project (either as a duplicate or
			// as the first). In all cases, we get a flyweight wrapper around the
			// plugin and add it to the ordered list of project-specific plugins.
			var projectPlugin = new ProjectPluginController(this, plugin);

			Controllers.Add(projectPlugin);

			// Because we've made changes to the plugin, we need to sort and reorder it.
			// This also creates some of the specialized lists required for handling
			// immediate editors (auto-correct).
			UpdatePlugins();

			// We were successful in adding the plugin.
			return true;
		}

		/// <summary>
		/// Checks for immediate edits on a block. This is intended to be a blocking
		/// editing that will always happen within a write lock.
		/// </summary>
		/// <param name="block">The block.</param>
		/// <param name="textIndex">Index of the text.</param>
		public void CheckForImmediateEdits(
			Block block,
			int textIndex)
		{
			foreach (ProjectPluginController controller in Controllers)
			{
				var immediateBlockEditor = controller.Controller as IImmediateBlockEditor;

				if (immediateBlockEditor != null)
				{
					immediateBlockEditor.CheckForImmediateEdits(block, textIndex);
				}
			}
		}

		public bool Contains(string pluginName)
		{
			foreach (ProjectPluginController plugin in Controllers)
			{
				if (plugin.Name == pluginName)
				{
					return true;
				}
			}

			return false;
		}

		private void UpdatePlugins()
		{
			// Sort the plugins so they are in execution order.

			// Determine the immediate plugins and pull them into separate list.
			ImmediateEditors.Clear();

			foreach (ProjectPluginController controller in Controllers)
			{
				if (controller.IsImmediateEditor)
				{
					ImmediateEditors.Add(controller);
				}
			}
		}

		#endregion

		#region Constructors

		public PluginSupervisor(Project project)
		{
			Project = project;
			Controllers = new ArrayList<ProjectPluginController>();
			ImmediateEditors = new ArrayList<ProjectPluginController>();
		}

		#endregion
	}
}
