// <copyright file="MemorySettings.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System;

using AuthorIntrusion.Contracts.Settings;

namespace AuthorIntrusion.Tests.Settings
{
	public class MemorySettings : ISettings
	{
		#region Public Methods and Operators

		public TType GetSection<TType>(string sectionName) where TType : class, new()
		{
			throw new NotImplementedException();
		}

		public void Save()
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
