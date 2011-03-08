#region Namespaces

using System.IO;

using AuthorIntrusion.Contracts.Structures;

#endregion

namespace AuthorIntrusion.Contracts.IO
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
		Document Read(FileInfo inputFile);

		#endregion
	}
}