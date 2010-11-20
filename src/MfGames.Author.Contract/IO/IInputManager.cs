#region Namespaces

using System.IO;

using MfGames.Author.Contract.Structures.Interfaces;

#endregion

namespace MfGames.Author.Contract.IO
{
	/// <summary>
	/// Defines the signature for the input manager.
	/// </summary>
	public interface IInputManager
	{
		#region Reading

		/// <summary>
		/// Reads the specified file and returns a structure elements.
		/// If there is any problems with reading the input, this should throw
		/// an exception and never return a null root structure.
		/// </summary>
		/// <param name="inputFile">The input file.</param>
		/// <returns></returns>
		IRootStructure Read(FileInfo inputFile);

		/// <summary>
		/// Reads the specified input stream and returns a structure elements.
		/// If there is any problems with reading the input, this should throw
		/// an exception and never return a null root structure.
		/// </summary>
		/// <param name="inputStream">The input stream.</param>
		/// <returns></returns>
		IRootStructure Read(Stream inputStream);

		#endregion
	}
}