#region Namespaces

using AuthorIntrusion.Contracts.Collections;
using AuthorIntrusion.Contracts.Interfaces;

#endregion

namespace AuthorIntrusion.Contracts.Contents
{
	/// <summary>
	/// Defines content that contains other content, such as quotes or
	/// sentences.
	/// </summary>
	public abstract class ContentContainerContent : Content, IContentContainer
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="ContentContainerContent"/> class.
		/// </summary>
		protected ContentContainerContent()
		{
			contents = new ContentList();
		}

		#endregion

		#region Contents

		private readonly ContentList contents;

		/// <summary>
		/// Gets the child contents inside this container.
		/// </summary>
		/// <value>The contents.</value>
		public ContentList Contents
		{
			get { return contents; }
		}

		/// <summary>
		/// Contains a flattened representation of the content.
		/// </summary>
		/// <value>The content string.</value>
		public override string ContentString
		{
			get { return contents.ContentString; }
		}

		#endregion
	}
}