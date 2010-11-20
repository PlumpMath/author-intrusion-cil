#region Namespaces

using System.Diagnostics;

using MfGames.Author.Contract.Structures.Collections;
using MfGames.Author.Contract.Structures.Interfaces;

#endregion

namespace MfGames.Author.Contract.Structures
{
	/// <summary>
	/// Defines the common functionality where an object can contain both
	/// inner sections and paragraphs.
	/// </summary>
	public abstract class SectionParagraphContainerBase
		: StructureBase, ISectionContainer, IParagraphContainer
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Chapter"/> class.
		/// </summary>
		protected SectionParagraphContainerBase()
		{
			sections = new SectionList(this);
			paragraphs = new ParagraphList(this);
		}

		#endregion

		#region Structural Relationships

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly ParagraphList paragraphs;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly SectionList sections;

		/// <summary>
		/// Gets the paragraphs inside the container.
		/// </summary>
		/// <value>The sections.</value>
		public ParagraphList Paragraphs
		{
			get { return paragraphs; }
		}

		/// <summary>
		/// Gets the sections inside the container.
		/// </summary>
		/// <value>The sections.</value>
		public SectionList Sections
		{
			get { return sections; }
		}

		#endregion
	}
}