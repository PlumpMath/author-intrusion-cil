// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

namespace AuthorIntrusion.Common.Plugins
{
	/// <summary>
	/// Defines a specific instance of an IPlugin along with its configuration,
	/// current state, and settings.
	/// </summary>
	public class ProjectPluginController
	{
		#region Properties

		public bool IsBlockAnalyzer
		{
			get
			{
				bool isImmediateEditor = ProjectPlugin is IBlockAnalyzerProjectPlugin;
				return isImmediateEditor;
			}
		}

		public bool IsImmediateEditor
		{
			get
			{
				bool isImmediateEditor = ProjectPlugin is IImmediateEditorProjectPlugin;
				return isImmediateEditor;
			}
		}

		public string Name
		{
			get { return Plugin.Key; }
		}

		public IProjectPluginProviderPlugin Plugin { get; set; }
		public IProjectPlugin ProjectPlugin { get; set; }
		public PluginSupervisor Supervisor { get; set; }

		#endregion

		#region Constructors

		public ProjectPluginController(
			PluginSupervisor supervisor,
			IProjectPluginProviderPlugin plugin)
		{
			Supervisor = supervisor;
			Plugin = plugin;
			ProjectPlugin = plugin.GetProjectPlugin(supervisor.Project);
		}

		#endregion
	}
}
