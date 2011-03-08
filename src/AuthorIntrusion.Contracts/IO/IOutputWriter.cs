#region Namespaces

using System.IO;

using AuthorIntrusion.Contracts.Structures;

#endregion

namespace AuthorIntrusion.Contracts.IO
{
	/// <summary>
	/// Defines the signature for something that can translate the internal
	/// structure into a specific format.
	/// </summary>
	public interface IOutputWriter
	{
		#region Identification

		/// <summary>
		/// Gets the file extensions that are commonly associated with this
		/// writer.
		/// </summary>
		/// <value>The file mask.</value>
		string[] FileExtensions { get; }

		/// <summary>
		/// Gets the name of the output format.
		/// </summary>
		/// <value>The name.</value>
		string Name { get; }

		#endregion

		#region Writing

		/// <summary>
		/// Writes out the root structure to the given output stream.
		/// </summary>
		/// <param name="outputStream">The output stream.</param>
		/// <param name="document">The document to write out.</param>
		void Write(
			Stream outputStream,
			Document document);

		#endregion
	}
}