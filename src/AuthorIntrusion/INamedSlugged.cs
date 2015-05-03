// <copyright file="INamedSlugged.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

namespace AuthorIntrusion
{
	/// <summary>
	/// Indicates that the item has both a name and a slug, used for some extension methods
	/// and selectors.
	/// </summary>
	public interface INamedSlugged : INamed,
		ISlugged
	{
	}
}
