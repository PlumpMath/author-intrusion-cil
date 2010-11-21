#region Namespaces

using System;

using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Castle.Windsor.Installer;

using MfGames.Author.Contract.IO;
using MfGames.Author.IO;

#endregion

namespace MfGames.Author
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
			// Set up Windsor container along with the extensions.
			container = new WindsorContainer();
			container.Install(FromAssembly.This());
			container.Kernel.Resolver.AddSubResolver(
				new CollectionResolver(container.Kernel, true));

			container.Register(
				Component.For<IInputManager>().ImplementedBy<InputManager>(),
				Component.For<IOutputManager>().ImplementedBy<OutputManager>());
		}

		#endregion

		#region Destructors

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			if (container != null)
			{
				container.Dispose();
				container = null;
			}
		}

		#endregion

		#region IOC

		private WindsorContainer container;

		#endregion

		#region Managers

		/// <summary>
		/// Gets the input manager.
		/// </summary>
		/// <value>The input manager.</value>
		public IInputManager InputManager
		{
			get { return container.Resolve<IInputManager>(); }
		}

		/// <summary>
		/// Gets the output manager.
		/// </summary>
		/// <value>The output manager.</value>
		public IOutputManager OutputManager
		{
			get { return container.Resolve<IOutputManager>(); }
		}

		#endregion
	}
}