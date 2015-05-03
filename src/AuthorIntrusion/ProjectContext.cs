// <copyright file="ProjectContext.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System.Diagnostics.Contracts;

namespace AuthorIntrusion
{
	/// <summary>
	/// Defines a context for an operation. Like EventArgs, this is a class to
	/// pass in parameters and internal state throughout the processing code.
	/// </summary>
	public class ProjectContext
	{
		#region Constructors and Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="ProjectContext"/> class.
		/// </summary>
		/// <param name="project">
		/// The project.
		/// </param>
		public ProjectContext(Project project)
		{
			// Establish our contracts.
			Contract.Requires(project != null);

			// Save the value.
			Project = project;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ProjectContext"/> class via
		/// a copy constructor.
		/// </summary>
		/// <param name="context">
		/// The context.
		/// </param>
		public ProjectContext(ProjectContext context)
		{
			// Establish our contracts.
			Contract.Requires(context != null);

			// Copy the elements.
			Project = context.Project;
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets the project associated with this context.
		/// </summary>
		public Project Project { get; private set; }

		#endregion
	}
}
