// <copyright file="WorkingDirectoryTestsBase.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System.IO;

using AuthorIntrusion.Plugins;

using NUnit.Framework;

namespace AuthorIntrusion.Cli.Tests
{
	/// <summary>
	/// Contains the common functionality uses by most file-system-based unit tests
	/// such as commands and operations.
	/// </summary>
	public abstract class WorkingDirectoryTestsBase
	{
		#region Properties

		/// <summary>
		/// Gets the IoC container for plugins.
		/// </summary>
		protected PluginContainer Container { get; private set; }

		/// <summary>
		/// Gets the directory to the sample files in the project.
		/// </summary>
		/// <value>
		/// The samples directory.
		/// </value>
		protected DirectoryInfo SamplesDirectory { get; private set; }

		/// <summary>
		/// Gets the directory which contains test data.
		/// </summary>
		/// <value>
		/// The input directory.
		/// </value>
		protected DirectoryInfo TestDirectory { get; private set; }

		/// <summary>
		/// Gets the working directory for the unit test which has been already cleared
		/// out and prepared for the unit test.
		/// </summary>
		/// <value>
		/// The working directory.
		/// </value>
		protected DirectoryInfo WorkingDirectory { get; private set; }

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Sets up the environment for a single test.
		/// </summary>
		[SetUp]
		public void Setup()
		{
			// Figure out where all the directories are.
			string className = GetType()
				.FullName;

			string workingPath = Path.Combine(
				"Tests",
				"Working",
				className);
			WorkingDirectory = new DirectoryInfo(workingPath);

			string testPath = Path.Combine(
				"Tests",
				className);
			TestDirectory = new DirectoryInfo(testPath);

			// Get the samples directory.
			SamplesDirectory = new DirectoryInfo("..\\samples");

			// Clear out the working directory.
			if (WorkingDirectory.Exists)
			{
				WorkingDirectory.Delete(true);
			}

			WorkingDirectory.Create();

			// Set up our plugin container for the CLI.
			Container = new PluginContainer(new CliRegistry());
			Container.AssertConfigurationIsValid();
		}

		#endregion
	}
}
