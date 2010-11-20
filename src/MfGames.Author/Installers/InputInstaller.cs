#region Namespaces

using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

using MfGames.Author.Contract.IO;

#endregion

namespace MfGames.Author.Installer
{
	/// <summary>
	/// Implements the installer for IInputReader instances.
	/// </summary>
	public class InputInstaller : IWindsorInstaller
	{
		#region Implementation of IWindsorInstaller

		/// <summary>
		/// Performs the installation in the <see cref="T:Castle.Windsor.IWindsorContainer"/>.
		/// </summary>
		/// <param name="container">The container.</param><param name="store">The configuration store.</param>
		public void Install(
			IWindsorContainer container,
			IConfigurationStore store)
		{
			// Register the individual input components.
			container.Register(
				AllTypes.FromThisAssembly()
				.BasedOn<IInputReader>()
				.WithService.DefaultInterface(),
				AllTypes.FromAssemblyInDirectory(new AssemblyFilter("Extensions", "*.dll"))
				.BasedOn<IInputReader>()
				.WithService.DefaultInterface());
				
			//    Component.For<IFoo>().ImplementedBy<ThisFoo>(),
			//                   Component.For<IFoo>().ImplementedBy<ThatFoo>() < b class=
			//"searchkeyword" > AllTypes </
			//b >.
			//FromThisAssembly().Pick().WithService.DefaultInterface();
			//)
			//;
		}

		#endregion
	}
}