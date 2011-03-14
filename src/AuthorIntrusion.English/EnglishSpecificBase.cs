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

using AuthorIntrusion.Contracts.Languages;

#endregion

namespace AuthorIntrusion.English
{
	/// <summary>
	/// Base class that handles the common processing for English classes.
	/// </summary>
	public abstract class EnglishSpecificBase : ILanguageSpecific
	{
		#region Languages

		/// <summary>
		/// Gets the language codes for this element. The codes are ordered in
		/// terms of most specific to least specific and correspond to ISO 639-3
		/// (3 character) and ISO 3166-1 alpha-3 codes. This is used to ensure
		/// that all the codes are identical.
		/// 
		/// For example, a parser for English may provide "eng-USA" and "eng",
		/// but one specific to only US English would only return "eng-USA".
		/// </summary>
		/// <value>The language codes.</value>
		public string[] LanguageCodes
		{
			get { return new[] { "eng" }; }
		}

		#endregion
	}
}