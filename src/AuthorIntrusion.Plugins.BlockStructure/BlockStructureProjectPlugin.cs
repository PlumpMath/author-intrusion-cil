// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Plugins;

namespace AuthorIntrusion.Plugins.BlockStructure
{
	/// <summary>
	/// A project plugin for creating a relationship between blocks within a
	/// document.
	/// </summary>
	public class BlockStructureProjectPlugin: IProjectPlugin
	{
		#region Properties

		public string Key
		{
			get { return "Block Structure"; }
		}

		#endregion
	}
}
