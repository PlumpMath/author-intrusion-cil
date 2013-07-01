// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common;
using AuthorIntrusion.Common.Plugins;

namespace AuthorIntrusion.Plugins.Spelling.LocalWords
{
	public class LocalWordsPlugin: IProjectPluginProviderPlugin
	{
		#region Properties

		public bool AllowMultiple
		{
			get { return false; }
		}

		public string Key
		{
			get { return "Local Words"; }
		}

		#endregion

		#region Methods

		public IProjectPlugin GetProjectPlugin(Project project)
		{
			return new LocalWordsProjectPlugin(project);
		}

		#endregion
	}
}
