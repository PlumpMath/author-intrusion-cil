#region Namespaces

using AuthorIntrusion.Contracts.Collections;
using AuthorIntrusion.Contracts.Interfaces;

#endregion

namespace AuthorIntrusion.Contracts.Structures
{
	/// <summary>
	/// Represents a single paragraph within the document.
	/// </summary>
	public class Paragraph : Structure, IContentContainer
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Paragraph"/> class.
		/// </summary>
		public Paragraph()
		{
			contents = new ContentList();
		}

		#endregion

		#region Contents

		private readonly ContentList contents;

		/// <summary>
		/// Gets a count of content container content (i.e. paragraphs) in this
		/// object or child objects.
		/// </summary>
		public override int ContentContainerStructureCount
		{
			get
			{
				// This has content, so just include itself. It won't have child
				// structures so the result is just 1.
				return 1;
			}
		}

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