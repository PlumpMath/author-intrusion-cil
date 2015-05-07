// <copyright file="ILogger.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System;

using MfGames.Enumerations;

namespace AuthorIntrusion.Contracts
{
	public interface ILogger
	{
		#region Public Methods and Operators

		void Log(
			Type type,
			Severity severity,
			string format,
			params object[] arguments);

		#endregion
	}
}
