// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System.Diagnostics;
using AuthorIntrusion.Common.Plugins;
using StructureMap;

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
		/// Gets a specific type of the given instance and returns it.
		/// </summary>
		/// <typeparam name="TResult">The type of the result.</typeparam>
		/// <returns>The TResult item.</returns>
		public TResult Get<TResult>()
		{
			var result = ObjectFactory.GetInstance<TResult>();
			return result;
		}

		/// <summary>
		/// Loads and initializes the plugin manager with all the plugins that can
		/// be found in the current environment.
		/// </summary>
		public void LoadPluginManager()
		{
			PluginManager.Instance = ObjectFactory.GetInstance<PluginManager>();
		}

		/// <summary>
		/// Initializes StructureMap to scan the assemblies and setup all the
		/// needed elements.
		/// </summary>
		/// <param name="init"></param>
		private static void InitializeStructureMap(IInitializationExpression init)
		{
			init.Scan(
				s =>
				{
					// Determine which assemblies contain types we need.
					s.AssembliesFromApplicationBaseDirectory(
						a => a.FullName.Contains("AuthorIntrusion"));

					// List all the assemblies we need. Since most of the
					// plugins are required to register IPlugin, we can just 
					// add those types.
					s.AddAllTypesOf<IPlugin>();
				});
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="EnvironmentResolver"/> class.
		/// </summary>
		public EnvironmentResolver()
		{
			// Set up StructureMap and loading all the plugins from the
			// main directory.
			ObjectFactory.Initialize(InitializeStructureMap);
			string results = ObjectFactory.WhatDoIHave();
			Debug.WriteLine(results);
			ObjectFactory.AssertConfigurationIsValid();
		}

		#endregion
	}
}
