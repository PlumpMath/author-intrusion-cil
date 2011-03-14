#region Namespaces

using AuthorIntrusion.Contracts.IO;
using AuthorIntrusion.Contracts.Languages;

using StructureMap;

#endregion

namespace AuthorIntrusion
{
	/// <summary>
	/// Manages the entire author process, including extensions.
	/// </summary>
	public static class Manager
	{
		#region IOC

		/// <summary>
		/// Registers the specified instance for a given interface type.
		/// </summary>
		/// <typeparam name="TInterface">The type of the interface.</typeparam>
		/// <param name="instance">The instance.</param>
		public static void Register<TInterface>(TInterface instance)
		{
			ObjectFactory.Inject(instance);
		}

		/// <summary>
		/// Sets up the IoC library and extensions.
		/// </summary>
		public static void Setup()
		{
			// Set up StructureMap with the classes in this assembly and
			// everything in the Extensions folder.
			ObjectFactory.Initialize(
				x => x.Scan(
						scanner =>
						{
							// List the places we are searching for assemblies.
							scanner.AssembliesFromApplicationBaseDirectory();
							scanner.AssembliesFromPath("Extensions");

							// List the common types we need to load.
							scanner.AddAllTypesOf<IInputManager>();
							scanner.AddAllTypesOf<IInputReader>();

							scanner.AddAllTypesOf<IOutputManager>();
							scanner.AddAllTypesOf<IOutputWriter>();

							scanner.AddAllTypesOf<ILanguageManager>();

							scanner.AddAllTypesOf<IContentParser>();
						}));

			//System.Diagnostics.Debug.WriteLine(ObjectFactory.Container.WhatDoIHave());
		}

		#endregion
	}
}