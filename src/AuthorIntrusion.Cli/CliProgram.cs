// <copyright file="CliProgram.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System;

using AuthorIntrusion.Storage;

using Ninject;

namespace AuthorIntrusion.Cli
{
	/// <summary>
	/// Main program for the command-line interface (CLI).
	/// </summary>
	public class CliProgram
	{
		#region Methods

		/// <summary>
		/// Main entry point for the CLI application.
		/// </summary>
		/// <param name="args">
		/// The arguments from the command line.
		/// </param>
		private static void Main(string[] args)
		{
			// Hook up the dependency injection for the entire application.
			var kernel = new StandardKernel();

			kernel.Load(
				typeof(StorageProviderManager).Assembly);
			kernel.Load(
				"AuthorIntrusion.Plugin.*.dll",
				"AuthorIntrusion.Cli.Plugin.*.dll");

			var results = kernel.Get<StorageProviderManager>();
			Console.WriteLine(results);
		}

		#endregion
	}
}
