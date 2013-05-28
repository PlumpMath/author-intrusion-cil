// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

namespace AuthorIntrusion.Common
{
	/// <summary>
	/// A project encapsulates all of the text, settings, and organization of a
	/// written document (novel, short story).
	/// </summary>
	public class Project
	{
		#region Properties

		/// <summary>
		/// Gets the block type manager associated with this project.
		/// </summary>
		protected BlockTypeManager BlockTypeManager { get; private set; }

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Project"/> class.
		/// </summary>
		public Project()
		{
			BlockTypeManager = new BlockTypeManager(this);
		}

		#endregion
	}
}
