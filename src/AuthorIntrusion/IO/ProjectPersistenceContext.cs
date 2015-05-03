// <copyright file="ProjectPersistenceContext.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System.Diagnostics.Contracts;

namespace AuthorIntrusion.IO
{
	/// <summary>
	/// A base class for a project operation that requires persistence.
	/// </summary>
	public class ProjectPersistenceContext : ProjectContext
	{
		#region Constructors and Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="ProjectPersistenceContext"/> class.
		/// </summary>
		/// <param name="project">
		/// The project.
		/// </param>
		/// <param name="persistence">
		/// The persistence.
		/// </param>
		public ProjectPersistenceContext(
			Project project,
			IPersistence persistence)
			: base(project)
		{
			Contract.Requires(project != null);
			Contract.Requires(persistence != null);

			Persistence = persistence;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ProjectPersistenceContext"/> class.
		/// </summary>
		/// <param name="context">
		/// The context.
		/// </param>
		public ProjectPersistenceContext(ProjectPersistenceContext context)
			: base(context)
		{
			Contract.Requires(context != null);

			Persistence = context.Persistence;
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets the persistence provider for retrieving data.
		/// </summary>
		/// <value>
		/// The persistence.
		/// </value>
		public IPersistence Persistence { get; private set; }

		#endregion
	}
}
