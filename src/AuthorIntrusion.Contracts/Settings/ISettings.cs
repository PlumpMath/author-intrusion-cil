// <copyright file="ISettings.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

namespace AuthorIntrusion.Contracts.Settings
{
	/// <summary>
	/// Represents the interface for an object that can provide and persist
	/// settings (including user preferences) for the application.
	/// </summary>
	public interface ISettings
	{
		#region Public Methods and Operators

		/// <summary>
		/// Retrieves a section from the settings object, creating a new one if
		/// it doesn't already exist. The returned section is "live" in that
		/// changes made to it are reflected in the rest of the system, but
		/// those changes aren't persisted until <c>Save</c> is called.
		/// </summary>
		/// <typeparam name="TType"></typeparam>
		/// <param name="sectionName"></param>
		/// <returns></returns>
		TType GetSection<TType>(string sectionName)
			where TType : class, new();

		/// <summary>
		/// Persists any changes in the settings object.
		/// </summary>
		void Save();

		#endregion
	}
}
