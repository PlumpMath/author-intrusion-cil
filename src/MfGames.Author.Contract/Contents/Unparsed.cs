#region Namespaces

using System;

using MfGames.Author.Contract.Enumerations;

#endregion

namespace MfGames.Author.Contract.Contents
{
	/// <summary>
	/// Represents a block of text that has not been parsed into other content
	/// elements.
	/// </summary>
	public class Unparsed : Content
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Unparsed"/> class.
		/// </summary>
		/// <param name="text">The text.</param>
		public Unparsed(string text)
		{
			if (text == null)
			{
				throw new ArgumentNullException("text");
			}

			this.text = text;
		}

		#endregion

		#region Contents

		private readonly string text;

		/// <summary>
		/// Contains a flattened representation of the content.
		/// </summary>
		/// <value>The content string.</value>
		public override string ContentString
		{
			get { return text; }
		}

		/// <summary>
		/// Gets the type of content this object represents.
		/// </summary>
		/// <value>The type of the content.</value>
		public override ContentType ContentType
		{
			get { return ContentType.Unparsed; }
		}

		/// <summary>
		/// Gets the contents of the unparsed string.
		/// </summary>
		/// <value>The contents.</value>
		public string Text
		{
			get { return text; }
		}

		#endregion
	}
}