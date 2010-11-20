#region Namespaces

using System;
using System.Collections.Generic;

#endregion

namespace MfGames.Author.Contract.Structures.Collections
{
	/// <summary>
	/// Implements a list that manages structure base elements.
	/// </summary>
	/// <typeparam name="TStructure">The type of the structure.</typeparam>
	public class StructureBaseList<TStructure> : List<TStructure>
		where TStructure: StructureBase
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="StructureBaseList&lt;TStructure&gt;"/> class.
		/// </summary>
		/// <param name="parent">The parent.</param>
		public StructureBaseList(StructureBase parent)
		{
			this.parent = parent;
		}

		#endregion

		#region Structural Relationship

		private readonly StructureBase parent;

		/// <summary>
		/// Gets the parent structural element.
		/// </summary>
		/// <value>The parent.</value>
		public StructureBase Parent
		{
			get { return parent; }
		}

		#endregion

		#region List Operations

		/// <summary>
		/// Adds the specified structure to the list.
		/// </summary>
		/// <param name="structure">The structure.</param>
		public new void Add(TStructure structure)
		{
			if (structure == null)
			{
				throw new ArgumentNullException("structure");
			}

			structure.Parent = parent;

			base.Add(structure);
		}

		#endregion

	}
}