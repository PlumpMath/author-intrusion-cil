#region Namespaces

using System;
using System.IO;

using AuthorIntrusion;
using AuthorIntrusion.Contracts.IO;
using AuthorIntrusion.Contracts.Languages;
using AuthorIntrusion.Contracts.Structures;

using MfGames.Logging;

#endregion

namespace AuthorIntrusionCli
{
	internal class CliEntry
	{
		public static void Main(string[] args)
		{
			// Initialize the author system.
			var manager = new Manager();

			// Set up logging for the console.
			ILogger logger = new ConsoleLogger("{1,5} {2}");
			manager.Register<ILogger>(logger);
			Log log = new Log(typeof(CliEntry), logger);

			// Read the input file.
			var inputFile = new FileInfo(args[0]);
			log.Info("Reading {0} {1}", inputFile, inputFile.Exists);

			IInputManager inputManager = manager.InputManager;
			Structure rootStructure = inputManager.Read(inputFile);

			// Parse the contents of the root.
			ILanguageManager languageManager = manager.LanguageManager;
			languageManager.Parse(rootStructure);

			// Write out the HTML
			var outputFile = new FileInfo(args[1]);
			log.Info("Writing {0} {1}", outputFile, outputFile.Exists);

			IOutputManager outputManager = manager.OutputManager;
			outputManager.Write(outputFile, rootStructure);

			// Just set up the input.
			Console.WriteLine("Hello World!");
			Console.WriteLine("Press ENTER to exit");
			Console.ReadLine();
		}
	}
}