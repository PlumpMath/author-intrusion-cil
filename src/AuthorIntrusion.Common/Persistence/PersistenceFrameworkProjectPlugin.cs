// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Plugins;

namespace AuthorIntrusion.Common.Persistence
{
	public class PersistenceFrameworkProjectPlugin: IProjectPlugin
	{
		#region Properties

		public string Key
		{
			get { return "Persistence Framework"; }
		}

		#endregion

		#region Constructors

		public PersistenceFrameworkProjectPlugin(Project project)
		{
		}

		#endregion
	}
}
