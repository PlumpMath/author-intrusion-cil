#region Namespaces

using System.Collections.Generic;

using MfGames.Author.Contract.Contents;
using MfGames.Author.Contract.Collections;
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
		/// Initializes a new instance of the <see cref="Paragraph"/> class.
		/// </summary>
		public ContentContainerStructure()
		{
			contents = new ContentList();
		}

		#endregion

		#region Contents

		private readonly ContentList contents;

		public ContentList Contents
		{
			get { return contents; }
		}

		#endregion
	}
}