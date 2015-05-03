// <copyright file="TransformCommandTests.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System.Collections.Generic;
using System.IO;
using System.Xml;

using AuthorIntrusion.Cli.Transform;

using Xunit;

namespace AuthorIntrusion.Cli.Tests
{
	/// <summary>
	/// Contains the simple commands to exercise the `transform` command from the
	/// CLI.
	/// </summary>
	public class TransformCommandTests : WorkingDirectoryTestsBase
	{
		#region Public Methods and Operators

		/// <summary>
		/// Tests loading a simple markdown file into memory and then write it out.
		/// </summary>
		[Fact]
		public void SimpleMarkdownToDocBookArticle()
		{
			// Create the options and populate the values.
			string outputFilename = Path.Combine(
				WorkingDirectory.FullName,
				"output.xml");
			var options = new TransformOptions
			{
				Input = Path.Combine(
					SamplesDirectory.FullName,
					"Frankenstein Markdown",
					"chapter-01.markdown"),
				Output = outputFilename
			};

			// Create the transform command and run the job.
			var command = Container.GetInstance<TransformCommand>();

			command.Run(options);

			// Load the XML back in to verify it.
			var xml = new XmlDocument();

			xml.Load(outputFilename);

			// Assert the output.
			Assert.Equal(
				"article",
				xml.LastChild.LocalName);
		}

		/// <summary>
		/// Tests loading a simple markdown file into memory and then write it out.
		/// </summary>
		[Fact]
		public void SimpleMarkdownToDocBookChapter()
		{
			// Create the options and populate the values.
			string outputFilename = Path.Combine(
				WorkingDirectory.FullName,
				"output.xml");
			var options = new TransformOptions
			{
				Input = Path.Combine(
					SamplesDirectory.FullName,
					"Frankenstein Markdown",
					"chapter-01.markdown"),
				Output = outputFilename,
				OutputOptions = new List<string>
				{
					"RootElement=chapter"
				}
			};

			// Create the transform command and run the job.
			var command = Container.GetInstance<TransformCommand>();

			command.Run(options);

			// Load the XML back in to verify it.
			var xml = new XmlDocument();

			xml.Load(outputFilename);

			// Assert the output.
			Assert.Equal(
				"chapter",
				xml.LastChild.LocalName);
		}

		/// <summary>
		/// Tests loading a simple markdown file into memory and then write it out.
		/// </summary>
		[Fact]
		public void SimpleMarkdownToMarkdown()
		{
			// Create the options and populate the values.
			string outputFilename = Path.Combine(
				WorkingDirectory.FullName,
				"output.markdown");
			var options = new TransformOptions
			{
				Input = Path.Combine(
					SamplesDirectory.FullName,
					"Frankenstein Markdown",
					"chapter-01.markdown"),
				Output = outputFilename
			};

			// Create the transform command and run the job.
			var command = Container.GetInstance<TransformCommand>();

			command.Run(options);

			// Load the text back into to verify it.
			string[] lines = File.ReadAllLines(outputFilename);

			// Assert the output.
			Assert.Equal(
				17,
				lines.Length);
		}

		#endregion
	}
}
