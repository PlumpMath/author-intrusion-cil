#region Namespaces

using MfGames.Author.Contract.Enumerations;

#endregion

namespace MfGames.Author.Contract.Contents
{
	/// <summary>
	/// Base class for all content elements.
	/// </summary>
	public abstract class Content
	{
		#region Contents

		/// <summary>
		/// Contains a flattened representation of the content.
		/// </summary>
		/// <value>The content string.</value>
		public abstract string ContentString { get; }

		/// <summary>
		/// Gets the type of content this object represents.
		/// </summary>
		/// <value>The type of the content.</value>
		public abstract ContentType ContentType { get; }

		/// <summary>
		/// Gets a value indicating whether this content is normally formatted
		/// with a leading space.
		/// </summary>
		/// <value><c>true</c> if [needs leading space]; otherwise, <c>false</c>.</value>
		public virtual bool NeedsLeadingSpace
		{
			get { return true; }
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
			// Show the content type plus a small section of the content itself.
			string contentString = ContentString;

			if (contentString.Length > 25)
			{
				contentString = contentString.Substring(0, 22) + "...";
			}

			return string.Format("{0} {1}", ContentType, contentString);
		}

		#endregion
	}
}