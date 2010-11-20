#region Namespaces

using System.IO;

using MfGames.Author.Contract.Structures.Interfaces;

#endregion

namespace MfGames.Author.Contract.IO
{
	/// <summary>
	/// Identifies the classes used for identifying and reading input streams
	/// and generating the internal structure tree.
	/// </summary>
	public interface IInputReader
	{
		#region Identification

		/// <summary>
		/// Gets the file extensions that are commonly associated with this
		/// input reader.
		/// </summary>
		/// <value>The file mask.</value>
		string[] FileExtensions { get; }

		/// <summary>
		/// Gets the name of the input file.
		/// </summary>
		/// <value>The name.</value>
		string Name { get; }

		/// <summary>
		/// Determines whether this reader can read the specified input stream.
		/// </summary>
		/// <param name="inputStream">The input stream.</param>
		/// <returns>
		/// 	<c>true</c> if this instance can read the specified input stream; otherwise, <c>false</c>.
		/// </returns>
		bool CanRead(Stream inputStream);

		#endregion

		#region Reading

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