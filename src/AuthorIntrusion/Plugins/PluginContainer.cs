// <copyright file="PluginContainer.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using StructureMap;
using StructureMap.Configuration.DSL;

namespace AuthorIntrusion.Plugins
{
	/// <summary>
	/// Implements the primary container for the IoC implementation.
	/// </summary>
	public class PluginContainer : Container
	{
		#region Constructors and Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="PluginContainer"/> class.
		/// </summary>
		/// <param name="additionalRegistries">
		/// The additional registries.
		/// </param>
		public PluginContainer(params Registry[] additionalRegistries)
			: base(new PluginRegistry(additionalRegistries))
		{
		}

		#endregion
	}
}
