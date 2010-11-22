#region Namespaces

using MfGames.Author.Contract.Collections;

#endregion

namespace MfGames.Author.Contract.Contents
{
	/// <summary>
	/// Represents a single sentence in the structure.
	/// </summary>
	public class Sentence : ContentContainerContent
	{
		public override ContentType ContentType
		{ get { return ContentType.Sentence; }}
	}
}