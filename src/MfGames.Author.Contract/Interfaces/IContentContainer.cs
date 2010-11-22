#region Namespaces

using MfGames.Author.Contract.Collections;

#endregion

namespace MfGames.Author.Contract.Interfaces
{
	/// <summary>
	/// Defines an item that can contain unparsed contents.
	/// </summary>
	public interface IContentContainer
	{
		#region Contents

		ContentList Contents { get; }

		#endregion
	}
}