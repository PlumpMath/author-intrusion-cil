using System;

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

		#region Properties


		public override ContentType ContentType {
			get {
				return ContentType.Word;
			}
		}

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

		#region Conversion

		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </returns>
		public override string ToString()
		{
			return text;
		}

		#endregion
	}
}