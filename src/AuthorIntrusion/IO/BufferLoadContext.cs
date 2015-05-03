// <copyright file="BufferLoadContext.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System.Diagnostics.Contracts;

namespace AuthorIntrusion.IO
{
	/// <summary>
	/// Contains the context for loading a buffer into a project.
	/// </summary>
	public class BufferLoadContext : RegionProcessingContext
	{
		#region Constructors and Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="BufferLoadContext"/> class.
		/// </summary>
		/// <param name="project">
		/// The project.
		/// </param>
		/// <param name="persistence">
		/// The persistence.
		/// </param>
		/// <param name="options">
		/// The options.
		/// </param>
		public BufferLoadContext(
			Project project,
			IPersistence persistence,
			BufferLoadOptions options)
			: base(project,
				persistence)
		{
			Contract.Requires(project != null);
			Contract.Requires(persistence != null);

			Options = options;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BufferLoadContext"/> class.
		/// </summary>
		/// <param name="project">
		/// The project.
		/// </param>
		/// <param name="persistence">
		/// The persistence.
		/// </param>
		public BufferLoadContext(
			Project project,
			IPersistence persistence)
			: this(project,
				persistence,
				BufferLoadOptions.Full)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BufferLoadContext"/> class.
		/// </summary>
		/// <param name="context">
		/// The context.
		/// </param>
		public BufferLoadContext(BufferLoadContext context)
			: base(context)
		{
			Contract.Requires(context != null);

			Options = context.Options;
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets the options for loading the buffer.
		/// </summary>
		/// <value>
		/// The options.
		/// </value>
		public BufferLoadOptions Options { get; private set; }

		#endregion
	}
}
