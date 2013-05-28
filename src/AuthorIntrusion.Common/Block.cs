// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

namespace AuthorIntrusion.Common
{
	/// <summary>
	/// A block is the primary structural element inside a project. It
	/// represents various paragraphs (normal, epigraphs) as well as some
	/// organizational units (chapters, scenes).
	/// </summary>
	public class Block
	{
		#region Properties

		/// <summary>
		/// Gets the project associated with this block.
		/// </summary>
		protected Project Project { get; private set; }

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Block"/> class.
		/// </summary>
		/// <param name="project">The project.</param>
		public Block(Project project)
		{
			Project = project;
		}

		#endregion
	}
}
