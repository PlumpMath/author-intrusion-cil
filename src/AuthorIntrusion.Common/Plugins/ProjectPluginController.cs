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

		public IProjectPlugin Controller { get; set; }

		public bool IsBlockAnalyzer
		{
			get
			{
				bool isImmediateEditor = Controller is IBlockAnalyzerProjectPlugin;
				return isImmediateEditor;
			}
		}

		public bool IsImmediateEditor
		{
			get
			{
				bool isImmediateEditor = Controller is IImmediateEditorProjectPlugin;
				return isImmediateEditor;
			}
		}

		public string Name
		{
			get { return Plugin.Name; }
		}

		public IPlugin Plugin { get; set; }
		public PluginSupervisor Supervisor { get; set; }

		#endregion

		#region Constructors

		public ProjectPluginController(
			PluginSupervisor supervisor,
			IPlugin plugin)
		{
			Supervisor = supervisor;
			Plugin = plugin;
			Controller = plugin.GetProjectPlugin(supervisor.Project);
		}

		#endregion
	}
}
