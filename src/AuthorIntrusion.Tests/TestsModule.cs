// <copyright file="TestsModule.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using AuthorIntrusion.Contracts.Storage;
using AuthorIntrusion.Tests.Storage;

using Ninject.Modules;

namespace AuthorIntrusion.Tests
{
	public class TestsModule : NinjectModule
	{
		#region Public Methods and Operators

		public override void Load()
		{
			Bind<IStorageProviderFactory>()
				.To<MemoryStorageProviderFactory>()
				.InSingletonScope();
		}

		#endregion
	}
}
