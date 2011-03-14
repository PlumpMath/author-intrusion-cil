#region Namespaces

using System;
using System.IO;

using AuthorIntrusion;
using AuthorIntrusion.Contracts;
using AuthorIntrusion.Contracts.Events;
using AuthorIntrusion.Contracts.IO;
using AuthorIntrusion.Contracts.Languages;

using MfGames.Logging;

using StructureMap;

#endregion

namespace AuthorIntrusionCli
{
	internal class CliEntry
	{
		public static void Main(string[] args)
		{
			// Set up the manager.
			Manager.Setup();

			// Set up logging for the console.
			ILogger logger = new ConsoleLogger("{1,5} {2}");
			Manager.Register(logger);
			Log log = new Log(typeof(CliEntry), logger);

			// Read the input file.
			var inputFile = new FileInfo(args[0]);
			log.Info("Reading {0} {1}", inputFile, inputFile.Exists);

			var inputManager = ObjectFactory.GetInstance<IInputManager>();
			Document document = inputManager.Read(inputFile);

			// Parse the contents of the root.
			log.Info("Paragraphs {0:N0}", document.Structure.ParagraphCount);

			DateTime lastReport = DateTime.UtcNow;
			var languageManager = ObjectFactory.GetInstance<ILanguageManager>();

			languageManager.ParseProgress += delegate(object sender, ParseProgressEventArgs progressArgs)
				{
					if ((DateTime.UtcNow - lastReport).TotalMilliseconds > 1000)
					{
						lastReport = DateTime.UtcNow;
						log.Info(
					         "Parsing paragraphs {0:N0}/{1:N0} {2:N2}%",
					         progressArgs.ParagraphsProcessed,
					         progressArgs.ParagraphCount,
					         100.0 * progressArgs.ParagraphsProcessed / progressArgs.ParagraphCount);
					}
				};
			languageManager.Parse(document.Structure);

			// Write out the HTML
			var outputFile = new FileInfo(args[1]);
			log.Info("Writing {0} {1}", outputFile, outputFile.Exists);

			var outputManager = ObjectFactory.GetInstance<IOutputManager>();
			outputManager.Write(outputFile, document);

			// Just set up the input.
			log.Info("Press ENTER to exit");
			Console.ReadLine();
		}
	}
}