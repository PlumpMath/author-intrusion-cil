// <copyright file="TestsBase.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System;

using AuthorIntrusion.Contracts;
using AuthorIntrusion.Contracts.Settings;
using AuthorIntrusion.Storage;
using AuthorIntrusion.Tests.Settings;

using Ninject;

using Xunit.Abstractions;

namespace AuthorIntrusion.Tests
{
	public abstract class TestsBase
	{
		#region Constructors and Destructors

		protected TestsBase(ITestOutputHelper output)
		{
			// Establish our contracts.
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}

			// Set up the Ninject IoC container.
			Kernel = new StandardKernel();

			// Bind an empty, memory-based setting so we always start fresh.
			var memorySettings = new MemorySettings();
			Kernel.Bind<ISettings>().ToConstant(memorySettings);

			// Set up logging for individual tests. This will allow the tests to
			// run in parallel while still writing results to the proper test.
			var logger = new XunitLogger(output);

			Kernel.Bind<ILogger>().ToConstant(logger);

			// Load the rest of the IoC plugins.
			Kernel.Load(
				typeof(StorageProviderManager).Assembly,
				GetType().Assembly);
			Kernel.Load(
				"AuthorIntrusion.Plugin.*.dll",
				"AuthorIntrusion.Tests.Plugin.*.dll");

			// Load the standard known types.
			StorageProviderManager = Kernel.Get<StorageProviderManager>();
		}

		#endregion

		#region Properties

		protected IKernel Kernel { get; private set; }
		protected StorageProviderManager StorageProviderManager { get; set; }

		#endregion
	}
}
