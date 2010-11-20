#region Namespaces

using System;
using System.IO;
using System.Xml;

#endregion

namespace MfGames.Author.IO
{
	/// <summary>
	/// Handles the common code for reading XML files.
	/// </summary>
	public abstract class XmlInputReaderBase
	{
		#region Identification

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
				reader.MoveToElement();
				throw new Exception("Cannot read this: " + reader.LocalName);
			}
		}

		#endregion
	}
}