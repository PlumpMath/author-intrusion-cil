// <copyright file="CliRegistry.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using AuthorIntrusion.Cli.Transform;

using StructureMap.Configuration.DSL;

namespace AuthorIntrusion.Cli
{
	/// <summary>
	/// Implements the StructureMap registry for this assembly.
	/// </summary>
	public class CliRegistry : Registry
	{
		#region Constructors and Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="CliRegistry"/> class.
		/// </summary>
		public CliRegistry()
		{
			For<TransformCommand>()
				.Singleton();
		}

		#endregion
	}
}
