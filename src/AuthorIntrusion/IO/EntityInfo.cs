// <copyright file="EntityInfo.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

namespace AuthorIntrusion.IO
{
	/// <summary>
	/// Describes the basic information for an entity in the system. This can be a character
	/// in the novel, a location or object, or even a contributor to the project.
	/// </summary>
	public class EntityInfo
	{
		#region Constructors and Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="EntityInfo"/> class.
		/// </summary>
		public EntityInfo()
		{
			Names = new NameDictionary();
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets the names of this instance.
		/// </summary>
		public NameDictionary Names { get; private set; }

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return Names.ToString();
		}

		#endregion
	}
}
