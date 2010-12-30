#region Namespaces

using System.Collections.Generic;

using AuthorIntrusion.Contracts.Interfaces;
using AuthorIntrusion.Contracts.Structures;

#endregion

namespace AuthorIntrusion.Contracts
{
	/// <summary>
	/// Top-level class for all elements inside the internal model. The
	/// primary extending classes are Structure and Content.
	/// </summary>
	public abstract class Element
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Element"/> class.
		/// </summary>
		protected Element()
		{
			tags = new HashSet<IElementTag>();
		}

		#endregion

		#region Relationships

		/// <summary>
		/// Gets or sets the parent structure element.
		/// </summary>
		/// <value>The parent.</value>
		public Element Parent { get; set; }

		#endregion

		#region Tags

		private readonly HashSet<IElementTag> tags;

		/// <summary>
		/// Gets the tags associated with this element.
		/// </summary>
		/// <value>The tags.</value>
		public HashSet<IElementTag> Tags
		{
			get { return tags; }
		}

		#endregion
	}
}