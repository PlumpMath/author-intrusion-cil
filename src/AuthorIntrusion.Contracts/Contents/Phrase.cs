#region Namespaces

using AuthorIntrusion.Contracts.Enumerations;

#endregion

namespace AuthorIntrusion.Contracts.Contents
{
	/// <summary>
	/// Defines a generic phrase within the contents.
	/// </summary>
	public class Phrase : ContentContainerContent
	{
		#region Contents

		/// <summary>
		/// Gets the type of content this object represents.
		/// </summary>
		/// <value>The type of the content.</value>
		public override ContentType ContentType
		{
			get { return ContentType.Phrase; }
		}

		#endregion
	}
}