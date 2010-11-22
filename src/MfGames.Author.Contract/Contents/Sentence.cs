#region Namespaces



#endregion

using MfGames.Author.Contract.Enumerations;

namespace MfGames.Author.Contract.Contents
{
	/// <summary>
	/// Represents a single sentence in the structure.
	/// </summary>
	public class Sentence : ContentContainerContent
	{
		#region Contents

		/// <summary>
		/// Gets the type of content this object represents.
		/// </summary>
		/// <value>The type of the content.</value>
		public override ContentType ContentType
		{
			get { return ContentType.Sentence; }
		}

		#endregion
	}
}