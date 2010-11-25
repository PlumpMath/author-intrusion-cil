#region Namespaces

using AuthorIntrusion.Contracts.Structures;

#endregion

namespace AuthorIntrusion.Contracts.Languages
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