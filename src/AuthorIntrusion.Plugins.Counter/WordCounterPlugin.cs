// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common;
using AuthorIntrusion.Common.Plugins;

namespace AuthorIntrusion.Plugins.Counter
{
	/// <summary>
	/// Plugin to implement an automatic word count plugin that keeps track of word
	/// (and character) counting on a paragraph and structural level.
	/// </summary>
	public class WordCounterPlugin: IProjectPlugin
	{
		#region Properties

		public bool AllowMultiple
		{
			get { return false; }
		}

		public string Name
		{
			get { return "Word Counter"; }
		}

		#endregion

		#region Methods

		public IProjectPluginController GetController(Project project)
		{
			return new WordCounterController();
		}

		#endregion
	}
}
