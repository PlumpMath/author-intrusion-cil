#region Namespaces

using AuthorIntrusion.Contracts.IO;

using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

#endregion

namespace AuthorIntrusion.Installers
{
	/// <summary>
	/// Implements the installer for IOutputWriter instances.
	/// </summary>
	public class OutputInstaller : IWindsorInstaller
	{
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
				AllTypes.FromThisAssembly().BasedOn<IOutputWriter>().WithService.
					DefaultInterface());
		}
	}
}