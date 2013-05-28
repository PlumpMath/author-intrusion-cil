// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

namespace AuthorIntrusion.Cli
{
	/// <summary>
	/// Implements a command-line interface for various functionality inside the
	/// AuthorIntrusion system.
	/// </summary>
	public class Program
	{
		#region Methods

		/// <summary>
		/// Main entry point into the application.
		/// </summary>
		/// <param name="args">The arguments.</param>
		private static void Main(string[] args)
		{
			// Create the IoC/DI resolver.
			var resolver = new Resolver();
		}

		#endregion
	}
}
