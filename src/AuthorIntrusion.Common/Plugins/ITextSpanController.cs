// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Actions;
using AuthorIntrusion.Common.Blocks;
using C5;

namespace AuthorIntrusion.Common.Plugins
{
	/// <summary>
	/// Describes the interface of a controller that controls and manages 
	/// <see cref="TextSpan"/> objects inside a block.
	/// </summary>
	public interface ITextSpanController: IProjectPluginController
	{
		#region Methods

		/// <summary>
		/// Gets the editor actions associated with the given TextSpan.
		/// </summary>
		/// <param name="block">The block.</param>
		/// <param name="textSpan">The text span.</param>
		/// <returns>
		/// A list of editor actions associated with this span.
		/// </returns>
		/// <remarks>
		/// This will be called within a read-only lock.
		/// </remarks>
		IList<IEditorAction> GetEditorActions(
			Block block,
			TextSpan textSpan);

		#endregion
	}
}
