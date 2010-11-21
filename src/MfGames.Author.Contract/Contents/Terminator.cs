using System;

namespace MfGames.Author.Contract.Contents
{
	/// <summary>
	/// Indicates a sentence terminator.
	/// </summary>
	public class Terminator : ContentBase
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Terminator"/> class.
		/// </summary>
		/// <param name="text">The text.</param>
		public Terminator(string text)
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

		#region Conversion

		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </returns>
		public override string ToString()
		{
			return String.Format("Terminator {0}", text);
		}

		#endregion
	}
}