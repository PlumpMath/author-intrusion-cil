#region Namespaces

using System;

using AuthorIntrusion.Contracts.IO;
using AuthorIntrusion.Contracts.Languages;

using StructureMap;

#endregion

namespace AuthorIntrusion
{
	/// <summary>
	/// Manages the entire author process, including extensions.
	/// </summary>
	public class Manager : IDisposable
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Manager"/> class.
		/// </summary>
		public Manager()
		{
			// Set up StructureMap with the classes in this assembly and
			// everything in the Extensions folder.
			ObjectFactory.Initialize(
				x => x.Scan(
				     	scanner =>
				     	{
							// List the places we are searching for assemblies.
				     		scanner.TheCallingAssembly();
				     		scanner.AssembliesFromPath("Extensions");

							// List the common types we need to load.
				     		scanner.AddAllTypesOf<IInputManager>();
				     		scanner.AddAllTypesOf<IInputReader>();

				     		scanner.AddAllTypesOf<IOutputManager>();
				     		scanner.AddAllTypesOf<IOutputWriter>();

				     		scanner.AddAllTypesOf<ILanguageManager>();

				     		scanner.AddAllTypesOf<IContentParser>();
				     	}));
		}

		#endregion

		#region Destructors

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			// TODO REMOVE
		}

		#endregion

		#region IOC

		/// <summary>
		/// Registers the specified instance for a given interface type.
		/// </summary>
		/// <typeparam name="TInterface">The type of the interface.</typeparam>
		/// <param name="instance">The instance.</param>
		public void Register<TInterface>(TInterface instance)
		{
			ObjectFactory.Inject(instance);
		}

		#endregion

		#region Managers

		/// <summary>
		/// Gets the input manager.
		/// </summary>
		/// <value>The input manager.</value>
		public IInputManager InputManager
		{
			get
			{
				return ObjectFactory.GetInstance<IInputManager>();
			}
		}

		/// <summary>
		/// Contains the language manager.
		/// </summary>
		/// <value>The language manager.</value>
		public ILanguageManager LanguageManager
		{
			get
			{
				return ObjectFactory.GetInstance<ILanguageManager>();
			}
		}

		/// <summary>
		/// Gets the output manager.
		/// </summary>
		/// <value>The output manager.</value>
		public IOutputManager OutputManager
		{
			get
			{
				return ObjectFactory.GetInstance<IOutputManager>();
			}
		}

		#endregion
	}
}