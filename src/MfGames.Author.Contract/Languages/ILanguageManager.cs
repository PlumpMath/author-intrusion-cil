#region Namespaces

using MfGames.Author.Contract.Structures;

#endregion

namespace MfGames.Author.Contract.Languages
{
	/// <summary>
	/// Defines the signature for anything that processes languages.
	/// </summary>
	public interface ILanguageManager
	{
		#region Parsing

		/// <summary>
		/// Parses the contents of the given structure.
		/// </summary>
		/// <param name="structure">The structure.</param>
		void Parse(Structure structure);

		#endregion
	}
}