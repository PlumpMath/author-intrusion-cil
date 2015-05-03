// <copyright file="IBufferFormat.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

namespace AuthorIntrusion.IO
{
	/// <summary>
	/// Describes the common interface for working with a buffer format, which may be specific
	/// to a persistence or be general.
	/// </summary>
	public interface IBufferFormat
	{
		#region Public Properties

		/// <summary>
		/// Gets the current settings associated with the format.
		/// </summary>
		IBufferFormatSettings Settings { get; }

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Loads a profile of a specific format into memory. Profiles are either
		/// internally identified by the format and may be stored as part of
		/// a project's settings.
		/// </summary>
		/// <param name="profileName">
		/// The name of the profile to load.
		/// </param>
		void LoadProfile(string profileName);

		/// <summary>
		/// Loads project data from the persistence layer and populates the project.
		/// </summary>
		/// <param name="context">
		/// The context for the load.
		/// </param>
		void LoadProject(BufferLoadContext context);

		/// <summary>
		/// Writes out the project to the given persistence using the 
		/// format instance.
		/// </summary>
		/// <param name="context">
		/// The context of the storing process.
		/// </param>
		void StoreProject(BufferStoreContext context);

		#endregion
	}
}
