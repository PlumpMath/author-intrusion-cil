#region Namespaces

using System;

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

			// Just set up the input.
			Console.WriteLine("Hello World!");
			Console.WriteLine("Press ENTER to exit");
			Console.ReadLine();
		}
	}
}