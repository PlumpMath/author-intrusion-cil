#region Namespaces

using MfGames.Author.Contract.Enumerations;

#endregion

namespace MfGames.Author.Contract.Contents
{
	/// <summary>
	/// Represents a puncuation symbol in the string. This can be terminating
	/// or non-terminating depending on the type of puncuation.
	/// </summary>
	public class Puncuation : Content
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Puncuation"/> class.
		/// </summary>
		/// <param name="text">The text.</param>
		public Puncuation(string text)
		{
			this.text = text;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Puncuation"/> class.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="isTerminating">if set to <c>true</c> [is terminating].</param>
		public Puncuation(
			string text,
			bool isTerminating)
		{
			this.text = text;
			this.isTerminating = isTerminating;
		}

		#endregion

		#region Contents

		private bool isTerminating;
		private string text;

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
			get { return isTerminating ? ContentType.Terminator : ContentType.Punctuation; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this puncuation is
		/// sentence-terminating.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is terminating; otherwise, <c>false</c>.
		/// </value>
		public bool IsTerminating
		{
			get { return isTerminating; }
			set { isTerminating = value; }
		}

		#endregion
	}
}