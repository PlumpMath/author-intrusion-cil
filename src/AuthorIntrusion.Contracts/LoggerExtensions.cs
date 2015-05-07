// <copyright file="LoggerExtensions.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System;

using MfGames.Enumerations;

namespace AuthorIntrusion.Contracts
{
	public static class LoggerExtensions
	{
		#region Public Methods and Operators

		public static void Info(
			this ILogger logger,
			Type type,
			string format,
			params object[] arguments)
		{
			logger.Log(type, Severity.Info, format, arguments);
		}

		#endregion
	}
}
