#region Namespaces

using AuthorIntrusion.Contracts.Languages;

using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

#endregion

namespace AuthorIntrusion.EnglishSharpNlp
{
	/// <summary>
	/// Installs the Windsor components for the EnglishSharpNlp project.
	/// </summary>
	public class EnglishSharpNlpInstaller : IWindsorInstaller
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
				AllTypes.FromThisAssembly().BasedOn<IContentParser>().WithService.
					DefaultInterface());
		}
	}
}