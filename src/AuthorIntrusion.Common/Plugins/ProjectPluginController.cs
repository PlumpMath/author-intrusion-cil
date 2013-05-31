// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

namespace AuthorIntrusion.Common.Plugins
{
	/// <summary>
	/// Defines a specific instance of an IProjectPlugin along with its configuration,
	/// current state, and settings.
	/// </summary>
	public class ProjectPluginController
	{
		#region Properties

		public IProjectPluginController Controller { get; set; }

		public bool IsImmediateEditor
		{
			get
			{
				bool isImmediateEditor = Controller is IImmediateBlockEditor;
				return isImmediateEditor;
			}
		}

		public string Name
		{
			get { return Plugin.Name; }
		}

		public IProjectPlugin Plugin { get; set; }
		public PluginSupervisor Supervisor { get; set; }

		#endregion

		#region Constructors

		public ProjectPluginController(
			PluginSupervisor supervisor,
			IProjectPlugin plugin)
		{
			Supervisor = supervisor;
			Plugin = plugin;
			Controller = plugin.GetController(supervisor.Project);
		}

		#endregion
	}
}
