// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common;
using AuthorIntrusion.Common.Plugins;

namespace AuthorIntrusion.Plugins.ImmediateBlockTypes
{
	/// <summary>
	/// The plugin for an immediate editor that changes the block type as the author
	/// is writing.
	/// </summary>
	public class ImmediateBlockTypesPlugin: IProjectPluginProviderPlugin
	{
		#region Properties

		public bool AllowMultiple
		{
			get { return false; }
		}

		public string Key
		{
			get { return "Immediate Block Types"; }
		}

		#endregion

		#region Methods

		public IProjectPlugin GetProjectPlugin(Project project)
		{
			return new ImmediateBlockTypesProjectPlugin(project);
		}

		#endregion
	}
}
