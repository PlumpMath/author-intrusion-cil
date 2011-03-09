#region Namespaces

using System.IO;

using AuthorIntrusion.Contracts.Structures;

#endregion

namespace AuthorIntrusion.Contracts.IO
{
	/// <summary>
	/// Interface that describes the manager that deals with writing out files
	/// to a specific format.
	/// </summary>
	public interface IOutputManager
	{
		#region Writers

		/// <summary>
		/// Gets the writers associated with the manager.
		/// </summary>
		/// <value>The writers.</value>
		IOutputWriter[] Writers { get; }

		#endregion

		#region Writing

		/// <summary>
		/// Writes out the root structure to the given file. This will look up
		/// the file format from the loaded output writers.
		/// </summary>
		/// <param name="inputFile">The input file.</param>
		/// <param name="document">The document to write out.</param>
		void Write(
			FileInfo inputFile,
			Document document);

		#endregion
	}
}