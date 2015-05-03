// <copyright file="EntityCollection.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System.Collections.Generic;

namespace AuthorIntrusion.IO
{
	/// <summary>
	/// A custom sequenced collection that contains zero or more entities.
	/// </summary>
	public class EntityCollection : List<EntityInfo>
	{
		#region Public Properties

		/// <summary>
		/// Gets the first (primary) entry in the entity collection.
		/// </summary>
		public EntityInfo First { get { return Count == 0 ? null : this[0]; } }

		#endregion
	}
}
