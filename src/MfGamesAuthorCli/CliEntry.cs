#region Namespaces

using System;
using System.IO;

using MfGames.Author;
using MfGames.Author.Contract.Interfaces;

#endregion

namespace MfGamesAuthorCli
{
	internal class CliEntry
	{
		public static void Main(string[] args)
		{
			// Initialize the author system.
			Manager manager = new Manager();
			IInputManager inputManager = manager.InputManager;

			// Read the input file.
			FileInfo inputFile = new FileInfo(args[0]);
			Console.WriteLine("Reading {0} {1}", inputFile, inputFile.Exists);
			IRootStructure rootStructure = inputManager.Read(inputFile);

			// Just set up the input.
			Console.WriteLine("Hello World!");
			Console.WriteLine("Press ENTER to exit");
			Console.ReadLine();
		}
	}
}