// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using Antlr4.StringTemplate;
using C5;

namespace AuthorIntrusion.Common.Projects
{
	/// <summary>
	/// Defines the macro storage and expansion class. This handles the various
	/// macro variables
	/// </summary>
	public class ProjectMacros
	{
		#region Properties

		/// <summary>
		/// Gets the macros and their values.
		/// </summary>
		public HashDictionary<string, string> Substitutions { get; private set; }

		#endregion

		#region Methods

		/// <summary>
		/// Expands the macros in the given string using the variables stored inside
		/// this object. The resulting string will have the macros expanded.
		/// </summary>
		/// <param name="input">The input string.</param>
		/// <returns>The output string with macros expanded.</returns>
		public string ExpandMacros(string input)
		{
			// We have to repeatedly run through the macros since we have macros
			// that expand into other macros.
			string results = input;

			while (results.IndexOf('<') >= 0)
			{
				// Create a template with all of the variables inside it.
				var template = new Template(results, '<', '>');

				foreach (KeyValuePair<string, string> macro in Substitutions)
				{
					template.Add(macro.Key, macro.Value);
				}

				// Render out the template to the results.
				results = template.Render();
			}

			// Return the resulting string.
			return results;
		}

		#endregion

		#region Constructors

		public ProjectMacros()
		{
			Substitutions = new HashDictionary<string, string>();
		}

		#endregion
	}
}
