#region Namespaces

using System.Collections.Generic;

using MfGames.Author.Contract.Contents;
using MfGames.Author.Contract.Contents.Collections;
using MfGames.Author.Contract.Contents.Interfaces;

#endregion

namespace MfGames.Author.Contract.Structures
{
	/// <summary>
	/// Represents a single paragraph within the document.
	/// </summary>
	public class Paragraph : StructureBase, IUnparsedContentContainer
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Paragraph"/> class.
		/// </summary>
		public Paragraph()
		{
			unparsedContents = new ContentList();
			sentences = new List<Sentence>();
		}

		#endregion

		#region Contents

		private readonly List<Sentence> sentences;
		private readonly ContentList unparsedContents;

		/// <summary>
		/// Contains the sentences within the paragraph.
		/// </summary>
		/// <value>The sentences.</value>
		public List<Sentence> Sentences
		{
			get { return sentences; }
		}

		/// <summary>
		/// Gets a value indicating whether this instance is parsed.
		/// </summary>
		/// <value><c>true</c> if this instance is parsed; otherwise, <c>false</c>.</value>
		public bool IsParsed
		{
			get { return unparsedContents.Count == 0; }
		}

		/// <summary>
		/// Gets the unparsed contents which is a list of partially formatted
		/// content elements along with UnparsedString.
		/// </summary>
		/// <value>The unparsed contents.</value>
		public ContentList UnparsedContents
		{
			get { return unparsedContents; }
		}

		#endregion
	}
}