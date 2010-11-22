
using System;

using MfGames.Author.Contract.Collections;

namespace MfGames.Author.Contract.Contents
{

	/// <summary>
	/// Defines content that contains other content, such as quotes or
	/// sentences.
	/// </summary>
	public abstract class ContentContainerContent : Content
	{

		public ContentContainerContent()
		{
			contents = new ContentList();
		}

		private ContentList contents;

		public ContentList Contents
		{ get { return contents; }
		}
	}
}
