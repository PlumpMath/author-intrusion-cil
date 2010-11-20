#region Namespaces

using System;
using System.IO;

using Castle.Core;

using MfGames.Author.Contract.Interfaces;

#endregion


namespace MfGames.Author.IO
{
	/// <summary>
	/// A singleton class that manages the input and reading of documents and
	/// converting them into the internal structure.
	/// </summary>
	[Singleton]
	public class InputManager : IInputManager
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="InputManager"/> class.
		/// </summary>
		/// <param name="inputReaders">The input readers.</param>
		public InputManager(IInputReader[] inputReaders)
		{
			this.inputReaders = inputReaders;
		}

		#endregion

		#region InputReader Management

		private readonly IInputReader[] inputReaders;

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