#region Namespaces

using AuthorIntrusion.Contracts.Enumerations;

#endregion

namespace AuthorIntrusion.Contracts.Contents
{
	/// <summary>
	/// Represents quoted text within the sentence.
	/// </summary>
	public class Quote : ContentContainerContent
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Quote"/> class.
		/// </summary>
		public Quote()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Quote"/> class.
		/// </summary>
		/// <param name="unparsed">The unparsed.</param>
		public Quote(string unparsed)
			: this()
		{
			Contents.Add(new Unparsed(unparsed));
		}

		#endregion

		#region Contents

		/// <summary>
		/// Gets the type of content this object represents.
		/// </summary>
		/// <value>The type of the content.</value>
		public override ContentType ContentType
		{
			get { return ContentType.Quote; }
		}

		#endregion
	}
}