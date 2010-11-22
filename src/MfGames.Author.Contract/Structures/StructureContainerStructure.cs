#region Namespaces

using MfGames.Author.Contract.Collections;
using MfGames.Author.Contract.Interfaces;

#endregion

namespace MfGames.Author.Contract.Structures
{
	/// <summary>
	/// Defines the common functionality of a structure that contains other
	/// structures. For example, a DocBook 5's &lt;book /&gt; element can
	/// contain other structural elements.
	/// </summary>
	public class StructureContainerStructure : Structure, IStructureContainer
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="StructureContainerStructure"/> class.
		/// </summary>
		public StructureContainerStructure()
		{
			structures = new StructureList(this);
		}

		#endregion

		#region Structures

		private readonly StructureList structures;

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