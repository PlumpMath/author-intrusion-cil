// <copyright file="FileSystemModule.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using AuthorIntrusion.Contracts.Storage;

using Ninject.Modules;

namespace AuthorIntrusion.Plugin.IO.FileSystem
{
	public class FileSystemModule : NinjectModule
	{
		#region Public Methods and Operators

		public override void Load()
		{
			Bind<IStorageProviderFactory>()
				.To<FileSystemStorageProviderFactory>()
				.InSingletonScope();
		}

		#endregion
	}
}
