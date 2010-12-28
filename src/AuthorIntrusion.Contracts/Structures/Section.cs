#region Namespaces

using AuthorIntrusion.Contracts.Collections;
using AuthorIntrusion.Contracts.Interfaces;

#endregion

namespace AuthorIntrusion.Contracts.Structures
{
	/// <summary>
	/// Defines the common functionality of a structure that contains other
	/// structures. For example, a DocBook 5's &lt;book /&gt; element can
	/// contain other structural elements.
	/// </summary>
	public class Section : Structure, IStructureContainer
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Section"/> class.
		/// </summary>
		public Section()
		{
			structures = new StructureList(this);
		}

		#endregion

		#region Structures

		private readonly StructureList structures;

		/// <summary>
		/// Gets a count of content container content (i.e. paragraphs) in this
		/// object or child objects.
		/// </summary>
		public override int ContentContainerStructureCount
		{
			get
			{
				int count = 0;

				foreach (Structure structure in structures)
				{
					count += structure.ContentContainerStructureCount;
				}

				return count;
			}
		}

		/// <summary>
		/// Contains a list of structures inside the container.
		/// </summary>
		/// <value>The child structures or an empty list.</value>
		public StructureList Structures
		{
			get { return structures; }
		}

		#endregion
	}
}