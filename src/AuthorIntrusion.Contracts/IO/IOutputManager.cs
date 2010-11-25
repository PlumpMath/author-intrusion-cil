#region Namespaces

using System.IO;

using MfGames.Author.Contract.Structures;

#endregion

namespace MfGames.Author.Contract.IO
{
	/// <summary>
	/// Interface that describes the manager that deals with writing out files
	/// to a specific format.
	/// </summary>
	public interface IOutputManager
	{
		#region Writing

		/// <summary>
		/// Writes out the root structure to the given file. This will look up
		/// the file format from the loaded output writers.
		/// </summary>
		/// <param name="inputFile">The input file.</param>
		/// <param name="rootStructure">The root structure.</param>
		void Write(
			FileInfo inputFile,
			Structure rootStructure);

		#endregion
	}
}