// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common;
using AuthorIntrusion.Common.Plugins;

namespace AuthorIntrusion.Plugins.BlockStructure
{
	/// <summary>
	/// Implements a project plugin provider that establishes a relationship
	/// between the various blocks to create a structure of elements, such as
	/// a book containing chapters containing paragraphs.
	/// </summary>
	public class BlockStructurePlugin: IProjectPluginProviderPlugin
	{
		#region Properties

		public bool AllowMultiple
		{
			get { return false; }
		}

		public string Key
		{
			get { return "Block Structure"; }
		}

		#endregion

		#region Methods

		public IProjectPlugin GetProjectPlugin(Project project)
		{
			var projectPlugin = new BlockStructureProjectPlugin();
			return projectPlugin;
		}

		#endregion
	}
}
