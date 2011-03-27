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

using MfGames.Reporting;

#endregion

namespace AuthorIntrusion.Contracts.Interfaces
{
	/// <summary>
	/// Defines a tag that can be attached to an element.
	/// </summary>
	public interface IElementTag
	{
		#region Tags

		/// <summary>
		/// Gets the provider for the element tag. In most cases, this will be
		/// a fully qualified type name, but can contain other elements. It is
		/// used to associate tags back with their provider while still allowing
		/// for classes to take over tags of other providers.
		/// </summary>
		/// <value>The provider.</value>
		string Provider { get; }

		/// <summary>
		/// Gets the severity of the element tag.
		/// </summary>
		/// <value>The severity.</value>
		Severity Severity { get; }

		#endregion
	}
}