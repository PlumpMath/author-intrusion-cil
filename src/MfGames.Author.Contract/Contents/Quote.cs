#region Namespaces

using MfGames.Author.Contract.Contents.Collections;
using MfGames.Author.Contract.Contents.Interfaces;
using MfGames.Author.Contract.Languages;

#endregion

namespace MfGames.Author.Contract.Contents
{
	/// <summary>
	/// Defines a quote within the content.
	/// </summary>
	public class Quote : ContentBase, IUnparsedContentContainer
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Quote"/> class.
		/// </summary>
		private Quote()
		{
			contents = new ContentList();
			unparsedContents = new ContentList();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Quote"/> class.
		/// </summary>
		/// <param name="unparsedString">The unparsed string.</param>
		public Quote(string unparsedString)
			: this()
		{
			unparsedContents.Add(unparsedString);
		}

		#endregion

		#region Contents

		private readonly ContentList unparsedContents;
		private readonly ContentList contents;

		/// <summary>
		/// Gets the parsed contents of the quote.
		/// </summary>
		/// <value>The contents.</value>
		public ContentList Contents
		{
			get { return contents; }
		}

		/// <summary>
		/// Gets a value indicating whether this instance is parsed.
		/// </summary>
		/// <value><c>true</c> if this instance is parsed; otherwise, <c>false</c>.</value>
		public bool IsParsed
		{
			get { return unparsedContents.Count != 0; }
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

		/// <summary>
		/// Splits the unparsed content of the quote and put it into the parsed
		/// contents.
		/// </summary>
		/// <param name="splitter">The splitter.</param>
		public void SplitContents(IContentSplitter splitter)
		{
			// If we don't have unparsed contents, don't do anything.
			if (unparsedContents.Count == 0)
			{
				return;
			}

			// Parse the contents and put it into the parsed list.
			contents.Clear();
			contents.AddRange(splitter.SplitContents(unparsedContents));

			// Clear out the unparsed contents since everything is parsed.
			unparsedContents.Clear();
		}

		#endregion
	}
}