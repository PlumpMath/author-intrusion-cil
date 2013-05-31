// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System;
using System.IO;
using AuthorIntrusion.Common;
using AuthorIntrusion.Common.Plugins;
using Ninject;
using Ninject.Extensions.Conventions;

namespace AuthorIntrusion
{
	/// <summary>
	/// The resolver is an abstracted layer around an Inversion of Control/Dependency
	/// Injection API. This is not a static singleton class since it is intended to be
	/// used in a single entry application and then referenced again throughout the
	/// system.
	/// </summary>
	public class EnvironmentResolver
	{
		#region Methods

		/// <summary>
		/// Loads and initializes the plugin manager with all the plugins that can
		/// be found in the current environment.
		/// </summary>
		public void LoadPluginManager()
		{
			PluginManager.Instance = kernel.Get<PluginManager>();
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="EnvironmentResolver"/> class.
		/// </summary>
		public EnvironmentResolver()
		{
			// Figure out the standard and plugins path.
			string applicationPath = AppDomain.CurrentDomain.BaseDirectory;

			// Set up Ninject with the standard configuration from the current assembly.
			kernel = new StandardKernel();
			kernel.Bind(
				x =>
					x.FromAssembliesInPath(applicationPath)
					 .SelectAllClasses()
					 .BindAllInterfaces()
					 .Configure(b => b.InSingletonScope()));
		}

		#endregion

		#region Fields

		private readonly IKernel kernel;

		#endregion
	}
}
