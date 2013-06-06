// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AuthorIntrusion.Common.Actions;
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

		public C5.IList<IBlockAnalyzerController> BlockAnalyzers { get; private set; }
		public C5.IList<ProjectPluginController> Controllers { get; private set; }
		public C5.IList<IImmediateBlockEditor> ImmediateEditors { get; private set; }

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

			// See if this is a plugin framework controller.
			IProjectPluginController pluginController = projectPlugin.Controller;
			var frameworkController = pluginController as IPluginFrameworkController;

			if (frameworkController != null)
			{
				var pluginControllers = new ArrayList<IProjectPluginController>();

				foreach (ProjectPluginController currentPlugin in Controllers)
				{
					pluginControllers.Add(currentPlugin.Controller);
				}

				frameworkController.InitializePluginFramework(Project, pluginControllers);
			}

			// Go through the list of existing plugin frameworks and see if they want to
			// add this one to their internal management.
			foreach (ProjectPluginController controller in Controllers)
			{
				frameworkController = controller.Controller as IPluginFrameworkController;

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
		/// Called after a block changes it's parent from the oldParentBlock to the
		/// currently assigned block.
		/// </summary>
		/// <param name="block">The block.</param>
		/// <param name="oldParentBlock">The old parent block.</param>
		public void ChangeBlockParent(
			Block block,
			Block oldParentBlock)
		{
			IEnumerable<ProjectPluginController> controllers =
				Controllers.Where(controller => controller is IBlockRelationshipController);

			foreach (ProjectPluginController controller in controllers)
			{
				var relationshipController = (IBlockRelationshipController) controller;
				relationshipController.ChangeBlockParent(block, oldParentBlock);
			}
		}

		/// <summary>
		/// Called after a block changes its type.
		/// </summary>
		/// <param name="block">The block.</param>
		/// <param name="oldBlockType">Old type of the block.</param>
		public void ChangeBlockType(
			Block block,
			BlockType oldBlockType)
		{
			IEnumerable<ProjectPluginController> controllers =
				Controllers.Where(controller => controller is IBlockTypeController);

			foreach (ProjectPluginController controller in controllers)
			{
				var blockTypeController = (IBlockTypeController) controller;
				blockTypeController.ChangeBlockType(block, oldBlockType);
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
		public C5.IList<IEditorAction> GetEditorActions(
			Block block,
			TextSpan textSpan)
		{
			// Loop through all the text span controllers and add their actions to
			// the list.
			IEnumerable<ProjectPluginController> controllers =
				Controllers.Where(
					controller => controller.Controller is ITextSpanController);
			var actions = new ArrayList<IEditorAction>();

			foreach (ProjectPluginController controller in controllers)
			{
				var textSpanController = (ITextSpanController) controller.Controller;
				C5.IList<IEditorAction> controllerActions =
					textSpanController.GetEditorActions(block, textSpan);

				actions.AddAll(controllerActions);
			}

			// Return the resulting list of actions.
			return actions;
		}

		/// <summary>
		/// Processes any block analysis on the given block.
		/// </summary>
		/// <param name="block">The block.</param>
		public async void ProcessBlockAnalysis(Block block)
		{
			// If we don't have any analysis controllers, we don't have to do anything.
			if (BlockAnalyzers.IsEmpty)
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

				using (block.AcquireReadLock())
				{
					blockVersion = block.Version;
				}

				// Create a background task that will analyze the block. This will return
				// false if the block had changed in the process of analysis (which would
				// have triggered another background task).
				var analyzer = new BlockAnalyzer(block, blockVersion, BlockAnalyzers);
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
		/// <param name="block">The block.</param>
		/// <param name="textIndex">Index of the text.</param>
		public void ProcessImmediateEdits(
			Block block,
			int textIndex)
		{
			foreach (IImmediateBlockEditor immediateBlockEditor in ImmediateEditors)
			{
				immediateBlockEditor.ProcessImmediateEdits(block, textIndex);
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

		private void UpdatePlugins()
		{
			// Sort the plugins so they are in execution order.

			// Determine the immediate plugins and pull them into separate list.
			ImmediateEditors.Clear();

			foreach (ProjectPluginController controller in
				Controllers.Where(controller => controller.IsImmediateEditor))
			{
				ImmediateEditors.Add(controller.Controller);
			}

			// Determine the block analzyers and put them into their own list.
			BlockAnalyzers.Clear();

			foreach (ProjectPluginController controller in
				Controllers.Where(controller => controller.IsBlockAnalyzer))
			{
				BlockAnalyzers.Add(controller.Controller);
			}
		}

		#endregion

		#region Constructors

		public PluginSupervisor(Project project)
		{
			Project = project;
			Controllers = new ArrayList<ProjectPluginController>();
			ImmediateEditors = new ArrayList<IImmediateBlockEditor>();
			BlockAnalyzers = new ArrayList<IBlockAnalyzerController>();
		}

		#endregion

		#region Fields

		private int tasksRunning;

		#endregion
	}
}
