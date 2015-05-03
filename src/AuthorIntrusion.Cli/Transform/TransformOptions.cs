// <copyright file="TransformOptions.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System;
using System.Collections.Generic;

using CommandLine;

namespace AuthorIntrusion.Cli.Transform
{
	/// <summary>
	/// Defines the "transform" sub-command options.
	/// </summary>
	public class TransformOptions
	{
		#region Constants

		/// <summary>
		/// Contains the long name that identifies this option
		/// </summary>
		public const string LongName = "transform";

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets the input project, which is the file that will be loaded.
		/// </summary>
		[ValueOption(0)]
		public string Input { get; set; }

		/// <summary>
		/// Gets a normalized URI-based version of Input.
		/// </summary>
		public Uri InputUri { get { return new Uri(Input); } }

		/// <summary>
		/// Gets or sets the output filename, which is a URI or file.
		/// </summary>
		/// <value>
		/// The output.
		/// </value>
		[ValueOption(1)]
		public string Output { get; set; }

		/// <summary>
		/// Gets or sets zero or more additional options to set.
		/// </summary>
		[OptionList('O', "output-option", Separator = ':')]
		public IList<string> OutputOptions { get; set; }

		/// <summary>
		/// Gets the output URI.
		/// </summary>
		/// <value>
		/// The output URI.
		/// </value>
		public Uri OutputUri { get { return new Uri(Output); } }

		#endregion
	}
}
