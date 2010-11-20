#region Namespaces

using System;
using System.IO;

using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Castle.Windsor.Installer;

using MfGames.Author.Contract.Interfaces;
using MfGames.Author.Installer;
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
		public Manager ()
		{
			// Set up Windsor container along with the extensions.
			windsor = new WindsorContainer();
			windsor.Install(FromAssembly.This());
			windsor.Kernel.Resolver.AddSubResolver(new CollectionResolver(windsor.Kernel, true));
			windsor.Register(
				Component.For<IInputManager>().ImplementedBy<InputManager>());
		}
		
		#endregion
		
		#region Destructors

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose ()
		{
			if (windsor != null)
			{
				windsor.Dispose ();
				windsor = null;
			}
		}
		
		#endregion
		
		#region IOC
		
		private WindsorContainer windsor;
		
		#endregion

		#region Managers

		/// <summary>
		/// Gets the input manager.
		/// </summary>
		/// <value>The input manager.</value>
		public IInputManager InputManager
		{
			get { return windsor.Resolve<IInputManager>(); }
		}

		#endregion
	}
}
