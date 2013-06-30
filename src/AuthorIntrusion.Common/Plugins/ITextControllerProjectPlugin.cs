// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System.Collections.Generic;
using System.Xml;
using AuthorIntrusion.Common.Actions;
using AuthorIntrusion.Common.Blocks;

namespace AuthorIntrusion.Common.Plugins
{
	/// <summary>
	/// Describes the interface of a controller that controls and manages 
	/// <see cref="TextSpan"/> objects inside a block.
	/// </summary>
	public interface ITextControllerProjectPlugin: IProjectPlugin
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

		/// <summary>
		/// Writes out the data stored in a TextSpan created by this controller.
		/// This will only be called if the text span data is not null and a wrapper
		/// tag will already be started before this is called. It is also the 
		/// responsibility of the calling code to close the opened tag.
		/// </summary>
		/// <param name="writer">The writer.</param>
		/// <param name="data">The data.</param>
		void WriteTextSpanData(
			XmlWriter writer,
			object data);

		#endregion
	}
}
