#region Copyright and License

// Copyright (c) 2011, Moonfire Games
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

using System;

#endregion

namespace AuthorIntrusion.Contracts.Processes
{
	/// <summary>
	/// Defines the various types of paragraph processes. Each one impacts the
	/// paragraph in different manners. This enumeration is used to limit
	/// processes to maintain speed when only updating the visual data of
	/// the paragraph is needed. 
	/// </summary>
	[Flags]
	public enum ProcessTypes
	{
		/// <summary>
		/// No processing is needed or required.
		/// </summary>
		None,

		/// <summary>
		/// Processes that impact the structure of the ContentList, such as ones
		/// that create phrases or re-arrange the contents.
		/// </summary>
		Parse = 1,

		/// <summary>
		/// Processes that do not impact the structure, but analyze and add
		/// information about the various elements in the content list.
		/// </summary>
		Analyze = 2,

		/// <summary>
		/// Processes that only change the severity or display information about
		/// the paragraph.
		/// </summary>
		Report = 4,

		/// <summary>
		/// Represents all types of processes.
		/// </summary>
		All = Parse | Analyze | Report,
	}
}