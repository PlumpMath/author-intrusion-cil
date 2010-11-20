#region Namespaces

using System;
using System.IO;

using MfGames.Author.Contract.Interfaces;

#endregion

namespace MfGames.Author.IO
{
	/// <summary>
	/// Defines an input reader that takes Docbook 5 XML and produces the
	/// internal structure. Docbook elements that are not understood are ignored
	/// and dropped.
	/// </summary>
	public class Docbook5InputReader : XmlInputReaderBase, IInputReader
	{
		#region Identification

		/// <summary>
		/// Gets the file masks that are commonly associated with this input
		/// file format.
		/// </summary>
		/// <value>The file mask.</value>
		public string[] FileExtensions
		{
			get
			{
				return new[] { ".xml" };
			}
		}

		/// <summary>
		/// Gets the name of the input file.
		/// </summary>
		/// <value>The name.</value>
		public string Name { get { return "Docbook 5"; } }

		#endregion

		#region Reading

		/// <summary>
		/// Reads the specified input stream and returns a structure elements.
		/// If there is any problems with reading the input, this should throw
		/// an exception and never return a null root structure.
		/// </summary>
		/// <param name="inputStream">The input stream.</param>
		/// <returns></returns>
		public IRootStructure Read(Stream inputStream)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}