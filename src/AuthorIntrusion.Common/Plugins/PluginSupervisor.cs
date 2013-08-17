// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AuthorIntrusion.Common.Actions;
using AuthorIntrusion.Common.Blocks;
using AuthorIntrusion.Common.Blocks.Locking;
using AuthorIntrusion.Common.Commands;

namespace AuthorIntrusion.Common.Plugins
{
	/// <summary>
	/// Project-based manager of plugins that are enabled and configured for a given
	/// project.
	/// </summary>
	public class PluginSupervisor
	{
		#region Properties

		/// <summary>
		/// Gets the <see cref="IProjectPlugin"/> with the specified plugin key.
		/// </summary>
		/// <value>
		/// The <see cref="IProjectPlugin"/>.
		/// </value>
		/// <param name="pluginKey">The plugin key.</param>
		/// <returns>The IProjectPlugin of the given key.</returns>
		/// <exception cref="System.Collections.Generic.KeyNotFoundException">Cannot find plugin with Key of  + pluginKey</exception>
		public IProjectPlugin this[string pluginKey]
		{
			get
			{
				foreach (ProjectPluginController controller in Controllers)
				{
					if (controller.ProjectPlugin.Key == pluginKey)
					{
						return controller.ProjectPlugin;
					}
				}

				throw new KeyNotFoundException(
					"Cannot find plugin with Key of " + pluginKey);
			}
		}

		public IList<IBlockAnalyzerProjectPlugin> BlockAnalyzers { get; private set; }
		public IList<ProjectPluginController> Controllers { get; private set; }
		public IList<IImmediateEditorProjectPlugin> ImmediateEditors { get; private set; }

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
			IProjectPluginProviderPlugin plugin;

			if (!PluginManager.TryGetProjectPlugin(pluginName, out plugin))
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

			// See if this is a plugin framework controller. If it is, we all
			// it to connect to any existing plugins.
			IProjectPlugin pluginController = projectPlugin.ProjectPlugin;
			var frameworkController = pluginController as IFrameworkProjectPlugin;

			if (frameworkController != null)
			{
				var pluginControllers = new List<IProjectPlugin>();

				foreach (ProjectPluginController currentPlugin in Controllers)
				{
					pluginControllers.Add(currentPlugin.ProjectPlugin);
				}

				frameworkController.InitializePluginFramework(Project, pluginControllers);
			}

			// Go through the list of existing plugin frameworks and see if they want to
			// add this one to their internal management.
			foreach (ProjectPluginController controller in Controllers)
			{
				frameworkController = controller.ProjectPlugin as IFrameworkProjectPlugin;

				if (frameworkController != null)
				{
					frameworkController.HandleAddedController(Project, pluginController);
				}
			}

			// Add the controllers to the list.
			Controllers.Add(projectPlugin);

			// Because we've made changes to the plugin, we need to sort and reorder it.
			// This also creates some of the specialized lists required for handling
			// immediate editors (auto-correct).
			UpdatePlugins();

			// We were successful in adding the plugin.
			return true;
		}

		/// <summary>
		/// Clears the analysis state of a block to indicate that all analysis
		/// needs to be completed on the task.
		/// </summary>
		public void ClearAnalysis()
		{
			using (Project.Blocks.AcquireLock(RequestLock.Read))
			{
				foreach (Block block in Project.Blocks)
				{
					block.ClearAnalysis();
				}
			}
		}

		/// <summary>
		/// Clears the analysis for a single plugin.
		/// </summary>
		/// <param name="plugin">The plugin.</param>
		public void ClearAnalysis(IBlockAnalyzerProjectPlugin plugin)
		{
			using (Project.Blocks.AcquireLock(RequestLock.Read))
			{
				foreach (Block block in Project.Blocks)
				{
					block.ClearAnalysis(plugin);
				}
			}
		}

		public bool Contains(string pluginName)
		{
			return Controllers.Any(plugin => plugin.Name == pluginName);
		}

		/// <summary>
		/// Gets the editor actions for a given text span.
		/// </summary>
		/// <param name="block">The block.</param>
		/// <param name="textSpan">The text span.</param>
		/// <returns></returns>
		public IList<IEditorAction> GetEditorActions(
			Block block,
			TextSpan textSpan)
		{
			// Loop through all the text span controllers and add their actions to
			// the list.
			IEnumerable<ProjectPluginController> controllers =
				Controllers.Where(
					controller => controller.ProjectPlugin is ITextControllerProjectPlugin);
			var actions = new List<IEditorAction>();

			foreach (ProjectPluginController controller in controllers)
			{
				var textSpanController =
					(ITextControllerProjectPlugin) controller.ProjectPlugin;
				IList<IEditorAction> controllerActions =
					textSpanController.GetEditorActions(block, textSpan);

				actions.AddRange(controllerActions);
			}

			// Return the resulting list of actions.
			return actions;
		}

		/// <summary>
		/// Processes the block analysis on all the blocks in the collection.
		/// </summary>
		public void ProcessBlockAnalysis()
		{
			using (Project.Blocks.AcquireLock(RequestLock.Read))
			{
				foreach (Block block in Project.Blocks)
				{
					ProcessBlockAnalysis(block);
				}
			}
		}

		/// <summary>
		/// Processes any block analysis on the given block.
		/// </summary>
		/// <param name="block">The block.</param>
		public async void ProcessBlockAnalysis(Block block)
		{
			// If we don't have any analysis controllers, we don't have to do anything.
			if (BlockAnalyzers.Count == 0)
			{
				return;
			}

			// Keep track of the running tasks so we can wait for them to finish
			// (if needed).
			Interlocked.Increment(ref tasksRunning);

			try
			{
				// Grab information about the block inside a read lock.
				int blockVersion;
				HashSet<IBlockAnalyzerProjectPlugin> analysis;

				using (block.AcquireBlockLock(RequestLock.Read))
				{
					blockVersion = block.Version;
					analysis = block.GetAnalysis();
				}

				// Create a background task that will analyze the block. This will return
				// false if the block had changed in the process of analysis (which would
				// have triggered another background task).
				var analyzer = new BlockAnalyzer(
					block, blockVersion, BlockAnalyzers, analysis);
				Task task = Task.Factory.StartNew(analyzer.Run);

				// Wait for the task to complete in the background so we can then
				// decrement our running counter.
				await task;
			}
			finally
			{
				// Decrement the counter to keep track of running tasks.
				Interlocked.Decrement(ref tasksRunning);
			}
		}

		/// <summary>
		/// Checks for immediate edits on a block. This is intended to be a blocking
		/// editing that will always happen within a write lock.
		/// </summary>
		/// <param name="context">The context of the edit just performed.</param>
		/// <param name="block">The block associated with the current changes.</param>
		/// <param name="textIndex">Index of the text inside the block.</param>
		public void ProcessImmediateEdits(
			BlockCommandContext context,
			Block block,
			int textIndex)
		{
			foreach (
				IImmediateEditorProjectPlugin immediateBlockEditor in ImmediateEditors)
			{
				immediateBlockEditor.ProcessImmediateEdits(context, block, textIndex);
			}
		}

		/// <summary>
		/// Waits for all running tasks to complete.
		/// </summary>
		public void WaitForBlockAnalzyers()
		{
			while (tasksRunning > 0)
			{
				Thread.Sleep(100);
			}
		}

		/// <summary>
		/// Sorts and prepares the plugins for use, along with putting various
		/// plugins in the specialized lists for processing immediate edits and
		/// block analysis.
		/// </summary>
		private void UpdatePlugins()
		{
			// Determine the immediate plugins and pull them into separate list.
			ImmediateEditors.Clear();

			foreach (ProjectPluginController controller in
				Controllers.Where(controller => controller.IsImmediateEditor))
			{
				ImmediateEditors.Add(
					(IImmediateEditorProjectPlugin) controller.ProjectPlugin);
			}

			// Determine the block analzyers and put them into their own list.
			BlockAnalyzers.Clear();

			foreach (ProjectPluginController controller in
				Controllers.Where(controller => controller.IsBlockAnalyzer))
			{
				BlockAnalyzers.Add((IBlockAnalyzerProjectPlugin) controller.ProjectPlugin);
			}
		}

		#endregion

		#region Constructors

		public PluginSupervisor(Project project)
		{
			Project = project;
			Controllers = new List<ProjectPluginController>();
			ImmediateEditors = new List<IImmediateEditorProjectPlugin>();
			BlockAnalyzers = new List<IBlockAnalyzerProjectPlugin>();
		}

		#endregion

		#region Fields

		private int tasksRunning;

		#endregion
	}
}
