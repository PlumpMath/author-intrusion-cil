#region Namespaces

using System;
using System.IO;

using MfGames.Author;
using MfGames.Author.Contract.IO;
using MfGames.Author.Contract.Languages;
using MfGames.Author.Contract.Structures;

#endregion

namespace MfGamesAuthorCli
{
	internal class CliEntry
	{
		public static void Main(string[] args)
		{
			// Initialize the author system.
			Manager manager = new Manager();

			// Read the input file.
			FileInfo inputFile = new FileInfo(args[0]);
			Console.WriteLine("Reading {0} {1}", inputFile, inputFile.Exists);

			IInputManager inputManager = manager.InputManager;
			Structure rootStructure = inputManager.Read(inputFile);

			// Parse the contents of the root.
			ILanguageManager languageManager = manager.LanguageManager;
			languageManager.Parse(rootStructure);

			// Write out the HTML
			FileInfo outputFile = new FileInfo(args[1]);
			Console.WriteLine("Writing {0} {1}", outputFile, outputFile.Exists);

			IOutputManager outputManager = manager.OutputManager;
			outputManager.Write(outputFile, rootStructure);

			// Just set up the input.
			Console.WriteLine("Hello World!");
			Console.WriteLine("Press ENTER to exit");
			Console.ReadLine();
		}
	}
}