// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System;
using AuthorIntrusion.Common.Commands;
using MfGames.Enumerations;
using MfGames.HierarchicalPaths;

namespace AuthorIntrusion.Common.Actions
{
	public class EditorAction: IEditorAction
	{
		#region Properties

		public string DisplayName { get; private set; }
		public Importance Importance { get; private set; }
		public HierarchicalPath ResourceKey { get; private set; }
		private Action<BlockCommandContext> Action { get; set; }

		#endregion

		#region Methods

		public void Do(BlockCommandContext context)
		{
			Action(context);
		}

		#endregion

		#region Constructors

		public EditorAction(
			string displayName,
			HierarchicalPath resourceKey,
			Action<BlockCommandContext> action,
			Importance importance = Importance.Normal)
		{
			DisplayName = displayName;
			Importance = importance;
			ResourceKey = resourceKey;
			Action = action;
		}

		#endregion
	}
}
