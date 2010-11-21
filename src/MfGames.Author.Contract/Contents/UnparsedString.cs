#region Namespaces

using System;

#endregion

namespace MfGames.Author.Contract.Contents
{
	/// <summary>
	/// Represents a block of text that has not been parsed into other content
	/// elements.
	/// </summary>
	public class UnparsedString : ContentBase
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="UnparsedString"/> class.
		/// </summary>
		/// <param name="contents">The contents.</param>
		public UnparsedString(string contents)
		{
			if (contents == null)
			{
				throw new ArgumentNullException("contents");
			}

			this.contents = contents;
		}

		#endregion

		#region Contents

		private readonly string contents;

		/// <summary>
		/// Gets the contents of the unparsed string.
		/// </summary>
		/// <value>The contents.</value>
		public string Contents
		{
			get { return contents; }
		}

		#endregion
	}
}