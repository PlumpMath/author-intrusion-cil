// <copyright file="TestsBase.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System;

using Xunit.Abstractions;

namespace AuthorIntrusion.Tests
{
	public abstract class TestsBase
	{
		#region Constructors and Destructors

		protected TestsBase(ITestOutputHelper output)
		{
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}

			Output = output;
		}

		#endregion

		#region Properties

		protected ITestOutputHelper Output { get; private set; }

		#endregion
	}
}
