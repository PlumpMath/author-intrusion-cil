// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System;

namespace AuthorIntrusion.Common.Events
{
	/// <summary>
	/// An event argument associated with a project.
	/// </summary>
	public class ProjectEventArgs: EventArgs
	{
		#region Properties

		public Project Project { get; private set; }

		#endregion

		#region Constructors

		public ProjectEventArgs(Project project)
		{
			Project = project;
		}

		#endregion
	}
}
