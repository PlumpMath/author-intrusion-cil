#region Namespaces

using System;
using System.Collections.Generic;

using MfGames.Author.Contract.Structures;

#endregion

namespace MfGames.Author.Contract.Collections
{
	/// <summary>
	/// Implements a list that manages structure elements.
	/// </summary>
	public class StructureList : List<Structure>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="StructureBaseList&lt;TStructure&gt;"/> class.
		/// </summary>
		/// <param name="parent">The parent.</param>
		public StructureList(Structure parent)
		{
			this.parent = parent;
		}

		#endregion

		#region Structural Relationship

		private readonly Structure parent;

		/// <summary>
		/// Gets the parent structural element.
		/// </summary>
		/// <value>The parent.</value>
		public Structure Parent
		{
			get { return parent; }
		}

		#endregion

		#region List Operations

		/// <summary>
		/// Adds the specified structure to the list.
		/// </summary>
		/// <param name="structure">The structure.</param>
		public new void Add(Structure structure)
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