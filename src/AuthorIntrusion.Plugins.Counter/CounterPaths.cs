// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Blocks;
using MfGames.HierarchicalPaths;

namespace AuthorIntrusion.Plugins.Counter
{
	public static class CounterPaths
	{
		#region Methods

		public static HierarchicalPath GetPath(BlockType blockType)
		{
			var path = new HierarchicalPath(blockType.Name, BlockTypePath);
			return path;
		}

		#endregion

		#region Constructors

		static CounterPaths()
		{
			BlockTypePath = new HierarchicalPath("/Plugins/Counter/Type Counts");
			CharacterCountPath = new HierarchicalPath("/Plugins/Counter/Character Count");
			NonWhitespaceCountPath =
				new HierarchicalPath("/Plugins/Counter/Non-Whitespace Count");
			WordCountPath = new HierarchicalPath("/Plugins/Counter/Word Count");

			StandardCounterPaths = new[]
			{
				WordCountPath, CharacterCountPath, NonWhitespaceCountPath
			};
		}

		#endregion

		#region Fields

		public static readonly HierarchicalPath BlockTypePath;

		public static readonly HierarchicalPath CharacterCountPath;

		public static readonly HierarchicalPath NonWhitespaceCountPath;

		public static readonly HierarchicalPath[] StandardCounterPaths;
		public static readonly HierarchicalPath WordCountPath;

		#endregion
	}
}
