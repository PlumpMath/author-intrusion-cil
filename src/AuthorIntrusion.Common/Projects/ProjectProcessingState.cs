// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

namespace AuthorIntrusion.Common.Projects
{
	/// <summary>
	/// Enumerations the various states that a project can be in. This is used
	/// to control batch operations.
	/// </summary>
	public enum ProjectProcessingState
	{
		/// <summary>
		/// Indicates that no special processing is going on with the project.
		/// </summary>
		Interactive,

		/// <summary>
		/// Indicates that the project is in the middle of a batch operation.
		/// </summary>
		Batch,
	}
}
