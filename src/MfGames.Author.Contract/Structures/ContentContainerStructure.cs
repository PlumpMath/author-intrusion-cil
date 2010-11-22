#region Namespaces

using System;

using MfGames.Author.Contract.Collections;
using MfGames.Author.Contract.Delegates;
using MfGames.Author.Contract.Interfaces;

#endregion

namespace MfGames.Author.Contract.Structures
{
	/// <summary>
	/// Represents a single paragraph within the document.
	/// </summary>
	public class ContentContainerStructure : Structure, IContentContainer
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="ContentContainerStructure"/> class.
		/// </summary>
		public ContentContainerStructure()
		{
			contents = new ContentList();
		}

		#endregion

		#region Contents

		private readonly ContentList contents;

		/// <summary>
		/// Gets the contents inside the structure.
		/// </summary>
		/// <value>The contents.</value>
		public ContentList Contents
		{
			get { return contents; }
		}

		/// <summary>
		/// Gets a flattened string that represents the entire container.
		/// </summary>
		/// <value>The content string.</value>
		public string ContentString
		{
			get { return contents.ContentString; }
		}

		#endregion
	}
}