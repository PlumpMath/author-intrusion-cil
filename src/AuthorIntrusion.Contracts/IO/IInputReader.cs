#region Namespaces

using System.IO;

using AuthorIntrusion.Contracts.Structures;

#endregion

namespace AuthorIntrusion.Contracts.IO
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
		/// Reads the specified input stream and returns a document representing
		/// the document.
		/// </summary>
		/// <param name="inputStream">The input stream.</param>
		/// <returns>A document representing the stream.</returns>
		Document Read(Stream inputStream);

		#endregion
	}
}