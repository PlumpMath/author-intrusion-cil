// <copyright file="PluginRegistry.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using AuthorIntrusion.IO;

using StructureMap.Configuration.DSL;

namespace AuthorIntrusion.Plugins
{
	/// <summary>
	/// Defines the StructureMap plugin registry for the primary Author Intrusion DLL.
	/// </summary>
	public class PluginRegistry : Registry
	{
		#region Constructors and Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="PluginRegistry"/> class.
		/// </summary>
		/// <param name="additionalRegistries">
		/// </param>
		public PluginRegistry(params Registry[] additionalRegistries)
		{
			// Set up the persistence from this assembly.
			For<PersistenceFactoryManager>()
				.Singleton();
			For<IPersistenceFactory>()
				.Add<FilePersistenceFactory>()
				.Singleton();
			For<IFileBufferFormatFactory>()
				.Add<MarkdownBufferFormatFactory>();
			For<IFileBufferFormatFactory>()
				.Add<DocBookBufferFormatFactory>();

			// Add in the additional registries.
			foreach (Registry registry in additionalRegistries)
			{
				IncludeRegistry(registry);
			}
		}

		#endregion
	}
}
