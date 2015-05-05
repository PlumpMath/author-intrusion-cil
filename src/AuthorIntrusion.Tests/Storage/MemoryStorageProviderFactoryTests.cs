// <copyright file="MemoryStorageProviderFactoryTests.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using AuthorIntrusion.Contracts.Storage;

using Xunit;
using Xunit.Abstractions;

namespace AuthorIntrusion.Tests.Storage
{
	public class MemoryStorageProviderFactoryTests : TestsBase
	{
		#region Constructors and Destructors

		public MemoryStorageProviderFactoryTests(ITestOutputHelper output)
			: base(output)
		{
		}

		#endregion

		#region Public Methods and Operators

		[Fact]
		public void VerifyStandardPluginsAreLoaded()
		{
			IStorageProviderFactory fileSystemFactory =
				StorageProviderManager.GetStorageProviderFactory("filesystem");

			Assert.NotNull(fileSystemFactory);
		}

		[Fact]
		public void VerifyTestPluginsAreLoaded()
		{
			IStorageProviderFactory memoryFactory =
				StorageProviderManager.GetStorageProviderFactory("memory");

			Assert.NotNull(memoryFactory);
		}

		#endregion
	}
}
