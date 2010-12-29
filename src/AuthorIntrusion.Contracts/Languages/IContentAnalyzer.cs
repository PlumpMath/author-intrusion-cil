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

using System.Collections.Generic;

using AuthorIntrusion.Contracts.Enumerations;
using AuthorIntrusion.Contracts.Structures;

#endregion

namespace AuthorIntrusion.Contracts.Languages
{
	/// <summary>
	/// Defines the signature for an analyzer, a class that parses through
	/// the content in a non-changing way and adds various tags.
	/// </summary>
	public interface IContentAnalyzer
	{
		#region Analyzing

		/// <summary>
		/// Analyzes the given paragraph and adds various tags as needed.
		/// </summary>
		/// <param name="paragraph">The paragraph.</param>
		/// <returns>The status result from the analysis.</returns>
		ProcessStatus Analyze(Paragraph paragraph);

		#endregion

		#region Dependencies

		/// <summary>
		/// Gets the analyzers this specific analyzer provides.
		/// </summary>
		/// <value>The provides analyzers.</value>
		/// <remarks>This is never null.</remarks>
		HashSet<string> ProvidesAnalyzers { get; }

		/// <summary>
		/// Gets the analyzers that must have successfully run before this
		/// analyzer can process. If the analyzer never has its requirements
		/// fulfilled, it will be noted.
		/// </summary>
		/// <value>The require analyzers.</value>
		/// <remarks>This is never null.</remarks>
		HashSet<string> RequireAnalyzers { get; }

		#endregion
	}
}