// <copyright file="CliOptions.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using AuthorIntrusion.Cli.Transform;

using CommandLine;

namespace AuthorIntrusion.Cli
{
	/// <summary>
	/// Encapsulates the argument options for the CLI tool.
	/// </summary>
	public class CliOptions
	{
		#region Public Properties

		/// <summary>
		/// Gets or sets the options for the sub-command form transforming
		/// files from one format to another.
		/// </summary>
		[VerbOption(TransformOptions.LongName,
			HelpText =
				"Transform an input file or project into a different format.")]
		public TransformOptions TransformOptions { get; set; }

		#endregion
	}
}
