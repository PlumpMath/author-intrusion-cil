using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthorIntrusion.Common;
using Ninject;
using Ninject.Extensions.Conventions;
using Ninject.Extensions.Conventions.Syntax;

namespace AuthorIntrusion
{
	/// <summary>
	/// The resolver is an abstracted layer around an Inversion of Control/Dependency
	/// Injection API. This is not a static singleton class since it is intended to be
	/// used in a single entry application and then referenced again throughout the
	/// system.
	/// </summary>
    public class Resolver
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="Resolver"/> class.
		/// </summary>
		public Resolver()
		{
			// Figure out the standard and plugins path.
			string applicationPath = AppDomain.CurrentDomain.BaseDirectory;
			string pluginsPath = Path.Combine(
				applicationPath,
				"Plugins");

			// Set up Ninject with the standard configuration from the current assembly.
			IKernel kernel = new StandardKernel();

			kernel.Bind(
				x =>
					x.FromThisAssembly()
					 .SelectAllClasses()
					 .WithAttribute<SingletonServiceAttribute>()
					 .BindAllInterfaces()
					 .Configure(b => b.InSingletonScope()));
			kernel.Bind(
				x =>
					x.FromThisAssembly()
					 .SelectAllClasses()
					 .WithoutAttribute<SingletonServiceAttribute>()
					 .BindAllInterfaces());

			// If we have a plugins directory, then also load assemblies from there.
			if (Directory.Exists(pluginsPath))
			{
				kernel.Bind(
					x =>
						x.FromAssembliesInPath(pluginsPath)
						 .SelectAllClasses()
						 .WithAttribute<SingletonServiceAttribute>()
						 .BindAllInterfaces()
						 .Configure(b => b.InSingletonScope()));
				kernel.Bind(
					x =>
						x.FromAssembliesInPath(pluginsPath)
						 .SelectAllClasses()
						 .WithoutAttribute<SingletonServiceAttribute>()
						 .BindAllInterfaces());
			}
		}
    }
}
