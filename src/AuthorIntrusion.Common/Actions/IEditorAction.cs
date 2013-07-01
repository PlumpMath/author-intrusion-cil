// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Commands;
using MfGames.Enumerations;
using MfGames.HierarchicalPaths;

namespace AuthorIntrusion.Common.Actions
{
	/// <summary>
	/// Defines an abstract editor action which is provided from the various plugins
	/// to the front-end interfaces, such as the GUI.
	/// </summary>
	public interface IEditorAction
	{
		#region Properties

		/// <summary>
		/// Gets the display name suitable for displaying in menus and popups. The
		/// display name may have a "&" in front of a character to indicate the
		/// preferred mnemonic, but if two items have the same shortcut (and in the
		/// same menu), then the one with the lowest Importanance.
		/// </summary>
		string DisplayName { get; }

		/// <summary>
		/// Gets the importance of the editor action. This is used to decide which
		/// items should be on the main menus when there are too many items to
		/// reasonably display on screen at the same time.
		/// </summary>
		Importance Importance { get; }

		/// <summary>
		/// Gets the key for this action. Unlike the display name, this should never
		/// be translated into other languages. It is used to look up additional
		/// resources for the interface, such as icons or help.
		/// </summary>
		HierarchicalPath ResourceKey { get; }

		#endregion

		#region Methods

		/// <summary>
		/// Performs the action associated with this editor action.
		/// </summary>
		void Do(BlockCommandContext context);

		#endregion
	}
}
