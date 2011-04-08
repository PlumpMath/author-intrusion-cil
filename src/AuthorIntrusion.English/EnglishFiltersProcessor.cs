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

using AuthorIntrusion.Contracts.Processors;

using C5;

#endregion

namespace AuthorIntrusion.English
{
	/// <summary>
	/// Represents the settings for an English filter settings.
	/// </summary>
	public class EnglishFiltersProcessor : Processor
	{
		#region Information

		/// <summary>
		/// Processors are identified by a UUID. For singleton processors, this
		/// MUST return the same <see cref="Guid"/> every time since it is used
		/// to recover settings for the project.
		/// </summary>
		/// <value>The processor key.</value>
		public override Guid ProcessorKey
		{
			get { return new Guid("229793C8-9EF8-4F5C-A6BF-A727BE022E3A"); }
		}

		/// <summary>
		/// Contains a list of services or processors that this item provides.
		/// This are analogous to the Requires to build up a dependency graph
		/// between the various providers.
		/// </summary>
		public override ICollection<string> Provides
		{
			get
			{
				var list = new ArrayList<string>();
				list.Add("English Structure Filters");
				return list;
			}
		}

		/// <summary>
		/// Contains a list of processors required to use this processor. These
		/// are string values as returned by the Provides of another processor.
		/// Circular references are not allow and will be disabled.
		/// </summary>
		public override ICollection<string> Requires
		{
			get
			{
				var list = new ArrayList<string>();
				list.Add("English Structure Tagging");
				return list;
			}
		}

		#endregion

		#region Processing

		/// <summary>
		/// Processes information using the given context.
		/// </summary>
		/// <param name="context">The context.</param>
		public override void Process(ProcessorContext context)
		{
		}

		#endregion
	}
}