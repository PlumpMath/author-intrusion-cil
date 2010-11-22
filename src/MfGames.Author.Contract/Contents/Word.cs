#region Namespaces

using MfGames.Author.Contract.Enumerations;

#endregion

namespace MfGames.Author.Contract.Contents
{
	/// <summary>
	/// Contains a single sematic word for parsing.
	/// </summary>
	public class Word : Content
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Word"/> class.
		/// </summary>
		/// <param name="text">The text.</param>
		public Word(string text)
		{
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
			get { return ContentType.Word; }
		}

		/// <summary>
		/// Gets the text of this word.
		/// </summary>
		/// <value>The text.</value>
		public string Text
		{
			get { return text; }
		}

		#endregion
	}
}