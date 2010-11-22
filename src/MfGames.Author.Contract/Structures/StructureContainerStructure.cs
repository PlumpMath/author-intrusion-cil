using System;

using MfGames.Author.Contract.Collections;
using MfGames.Author.Contract.Interfaces;

namespace MfGames.Author.Contract.Structures
{
	/// <summary>
	/// Defines the common functionality of a structure that contains other
	/// structures. For example, a DocBook 5's &lt;book /&gt; element can
	/// contain other structural elements.
	/// </summary>
	public class StructureContainerStructure : Structure, IStructureContainer
	{
		public StructureContainerStructure()
		{
			structures = new StructureList(this);
		}

		#region Structures

		private StructureList structures;

		public StructureList Structures
		{
			get { return structures; }
		}

		#endregion
	}
}
