// <copyright file="BufferStoreContext.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

namespace AuthorIntrusion.IO
{
	/// <summary>
	/// Encapsulates the context for loading buffers.
	/// </summary>
	public class BufferStoreContext : RegionProcessingContext
	{
		#region Constructors and Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="BufferStoreContext"/> class.
		/// </summary>
		/// <param name="project">
		/// The project.
		/// </param>
		/// <param name="persistence">
		/// The persistence.
		/// </param>
		public BufferStoreContext(
			Project project,
			IPersistence persistence)
			: base(project,
				persistence)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BufferStoreContext"/> class.
		/// </summary>
		/// <param name="context">
		/// The context.
		/// </param>
		public BufferStoreContext(BufferStoreContext context)
			: base(context)
		{
		}

		#endregion
	}
}
