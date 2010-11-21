namespace MfGames.Author.Contract.Contents
{
	/// <summary>
	/// Contains a single sematic word for parsing.
	/// </summary>
	public class Word : ContentBase
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

		#region Properties

		private readonly string text;

		/// <summary>
		/// Gets the text.
		/// </summary>
		/// <value>The text.</value>
		public string Text
		{
			get { return text; }
		}

		#endregion
	}
}