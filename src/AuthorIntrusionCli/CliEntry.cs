#region Copyright and License

// Copyright (c) 2005-2011, Moonfire Games
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

#endregion

#region Namespaces

using System;
using System.IO;

using AuthorIntrusion;
using AuthorIntrusion.Contracts;
using AuthorIntrusion.Contracts.Events;
using AuthorIntrusion.Contracts.IO;
using AuthorIntrusion.Contracts.Languages;

using MfGames.Reporting;

using StructureMap;

#endregion

namespace AuthorIntrusionCli
{
	internal class CliEntry
	{
		public static void Main(string[] args)
		{
			// Set up the manager.
			Container container = Manager.Setup();

			// Set up logging for the console.
			var log = new Logger(typeof(CliEntry));

			// Read the input file.
			var inputFile = new FileInfo(args[0]);
			log.Info("Reading {0} {1}", inputFile, inputFile.Exists);

			var inputManager = container.GetInstance<IInputManager>();
			Document document = inputManager.Read(inputFile);

			//// Parse the contents of the root.
			//log.Info("Paragraphs {0:N0}", document.Structure.ParagraphCount);

			//DateTime lastReport = DateTime.UtcNow;
			//var languageManager = container.GetInstance<ILanguageManager>();

			//languageManager.ParseProgress += delegate(object sender,
			//                                          ParseProgressEventArgs progressArgs)
			//                                 {
			//                                    if (
			//                                        (DateTime.UtcNow - lastReport).
			//                                            TotalMilliseconds > 1000)
			//                                    {
			//                                        lastReport = DateTime.UtcNow;
			//                                        log.Info(
			//                                            "Parsing paragraphs {0:N0}/{1:N0} {2:N2}%",
			//                                            progressArgs.ParagraphsProcessed,
			//                                            progressArgs.ParagraphCount,
			//                                            100.0 * progressArgs.ParagraphsProcessed /
			//                                            progressArgs.ParagraphCount);
			//                                    }
			//                                 };
			//languageManager.Parse(document.Structure);

			//// Write out the HTML
			//var outputFile = new FileInfo(args[1]);
			//log.Info("Writing {0} {1}", outputFile, outputFile.Exists);

			//var outputManager = container.GetInstance<IOutputManager>();
			//outputManager.Write(outputFile, document);

			// TODO Fix

			// Just set up the input.
			log.Info("Press ENTER to exit");
			Console.ReadLine();
		}
	}
}