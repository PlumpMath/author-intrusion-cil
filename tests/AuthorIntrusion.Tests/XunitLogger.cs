// <copyright file="XunitLogger.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System;

using AuthorIntrusion.Contracts;

using MfGames.Enumerations;

using Xunit.Abstractions;

namespace AuthorIntrusion.Tests
{
	public class XunitLogger : ILogger
	{
		#region Fields

		private readonly ITestOutputHelper output;

		#endregion

		#region Constructors and Destructors

		public XunitLogger(ITestOutputHelper output)
		{
			this.output = output;
		}

		#endregion

		#region Public Methods and Operators

		public void Log(
			Type type,
			Severity severity,
			string format,
			params object[] arguments)
		{
			string message = string.Format(format, arguments);
			string formatted = string.Format(
				"{0}: {1}",
				severity.ToString().PadRight(5),
				message);

			output.WriteLine(formatted);
		}

		#endregion
	}
}
