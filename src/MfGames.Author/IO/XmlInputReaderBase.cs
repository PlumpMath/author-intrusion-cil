#region Namespaces

using System.IO;
using System.Xml;

using MfGames.Author.Contract.IO;
using MfGames.Author.Contract.Structures;

#endregion

namespace MfGames.Author.IO
{
	/// <summary>
	/// Handles the common code for reading XML files.
	/// </summary>
	public abstract class XmlInputReaderBase : IInputReader
	{
		#region Identification

		/// <summary>
		/// Gets the file extensions that are commonly associated with this
		/// input reader.
		/// </summary>
		/// <value>The file mask.</value>
		public abstract string[] FileExtensions
		{
			get;
		}

		/// <summary>
		/// Gets the name of the input file.
		/// </summary>
		/// <value>The name.</value>
		public abstract string Name
		{
			get;
		}

		/// <summary>
		/// Determines whether this reader can read the specified input stream.
		/// </summary>
		/// <param name="inputStream">The input stream.</param>
		/// <returns>
		/// 	<c>true</c> if this instance can read the specified input stream; otherwise, <c>false</c>.
		/// </returns>
		public virtual bool CanRead(Stream inputStream)
		{
			// Wrap the stream in an XmlReader so we can parse the top-level
			// element.
			using (XmlReader reader = XmlReader.Create(inputStream))
			{
				// Read the first element.
				while (reader.Read())
				{
					// If we haven't gotten to the first element, keep on reading.
					if (reader.NodeType != XmlNodeType.Element)
					{
						continue;
					}

					// We have the first element, so ask the extending classes
					// if this is a valid document.
					return CanReadElement(reader);
				}
			}

			// We couldn't find an element, so we can't read it.
			return false;
		}

		/// <summary>
		/// Determines whether this instance can read XML files with the given root
		/// element.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <returns>
		/// 	<c>true</c> if this instance [can read element] the specified reader; otherwise, <c>false</c>.
		/// </returns>
		protected abstract bool CanReadElement(XmlReader reader);

		#endregion

		#region Reading

		#region Implementation of IInputReader

		/// <summary>
		/// Reads the specified input stream and returns a structure elements.
		/// If there is any problems with reading the input, this should throw
		/// an exception and never return a null root structure.
		/// </summary>
		/// <param name="inputStream">The input stream.</param>
		/// <returns>An StructureBase containing the top-level element.</returns>
		public Structure Read(Stream inputStream)
		{
			// Wrap the stream in an XmlReader so we can parse the top-level
			// element.
			using (XmlReader reader = XmlReader.Create(inputStream))
			{
				return Read(reader);
			}
		}

		/// <summary>
		/// Reads the specified XML stream and returns a structure elements.
		/// If there is any problems with reading the input, this should throw
		/// an exception and never return a null root structure.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <returns>
		/// An StructureBase containing the top-level element.
		/// </returns>
		protected abstract Structure Read(XmlReader reader);

		#endregion

		#endregion

	}
}